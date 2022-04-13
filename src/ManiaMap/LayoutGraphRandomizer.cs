using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for generating randomized variations in a LayoutGraph.
    /// </summary>
    public class LayoutGraphRandomizer : IGenerationStep
    {
        /// <summary>
        /// Creates a randomized copy of a layout graph and adds it to the artifacts.
        /// 
        /// The following arguments are required:
        /// * %LayoutGraph - The layout graph.
        /// * %RandomSeed - The random seed.
        /// 
        /// The following entries are added to the artifacts dictionary:
        /// * %LayoutGraph - The randomized layout graph.
        /// </summary>
        /// <param name="args">The pipeline arguments dictionary.</param>
        /// <param name="artifacts">The pipeline artifacts dictionary.</param>
        public void ApplyStep(Dictionary<string, object> args, Dictionary<string, object> artifacts)
        {
            var randomSeed = GenerationPipeline.GetArgument<RandomSeed>("RandomSeed", args, artifacts);
            var graph = GenerationPipeline.GetArgument<LayoutGraph>("LayoutGraph", args, artifacts);
            artifacts["LayoutGraph"] = RandomizeLayout(graph, randomSeed);
        }

        /// <summary>
        /// Returns a randomized copy of the layout graph based on the available variations.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="randomSeed">The random seed.</param>
        public LayoutGraph RandomizeLayout(LayoutGraph graph, RandomSeed randomSeed)
        {
            return graph.GetVariation(randomSeed);
        }
    }
}
