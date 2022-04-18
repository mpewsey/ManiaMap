using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for chaining multiple generation stages together.
    /// </summary>
    public class GenerationPipeline
    {
        /// <summary>
        /// A container for holding pipeline results.
        /// </summary>
        public class Results
        {
            /// <summary>
            /// A dictionary of pipeline inputs.
            /// </summary>
            public Dictionary<string, object> Inputs { get; set; }

            /// <summary>
            /// A dictionary of pipeline output results.
            /// </summary>
            public Dictionary<string, object> Outputs { get; set; } = new Dictionary<string, object>();

            /// <summary>
            /// True if the pipeline steps are successful.
            /// </summary>
            public bool Success { get; set; } = true;

            /// <summary>
            /// Initializes a new result.
            /// </summary>
            /// <param name="inputs">The input dictionary.</param>
            public Results(Dictionary<string, object> inputs)
            {
                Inputs = inputs;
            }

            /// <summary>
            /// Searches the output and input dictionaries for the key and returns it.
            /// </summary>
            /// <typeparam name="T">The object type.</typeparam>
            /// <param name="key">The dictionary key.</param>
            public T GetArgument<T>(string key)
            {
                if (Outputs.TryGetValue(key, out var value))
                    return (T)value;
                return (T)Inputs[key];
            }
        }

        /// <summary>
        /// A list of generation stages.
        /// </summary>
        public List<IGenerationStep> Steps { get; private set; } = new List<IGenerationStep>();

        /// <summary>
        /// Initializes a new pipeline.
        /// </summary>
        /// <param name="steps">The generation steps in order.</param>
        public GenerationPipeline(params IGenerationStep[] steps)
        {
            Steps.AddRange(steps);
        }

        /// <summary>
        /// Returns a new pipeline with common default generators in the following order:
        /// 
        /// 1. LayoutGraphRandomizer
        /// 2. LayoutGenerator
        /// 3. CollectableGenerator
        /// </summary>
        public static GenerationPipeline CreateDefaultPipeline()
        {
            return new GenerationPipeline(
                new LayoutGraphRandomizer(),
                new LayoutGenerator(),
                new CollectableGenerator());
        }

        /// <summary>
        /// Invokes all generators of the pipeline and returns the results.
        /// </summary>
        /// <param name="inputs">A dictionary of generator inputs.</param>
        public Results Generate(Dictionary<string, object> inputs)
        {
            var results = new Results(inputs);

            foreach (var step in Steps)
            {
                results.Success = false;
                step.ApplyStep(results);

                if (!results.Success)
                    return results;
            }

            return results;
        }
    }
}
