using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for chaining multiple generation stages together.
    /// </summary>
    public class GenerationPipeline
    {
        /// <summary>
        /// A list of generation stages.
        /// </summary>
        public List<IGenerationStep> Steps { get; set; } = new List<IGenerationStep>();

        public GenerationPipeline(params IGenerationStep[] steps)
        {
            Steps.AddRange(steps);
        }

        /// <summary>
        /// Returns a new pipeline with the default generators.
        /// </summary>
        public static GenerationPipeline CreateDefaultPipeline()
        {
            return new GenerationPipeline(
                new LayoutGenerator(),
                new CollectableGenerator());
        }

        /// <summary>
        /// Invokes all generators of the pipeline and returns a dictionary of results.
        /// </summary>
        /// <param name="map">A dictionary of generator arguments.</param>
        public Dictionary<string, object> Generate(Dictionary<string, object> map)
        {
            map = new Dictionary<string, object>(map);
            Steps.ForEach(x => x.Generate(map));
            return map;
        }
    }
}
