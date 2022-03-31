namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Settings for the `LayoutGenerator`.
    /// </summary>
    public class LayoutGeneratorSettings
    {
        /// <summary>
        /// The maximum number of times that a sub layout can be used as a base before it is discarded.
        /// </summary>
        public int MaxRebases { get; set; }

        /// <summary>
        /// The maximum branch chain length. Branch chains exceeding this length will be split.
        /// Negative and zero values will be ignored.
        /// </summary>
        public int MaxBranchLength { get; set; }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        /// <param name="maxRebases">The maximum number of times that a sub layout can be used as a base before it is discarded.</param>
        /// <param name="maxBranchLength">The maximum branch chain length. Branch chains exceeding this length will be split. Negative and zero values will be ignored.</param>
        public LayoutGeneratorSettings(int maxRebases = 100, int maxBranchLength = -1)
        {
            MaxRebases = maxRebases;
            MaxBranchLength = maxBranchLength;
        }

        public override string ToString()
        {
            return $"LayoutGeneratorSettings(MaxRebases = {MaxRebases}, MaxBranchLength = {MaxBranchLength})";
        }
    }
}
