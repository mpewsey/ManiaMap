using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception thrown when cells are not fully connected.
    /// </summary>
    public class CellsNotFullyConnectedException : Exception
    {
        /// <inheritdoc/>
        public CellsNotFullyConnectedException(string message) : base(message)
        {

        }
    }
}
