using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using MPewsey.Common.Pipelines;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// A class for distributing collectables throughout a Layout.
    /// 
    /// References
    /// ----------
    /// * [1] Pewsey, Matt (2022, April 19). An Approach to Randomly Distributing Game Collectables. Retrieved April 19, 2022, from https://mpewsey.github.io/2022/04/19/collectable-distribution-algorithm.html.
    /// </summary>
    public class CollectableGenerator : IPipelineStep
    {
        /// <summary>
        /// The initial neighbor weight.
        /// </summary>
        public int InitialNeighborWeight { get; set; }

        /// <summary>
        /// The exponent used for the door weight.
        /// </summary>
        public float DoorPower { get; set; }

        /// <summary>
        /// The exponent used for the neighbor weight.
        /// </summary>
        public float NeighborPower { get; set; }

        /// <summary>
        /// The layout.
        /// </summary>
        private Layout Layout { get; set; }

        /// <summary>
        /// The collectable groups.
        /// </summary>
        private CollectableGroups CollectableGroups { get; set; }

        /// <summary>
        /// The random seed.
        /// </summary>
        private RandomSeed RandomSeed { get; set; }

        /// <summary>
        /// A list of collectable spots.
        /// </summary>
        private List<CollectableSpotWeight> CollectableSpotWeights { get; set; }

        /// <summary>
        /// A dictionary of distances by local position and room template.
        /// </summary>
        private Dictionary<RoomTemplate, Dictionary<Vector2DInt, Array2D<int>>> Distances { get; set; }

        /// <summary>
        /// A dictionary of clusters by room ID.
        /// </summary>
        private Dictionary<Uid, HashSet<Uid>> Clusters { get; set; }

        /// /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="doorPower">The exponent used for the door weight.</param>
        /// <param name="neighborPower">The exponent used for the neighbor weight.</param>
        /// <param name="initialNeighborWeight">The initial neighbor weight.</param>
        public CollectableGenerator(float doorPower = 2, float neighborPower = 1, int initialNeighborWeight = 1000)
        {
            DoorPower = doorPower;
            NeighborPower = neighborPower;
            InitialNeighborWeight = initialNeighborWeight;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"CollectableGenerator(DoorPower = {DoorPower}, NeighborPower = {NeighborPower}, InitialNeighborWeight = {InitialNeighborWeight})";
        }

        /// <summary>
        /// Adds collectables to the layout.
        /// 
        /// The following arguments are required:
        /// * %Layout - The room layout.
        /// * %CollectableGroups - The collectable groups.
        /// * %RandomSeed - The random seed.
        /// </summary>
        /// <param name="results">The pipeline results.</param>
        /// <param name="logger">The logging action. Ignored if null.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public bool ApplyStep(PipelineResults results, Action<string> logger, CancellationToken cancellationToken)
        {
            var layout = results.GetArgument<Layout>("Layout");
            var collectableGroups = results.GetArgument<CollectableGroups>("CollectableGroups");
            var randomSeed = results.GetArgument<RandomSeed>("RandomSeed");
            Generate(layout, collectableGroups, randomSeed, logger);
            return true;
        }

        /// <summary>
        /// Initializes the generator's buffers.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="collectableGroups">The collectable groups.</param>
        /// <param name="randomSeed">The random seed.</param>
        private void Initialize(Layout layout, CollectableGroups collectableGroups, RandomSeed randomSeed)
        {
            Layout = layout;
            CollectableGroups = collectableGroups;
            RandomSeed = randomSeed;
            Distances = new Dictionary<RoomTemplate, Dictionary<Vector2DInt, Array2D<int>>>();
            Clusters = Layout.FindClusters(1);
        }

        /// <summary>
        /// Adds collectables to the layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="collectableGroups">The collectable groups.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="logger">The logging action. Ignored if null.</param>
        public void Generate(Layout layout, CollectableGroups collectableGroups, RandomSeed randomSeed, Action<string> logger = null)
        {
            logger?.Invoke("[Collectable Generator] Running collectable generator...");
            Initialize(layout, collectableGroups, randomSeed);

            CreateCollectableSpotWeights();
            AssignDoorWeights();
            AssignInitialNeighborWeights();

            foreach (var collectable in GetCollectables())
            {
                AddCollectable(collectable.Group, collectable.Id);
            }

            logger?.Invoke("[Collectable Generator] Collectable generator complete.");
        }

        /// <summary>
        /// Returns a new list of shuffled collectables.
        /// </summary>
        private List<Collectable> GetCollectables()
        {
            var collectables = CollectableGroups.GetCollectables();
            RandomSeed.Shuffle(collectables);
            return collectables;
        }

        /// <summary>
        /// Adds the collectable to the layout.
        /// </summary>
        /// <param name="group">The collectable group.</param>
        /// <param name="id">The collectable ID.</param>
        /// <exception cref="CollectableSpotNotFoundException">Raised if a collectable spot can not be found for the group.</exception>
        private void AddCollectable(string group, int id)
        {
            // Draw a collectable spot and assign the collectable to the room.
            var weights = GetWeights(group);
            var index = RandomSeed.DrawWeightedIndex(weights);

            if (index < 0)
                throw new CollectableSpotNotFoundException($"Failed to find collectable spot for group: {group}.");

            var spot = CollectableSpotWeights[index];
            CollectableSpotWeights.RemoveAt(index);
            var room = Layout.Rooms[spot.Room];
            room.Collectables.Add(spot.Id, id);

            // Update the draw weights of neighboring spots
            UpdateNeighborWeights(spot);
        }

        /// <summary>
        /// Updates the neighbor weights of the collectable spots in the same
        /// or neighboring rooms.
        /// </summary>
        /// <param name="spot">The collectable spot.</param>
        private void UpdateNeighborWeights(CollectableSpotWeight spot)
        {
            var cluster = Clusters[spot.Room];

            foreach (var other in CollectableSpotWeights)
            {
                if (!cluster.Contains(other.Room))
                    continue;

                if (other.Room == spot.Room)
                {
                    // Spots are in the same room. Use distance between collectables.
                    var room = Layout.Rooms[spot.Room];
                    var distance = GetDistance(room, spot.CollectableSpot.Position, other.CollectableSpot.Position);
                    other.NeighborWeight = Math.Min(distance, other.NeighborWeight);
                }
                else
                {
                    // Spots are in different rooms. Use total distance to connecting doors.
                    var connection = Layout.GetDoorConnection(spot.Room, other.Room);
                    var fromRoom = Layout.Rooms[connection.FromRoom];
                    var toRoom = Layout.Rooms[connection.ToRoom];
                    var fromPosition = spot.CollectableSpot.Position;
                    var toPosition = other.CollectableSpot.Position;

                    if (connection.ToRoom == spot.Room)
                        (fromPosition, toPosition) = (toPosition, fromPosition);

                    var distance = GetDistance(fromRoom, connection.FromDoor.Position, fromPosition);
                    distance += GetDistance(toRoom, connection.ToDoor.Position, toPosition);
                    other.NeighborWeight = Math.Min(distance, other.NeighborWeight);
                }
            }
        }

        /// <summary>
        /// Returns a new array of draw weights for the collectable spots.
        /// </summary>
        /// <param name="group">The group name.</param>
        private double[] GetWeights(string group)
        {
            var weights = new double[CollectableSpotWeights.Count];

            for (int i = 0; i < weights.Length; i++)
            {
                var spot = CollectableSpotWeights[i];

                if (spot.CollectableSpot.Group == group)
                    weights[i] = spot.GetWeight(DoorPower, NeighborPower);
            }

            return weights;
        }

        /// <summary>
        /// Creates a new list of collectable spots.
        /// </summary>
        private void CreateCollectableSpotWeights()
        {
            CollectableSpotWeights = new List<CollectableSpotWeight>();

            foreach (var room in Layout.Rooms.Values)
            {
                CreateRoomCollectableSpotWeights(room);
            }
        }

        /// <summary>
        /// Adds new collectable spots for the room.
        /// </summary>
        /// <param name="room">The room.</param>
        private void CreateRoomCollectableSpotWeights(Room room)
        {
            var cells = room.Template.Cells;

            foreach (var pair in room.Template.CollectableSpots.OrderBy(x => x.Key))
            {
                if (cells.GetOrDefault(pair.Value.Position) != null)
                    CollectableSpotWeights.Add(new CollectableSpotWeight(room.Id, pair.Key, pair.Value));
            }
        }

        /// <summary>
        /// Assigns the door weights to the collectable spots.
        /// </summary>
        private void AssignDoorWeights()
        {
            var doors = Layout.GetRoomDoors();

            foreach (var spot in CollectableSpotWeights)
            {
                var room = Layout.Rooms[spot.Room];

                foreach (var door in doors[spot.Room])
                {
                    var distance = GetDistance(room, door.Position, spot.CollectableSpot.Position);
                    spot.DoorWeight = Math.Min(distance, spot.DoorWeight);
                }
            }
        }

        /// <summary>
        /// Assigns the initial neighbor weight to the collectable spots.
        /// </summary>
        private void AssignInitialNeighborWeights()
        {
            foreach (var spot in CollectableSpotWeights)
            {
                spot.NeighborWeight = InitialNeighborWeight;
            }
        }

        /// <summary>
        /// Returns the distance between the two points in the room.
        /// This method attempts to use cached distance matrix results if it can.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="index1">The first point.</param>
        /// <param name="index2">the second point.</param>
        private int GetDistance(Room room, Vector2DInt index1, Vector2DInt index2)
        {
            if (!Distances.TryGetValue(room.Template, out var positions))
            {
                positions = new Dictionary<Vector2DInt, Array2D<int>>();
                Distances.Add(room.Template, positions);
            }

            if (positions.TryGetValue(index1, out var distances))
                return distances[index2.X, index2.Y];
            if (positions.TryGetValue(index2, out distances))
                return distances[index1.X, index1.Y];

            distances = room.Template.FindCellDistances(index1);
            positions.Add(index1, distances);
            return distances[index2.X, index2.Y];
        }
    }
}
