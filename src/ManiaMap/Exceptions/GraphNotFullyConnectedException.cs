using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception thrown when a graph is not fully connected.
    /// </summary>
    public class GraphNotFullyConnectedException : Exception
    {
        /// <inheritdoc/>
        public GraphNotFullyConnectedException(string message) : base(message)
        {

        }
    }
}
