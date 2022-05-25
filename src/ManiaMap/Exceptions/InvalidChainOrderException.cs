using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for invalid chain order errors.
    /// </summary>
    public class InvalidChainOrderException : Exception
    {
        public InvalidChainOrderException(string message) : base(message)
        {

        }
    }
}
