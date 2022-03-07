namespace MPewsey.ManiaMap
{
    /// <summary>
    /// The layout edge direction.
    /// </summary>
    public enum EdgeDirection
    {
        /// An edge pointing in both directions.
        Both,
        /// An edge pointing forward and where reverse traversal is possible after conditions are met.
        ForwardFlexible,
        /// An edge pointing permanently forward.
        ForwardFixed,
        /// An edge poining backward and where forward traversal is possible after conditions are met.
        ReverseFlexible,
        /// An edge pointing permanently backward.
        ReverseFixed,
    }
}
