using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class referencing two RoomTemplate and a list of all valid Configuration between them.
    /// </summary>
    public class ConfigurationSpace
    {
        /// <summary>
        /// The from room template.
        /// </summary>
        public RoomTemplate FromTemplate { get; }

        /// <summary>
        /// The to room template.
        /// </summary>
        public RoomTemplate ToTemplate { get; }

        /// <summary>
        /// A list of all valid configurations between the room templates.
        /// </summary>
        public List<Configuration> Configurations { get; } = new List<Configuration>();

        /// <summary>
        /// Initializes a configuration space from two room templates.
        /// </summary>
        /// <param name="from">The from template.</param>
        /// <param name="to">The to template.</param>
        public ConfigurationSpace(RoomTemplate from, RoomTemplate to)
        {
            FromTemplate = from;
            ToTemplate = to;
            FindConfigurations();
        }

        public override string ToString()
        {
            return $"ConfigurationSpace(FromTemplate = {FromTemplate}, ToTemplate = {ToTemplate})";
        }

        /// <summary>
        /// Finds all room configurations that are valid between the room templates.
        /// </summary>
        private void FindConfigurations()
        {
            Configurations.Clear();

            for (int i = -ToTemplate.Cells.Rows; i <= FromTemplate.Cells.Rows; i++)
            {
                for (int j = -ToTemplate.Cells.Columns; j <= FromTemplate.Cells.Columns; j++)
                {
                    var position = new Vector2DInt(i, j);
                    var doorPairs = FromTemplate.AlignedDoors(ToTemplate, position);

                    foreach (var pair in doorPairs)
                    {
                        var config = new Configuration(position, pair.FromDoor, pair.ToDoor);
                        Configurations.Add(config);
                    }
                }
            }
        }
    }
}
