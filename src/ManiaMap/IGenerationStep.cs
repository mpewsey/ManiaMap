using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// An interface for creating a step of the GenerationPipeline.
    /// </summary>
    public interface IGenerationStep
    {
        /// <summary>
        /// Performs the generation operations for this step.
        /// Artifacts should be written to the input dictionary.
        /// </summary>
        /// <param name="args">A dictionary of arguments.</param>
        /// <param name="artifacts">A dictionary of generator artifacts.</param>
        void Generate(Dictionary<string, object> args, Dictionary<string, object> artifacts);
    }
}
