using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains the states of a `Layout`.
    /// </summary>
    [DataContract]
    public class LayoutState
    {
        /// <summary>
        /// The ID of the corresponding layout.
        /// </summary>
        [DataMember(Order = 0)]
        public int Id { get; private set; }

        /// <summary>
        /// A dictionary of room states by ID.
        /// </summary>
        [DataMember(Order = 1)]
        public Dictionary<Uid, RoomState> RoomStates { get; private set; } = new Dictionary<Uid, RoomState>();

        /// <summary>
        /// Initializes an object from a layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        public LayoutState(Layout layout)
        {
            Id = layout.Id;
            RoomStates = layout.Rooms.Values.ToDictionary(x => x.Id, x => new RoomState(x));
        }

        /// <summary>
        /// Saves the layout state for a file using the DataContractSerializer.
        /// </summary>
        /// <param name="path">The save file path.</param>
        public void SaveXml(string path)
        {
            var serializer = new DataContractSerializer(GetType());

            using (var stream = File.Create(path))
            {
                serializer.WriteObject(stream, this);
            }
        }

        /// <summary>
        /// Loads a layout state from a file using the DataContractSerializer.
        /// </summary>
        /// <param name="path">he file path.</param>
        public static LayoutState LoadXml(string path)
        {
            var serializer = new DataContractSerializer(typeof(LayoutState));

            using (var stream = File.OpenRead(path))
            {
                return (LayoutState)serializer.ReadObject(stream);
            }
        }
    }
}
