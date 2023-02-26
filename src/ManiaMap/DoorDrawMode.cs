namespace MPewsey.ManiaMap
{
    /// <summary>
    /// An option controlling which doors are drawn on maps.
    /// </summary>
    public enum DoorDrawMode
    {
        /// No doors are displayed.
        None,
        /// All doors are displayed.
        AllDoors,
        /// All doors within a layer are displayed. Any doors between layers are not displayed.
        IntralayerDoors,
        /// All doors between layers are displayed. Any doors within a layer are not displayed.
        InterlayerDoors,
    }
}
