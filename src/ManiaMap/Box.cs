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

        /// <summary>
        /// Initializes a new box from two vectors.
        /// </summary>
        /// <param name="min">The minimum range of the box.</param>
        /// <param name="max">The maximum range of the box.</param>
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
            return position.Z >= Min.Z
                && position.Z <= Max.Z
                && template.Intersects(Min - position, Max - position);
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
    }
}
