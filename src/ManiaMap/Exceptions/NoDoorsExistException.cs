using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception raised when a no doors exist.
    /// </summary>
    public class NoDoorsExistException : Exception
    {
        public NoDoorsExistException(string message) : base(message)
        {

        }
    }
}
