using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class representing a box in 3D space. The box is defined by integer coordinate ranges.
    /// </summary>
    [DataContract]
    public class Box
    {
        /// <summary>
        /// The minimum x value.
        /// </summary>
        [DataMember(Order = 1)]
        public int XMin { get; private set; }

        /// <summary>
        /// The maximum x value.
        /// </summary>
        [DataMember(Order = 2)]
        public int XMax { get; private set; }

        /// <summary>
        /// The minimum y value.
        /// </summary>
        [DataMember(Order = 3)]
        public int YMin { get; private set; }

        /// <summary>
        /// The maximum y value.
        /// </summary>
        [DataMember(Order = 4)]
        public int YMax { get; private set; }

        /// <summary>
        /// The minimum z value.
        /// </summary>
        [DataMember(Order = 5)]
        public int ZMin { get; private set; }

        /// <summary>
        /// The maximum z value.
        /// </summary>
        [DataMember(Order = 6)]
        public int ZMax { get; private set; }

        /// <summary>
        /// Initializes the Box by range values.
        /// </summary>
        /// <param name="xMin">The minimum x value.</param>
        /// <param name="xMax">The maximum x value.</param>
        /// <param name="yMin">The minimum y value.</param>
        /// <param name="yMax">The maximum y value.</param>
        /// <param name="zMin">The minimum z value.</param>
        /// <param name="zMax">The maximum z value.</param>
        public Box(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
            ZMin = zMin;
            ZMax = zMax;
        }

        public override string ToString()
        {
            return $"Box(XMin = {XMin}, XMax = {XMax}, YMin = {YMin}, YMax = {YMax}, ZMin = {ZMin}, ZMax = {ZMax})";
        }

        /// <summary>
        /// Returns true if the room template intersects the Box.
        /// </summary>
        /// <param name="template">The room template.</param>
        /// <param name="x">The x position of the template.</param>
        /// <param name="y">The y position of the template.</param>
        /// <param name="z">The z position of the template.</param>
        public bool Intersects(RoomTemplate template, int x, int y, int z)
        {
            if (z >= ZMin && z <= ZMax)
            {
                var x1 = XMin - x;
                var x2 = XMax - x;
                var y1 = YMin - y;
                var y2 = YMax - y;

                if (template.Intersects(x1, x2, y1, y2))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the range intersects the Box.
        /// </summary>
        /// <param name="xMin">The minimum x value.</param>
        /// <param name="xMax">The maximum x value.</param>
        /// <param name="yMin">The minimum y value.</param>
        /// <param name="yMax">The maximum y value.</param>
        /// <param name="zMin">The minimum z value.</param>
        /// <param name="zMax">The maximum z value.</param>
        public bool Intersects(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
        {
            return xMin <= XMax && xMax >= XMin
                && yMin <= YMax && yMax >= YMin
                && zMin <= ZMax && zMax >= ZMin;
        }
    }
}
