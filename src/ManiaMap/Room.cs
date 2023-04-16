﻿using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using MPewsey.Common.Random;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a room in a Layout.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class Room : IDataContractValueDictionaryValue<Uid>
    {
        /// <summary>
        /// The room ID.
        /// </summary>
        [DataMember(Order = 0, IsRequired = true)]
        public Uid Id { get; private set; }

        /// <summary>
        /// The room name.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The position of the room.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public Vector3DInt Position { get; set; }

        /// <summary>
        /// The random seed of the room that may be used for room specific generation.
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        public int Seed { get; set; }

        private int _templateId;
        /// <summary>
        /// The template ID.
        /// </summary>
        [DataMember(Order = 6, IsRequired = true)]
        public int TemplateId
        {
            get => Template != null ? Template.Id : _templateId;
            private set => _templateId = value;
        }

        /// <summary>
        /// The room template.
        /// </summary>
        public RoomTemplate Template { get; private set; }

        /// <summary>
        /// The room color.
        /// </summary>
        [DataMember(Order = 7, IsRequired = true)]
        public Color4 Color { get; set; }

        /// <summary>
        /// A dictionary of collectable object ID's by location ID.
        /// </summary>
        [DataMember(Order = 8, IsRequired = true)]
        public DataContractDictionary<int, int> Collectables { get; private set; } = new DataContractDictionary<int, int>();

        /// <inheritdoc/>
        public Uid Key => Id;

        /// <summary>
        /// Initializes a room from a room source.
        /// </summary>
        /// <param name="source">The room source.</param>
        /// <param name="position">The position in the layout.</param>
        /// <param name="template">The room template.</param>
        /// <param name="random">The random seed.</param>
        public Room(IRoomSource source, Vector2DInt position, RoomTemplate template, RandomSeed random)
        {
            Id = source.RoomId;
            Name = source.Name;
            Position = new Vector3DInt(position.X, position.Y, source.Z);
            Seed = random.Next();
            Color = source.Color;
            Template = template;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Room(Id = {Id}, Name = {Name}, Position = {Position}, Template = {Template})";
        }

        /// <summary>
        /// Returns true if the range intersects the room.
        /// </summary>
        /// <param name="min">The minimum values of the range.</param>
        /// <param name="max">The maximum values of the range.</param>
        public bool Intersects(Vector3DInt min, Vector3DInt max)
        {
            return Position.Z >= min.Z
                && Position.Z <= max.Z
                && Template.Intersects((Vector2DInt)(min - Position), (Vector2DInt)(max - Position));
        }

        /// <summary>
        /// Sets the room template based on the current template ID.
        /// </summary>
        /// <param name="templates">A dictionary of room templates.</param>
        public void SetTemplate(Dictionary<int, RoomTemplate> templates)
        {
            var template = templates[TemplateId];
            TemplateId = template.Id;
            Template = template;
        }

        /// <summary>
        /// Returns the ratio of visible cells over total cells.
        /// </summary>
        /// <param name="state">The room state.</param>
        public float VisibleCellProgress(RoomState state)
        {
            var counts = VisibleCellCount(state);
            return counts.Y > 0 ? counts.X / (float)counts.Y : 1;
        }

        /// <summary>
        /// Returns a vector with of the visible cell count (X) and total cell count (Y).
        /// </summary>
        /// <param name="state">The room state.</param>
        public Vector2DInt VisibleCellCount(RoomState state)
        {
            var cellCount = 0;
            var visibleCount = 0;
            var cells = Template.Cells;
            var visibleCells = state.VisibleCells;

            for (int i = 0; i < cells.Rows; i++)
            {
                for (int j = 0; j < cells.Columns; j++)
                {
                    if (cells[i, j] != null)
                    {
                        if (visibleCells[i, j])
                            visibleCount++;
                        cellCount++;
                    }
                }
            }

            return new Vector2DInt(visibleCount, cellCount);
        }
    }
}
