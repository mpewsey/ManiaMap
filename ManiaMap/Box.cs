using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Box
    {
        [DataMember(Order = 1)]
        public int XMin { get; private set; }

        [DataMember(Order = 2)]
        public int XMax { get; private set; }

        [DataMember(Order = 3)]
        public int YMin { get; private set; }

        [DataMember(Order = 4)]
        public int YMax { get; private set; }

        [DataMember(Order = 5)]
        public int ZMin { get; private set; }

        [DataMember(Order = 6)]
        public int ZMax { get; private set; }

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
            return $"Shaft(XMin = {XMin}, XMax = {XMax}, YMin = {YMin}, YMax = {YMax}, ZMin = {ZMin}, ZMax = {ZMax})";
        }

        /// <summary>
        /// Returns true if the tempalte intersects the shaft.
        /// </summary>
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
        /// Returns true if the range intersects the shaft.
        /// </summary>
        public bool Intersects(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
        {
            return xMin <= XMax && xMax >= XMin
                && yMin <= YMax && yMax >= YMin
                && zMin <= ZMax && zMax >= XMin;
        }
    }
}
