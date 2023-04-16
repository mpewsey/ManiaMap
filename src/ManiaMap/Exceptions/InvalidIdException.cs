using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for invalid ID errors.
    /// </summary>
    public class InvalidIdException : Exception
    {
        /// <inheritdoc/>
        public InvalidIdException(string message) : base(message)
        {

        }
    }
}
