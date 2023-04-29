using System;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// A class containing data for a collectable location.
    /// </summary>
    public class CollectableSpotWeight
    {
        /// <summary>
        /// The room ID.
        /// </summary>
        public Uid Room { get; set; }

        /// <summary>
        /// The location ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The collectable spot.
        /// </summary>
        public CollectableSpot CollectableSpot { get; set; }

        /// <summary>
        /// The door draw weight.
        /// </summary>
        public int DoorWeight { get; set; } = int.MaxValue;

        /// <summary>
        /// The neighboring collectable draw weight.
        /// </summary>
        public int NeighborWeight { get; set; } = int.MaxValue;

        /// <summary>
        /// Initializes a new spot.
        /// </summary>
        /// <param name="room">The room ID.</param>
        /// <param name="id">The location ID.</param>
        /// <param name="spot">The collectble spot.</param>
        public CollectableSpotWeight(Uid room, int id, CollectableSpot spot)
        {
            Room = room;
            Id = id;
            CollectableSpot = spot;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"CollectableSpotWeight(Room = {Room}, Id = {Id}, CollectableSpot = {CollectableSpot})";
        }

        /// <summary>
        /// Returns the draw weight for the spot.
        /// </summary>
        /// <param name="doorPower">The exponent used for the door weight.</param>
        /// <param name="neighborPower">The exponent used for the neighbor weight.</param>
        public double GetWeight(double doorPower, double neighborPower)
        {
            var wd = Math.Pow(DoorWeight + 1, doorPower);
            var wn = Math.Pow(NeighborWeight + 1, neighborPower);
            return wd * wn * CollectableSpot.Weight;
        }
    }
}
