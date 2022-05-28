using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception thrown when a graph is not fully connected.
    /// </summary>
    public class GraphNotFullyConnectedException : Exception
    {
        public GraphNotFullyConnectedException(string message) : base(message)
        {

        }
    }
}
