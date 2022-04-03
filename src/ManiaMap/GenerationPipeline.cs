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

        /// <summary>
        /// Initializes a new pipeline.
        /// </summary>
        /// <param name="steps">The generation steps in order.</param>
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
        /// <param name="args">A dictionary of generator arguments.</param>
        public Dictionary<string, object> Generate(Dictionary<string, object> args)
        {
            var artifacts = new Dictionary<string, object>();
            Steps.ForEach(x => x.Generate(args, artifacts));
            return artifacts;
        }

        /// <summary>
        /// Returns the argument from the argument and artifacts dictionary.
        /// The artifact dictionary is checked first, then the argument dictionary.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="args">The argument dictionary.</param>
        /// <param name="artifacts">The artifact dictionary.</param>
        public static T GetArgument<T>(string name,
            Dictionary<string, object> args, Dictionary<string, object> artifacts = null)
        {
            if (artifacts != null && artifacts.TryGetValue(name, out var value))
                return (T)value;
            return (T)args[name];
        }
    }
}
