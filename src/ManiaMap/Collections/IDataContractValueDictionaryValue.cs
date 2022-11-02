namespace MPewsey.ManiaMap.Collections
{
    /// <summary>
    /// An interface for values of the DataContractValueDictionary.
    /// </summary>
    public interface IDataContractValueDictionaryValue<T>
    {
        /// <summary>
        /// The unique key.
        /// </summary>
        T Key { get; }
    }
}
