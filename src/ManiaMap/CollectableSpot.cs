using System;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class containing data for a collectable location.
    /// </summary>
    public class CollectableSpot
    {
        /// <summary>
        /// The room ID.
        /// </summary>
        public Uid Room { get; set; }

        /// <summary>
        /// The local position in the room.
        /// </summary>
        public Vector2DInt Position { get; set; }

        /// <summary>
        /// The location ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The collectable group name.
        /// </summary>
        public string Group { get; set; }

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
        /// <param name="position">The local position in the room.</param>
        /// <param name="id">The location ID.</param>
        /// <param name="group">The collectable group.</param>
        public CollectableSpot(Uid room, Vector2DInt position, int id, string group)
        {
            Room = room;
            Position = position;
            Id = id;
            Group = group;
        }

        public override string ToString()
        {
            return $"CollectableSpot(Room = {Room}, Position = {Position}, Id = {Id}, Group = {Group})";
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
            return wd * wn;
        }
    }
}
