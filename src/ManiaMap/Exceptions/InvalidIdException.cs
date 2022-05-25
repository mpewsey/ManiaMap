using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for invalid ID errors.
    /// </summary>
    public class InvalidIdException : Exception
    {
        public InvalidIdException(string message) : base(message)
        {

        }
    }
}
