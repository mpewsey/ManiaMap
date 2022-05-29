using System;

namespace MPewsey.ManiaMap.Exceptions
{
    /// <summary>
    /// Exception raised when no template group is assigned to a room source.
    /// </summary>
    public class NoTemplateGroupAssignedException : Exception
    {
        public NoTemplateGroupAssignedException(string message) : base(message)
        {

        }
    }
}
