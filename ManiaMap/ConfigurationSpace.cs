using System;
using System.Collections.Generic;
using System.Linq;

namespace ManiaMap
{
    public class ConfigurationSpace
    {
        public RoomTemplate FromTemplate { get; }
        public RoomTemplate ToTemplate { get; }
        public List<Configuration> Configurations { get; } = new List<Configuration>();

        public ConfigurationSpace(RoomTemplate from, RoomTemplate to)
        {
            FromTemplate = from;
            ToTemplate = to;
            FindConfigurations();
        }

        public override string ToString()
        {
            return $"ConfigurationSpace(FromTemplate = {FromTemplate}, ToTemplate = {ToTemplate}, Configurations.Count = {Configurations.Count})";
        }

        /// <summary>
        /// Finds all room configurations that var valid between the room templates.
        /// </summary>
        private void FindConfigurations()
        {
            Configurations.Clear();

            for (int i = -ToTemplate.Cells.Rows; i <= FromTemplate.Cells.Rows; i++)
            {
                for (int j = -ToTemplate.Cells.Columns; j <= FromTemplate.Cells.Columns; j++)
                {
                    FindIndexConfigurations(i, j);
                }
            }
        }

        /// <summary>
        /// Adds all configurations that are valid for the specified template offset.
        /// </summary>
        private void FindIndexConfigurations(int dx, int dy)
        {
            if (!FromTemplate.Intersects(ToTemplate, dx, dy))
            {
                var doorPairs = FromTemplate.AlignedDoors(ToTemplate, dx, dy);

                foreach (var pair in doorPairs)
                {
                    Configurations.Add(new Configuration(dx, dy, pair.FromDoor, pair.ToDoor));
                }
            }
        }
    }
}
