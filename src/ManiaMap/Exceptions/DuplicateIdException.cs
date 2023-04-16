using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for duplicate ID errors.
    /// </summary>
    public class DuplicateIdException : Exception
    {
        /// <inheritdoc/>
        public DuplicateIdException(string message) : base(message)
        {

        }
    }
}
