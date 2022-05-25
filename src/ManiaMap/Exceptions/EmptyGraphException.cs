using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for empty graph errors.
    /// </summary>
    public class EmptyGraphException : Exception
    {
        public EmptyGraphException(string message) : base(message)
        {

        }
    }
}
