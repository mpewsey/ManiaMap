using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for collectable spot not found errors.
    /// </summary>
    public class CollectableSpotNotFoundException : Exception
    {
        public CollectableSpotNotFoundException(string message) : base(message)
        {

        }
    }
}
