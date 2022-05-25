using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception for unhandled switch case exceptions.
    /// </summary>
    public class UnhandledCaseException : Exception
    {
        public UnhandledCaseException(string message) : base(message)
        {

        }
    }
}
