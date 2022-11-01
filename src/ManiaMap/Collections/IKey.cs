namespace MPewsey.ManiaMap.Collections
{
    /// <summary>
    /// An interface requiring a unique key.
    /// </summary>
    public interface IKey<T>
    {
        /// <summary>
        /// The unique key.
        /// </summary>
        T Key { get; }
    }
}
