using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for invalid name errors.
    /// </summary>
    public class InvalidNameException : Exception
    {
        /// <inheritdoc/>
        public InvalidNameException(string message) : base(message)
        {

        }
    }
}
