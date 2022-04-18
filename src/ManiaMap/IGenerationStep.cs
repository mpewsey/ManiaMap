namespace MPewsey.ManiaMap
{
    /// <summary>
    /// An interface for creating a step of the GenerationPipeline.
    /// </summary>
    public interface IGenerationStep
    {
        /// <summary>
        /// Performs the generation operations for this step.
        /// Artifacts should be written to the results output dictionary.
        /// </summary>
        /// <param name="results">The generation pipeline results.</param>
        void ApplyStep(GenerationPipeline.Results results);
    }
}
