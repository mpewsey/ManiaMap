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
        /// The minimum index range of the box.
        /// </summary>
        [DataMember(Order = 0)]
        public Vector3DInt Min { get; private set; }

        /// <summary>
        /// The maximum index range of the box.
        /// </summary>
        [DataMember(Order = 1)]
        public Vector3DInt Max { get; private set; }
        
        public Box(Vector3DInt min, Vector3DInt max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"Box(Min = {Min}, Max = {Max})";
        }

        /// <summary>
        /// Returns true if the room template intersects the Box.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="position"></param>
        public bool Intersects(RoomTemplate template, Vector3DInt position)
        {
            if (position.Z >= Min.Z && position.Z <= Max.Z)
            {
                var x1 = Min.X - position.X;
                var x2 = Max.X - position.X;
                var y1 = Min.Y - position.Y;
                var y2 = Max.Y - position.Y;

                if (template.Intersects(x1, x2, y1, y2))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// REMOVE THIS EVENTUALLY.
        /// </summary>
        public bool Intersects(RoomTemplate template, int x, int y, int z)
        {
            return Intersects(template, new Vector3DInt(x, y, z));
        }

        /// <summary>
        /// Returns true if the specified range intersects the box.
        /// </summary>
        /// <param name="min">The minimum values of the range.</param>
        /// <param name="max">The maximum values of the range.</param>
        public bool Intersects(Vector3DInt min, Vector3DInt max)
        {
            return min.X <= Max.X && max.X >= Min.X
                && min.Y <= Max.Y && max.Y >= Min.Y
                && min.Z <= Max.Z && max.Z >= Min.Z;
        }

        /// <summary>
        /// REMOVE THIS EVENTUALLY.
        /// </summary>
        public bool Intersects(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
        {
            return Intersects(new Vector3DInt(xMin, yMin, zMin), new Vector3DInt(xMax, yMax, zMax));
        }
    }
}
