using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{

    /// <summary>
    /// A template group entry, consisting of a RoomTemplate and usage constaints.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class TemplateGroupsEntry
    {
        /// <summary>
        /// The room template.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public RoomTemplate Template { get; private set; }

        private int _minQuantity;
        /// <summary>
        /// The minimum number of uses for the entry.
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public int MinQuantity
        {
            get => _minQuantity;
            set => _minQuantity = Math.Max(value, 0);
        }

        private int _maxQuantity = int.MaxValue;
        /// <summary>
        /// The maximum number of uses for the entry.
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        public int MaxQuantity
        {
            get => _maxQuantity;
            set => _maxQuantity = Math.Max(value, 0);
        }

        /// <summary>
        /// Intializes a new entry with no quantity contraints.
        /// </summary>
        /// <param name="template">The room template.</param>
        public TemplateGroupsEntry(RoomTemplate template)
        {
            Template = template;
        }

        /// <summary>
        /// Initializes a new entry with quantity constraints.
        /// </summary>
        /// <param name="template">The room template.</param>
        /// <param name="minQuantity">The minimum number of uses for the entry.</param>
        /// <param name="maxQuantity">The maximum number of uses for the entry.</param>
        public TemplateGroupsEntry(RoomTemplate template, int minQuantity, int maxQuantity = int.MaxValue)
        {
            Template = template;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
        }

        /// <summary>
        /// Returns true if the constraint quantity is satisfied.
        /// </summary>
        /// <param name="count">The entry count.</param>
        public bool QuantitySatisfied(int count)
        {
            return count >= MinQuantity && count <= MaxQuantity;
        }

        /// <summary>
        /// If the specified room template has the same values as the current template,
        /// sets it as the current template.
        /// </summary>
        /// <param name="other">The specified room template.</param>
        public void ConsolidateTemplate(RoomTemplate other)
        {
            if (RoomTemplate.ValuesAreEqual(Template, other))
                Template = other;
        }
    }
}
