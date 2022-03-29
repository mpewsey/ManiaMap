namespace MPewsey.ManiaMap
{
    /// <summary>
    /// The door type.
    /// </summary>
    public enum DoorType
    {
        /// No door.
        None,
        /// A two-way door.
        TwoWay,
        /// A one-way exit door that, once conditions are met, becomes a two-way door.
        TwoWayExit,
        /// A one-way entrance door that, once conditions are met, becomes a two-way door.
        TwoWayEntrance,
        /// A one-way exit.
        OneWayExit,
        /// A one-way entrance.
        OneWayEntrance,
    }
}
