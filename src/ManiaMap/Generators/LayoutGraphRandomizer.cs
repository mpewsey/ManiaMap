using MPewsey.Common.Logging;
using MPewsey.Common.Pipelines;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Graphs;
using System.Threading;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// A class for generating randomized variations in a LayoutGraph.
    /// </summary>
    public class LayoutGraphRandomizer : IPipelineStep
    {
        /// <summary>
        /// Creates a randomized copy of a layout graph and adds it to the results output dictionary.
        /// 
        /// The following arguments are required:
        /// * %LayoutGraph - The layout graph.
        /// * %RandomSeed - The random seed.
        /// 
        /// The following entries are added to the results output dictionary:
        /// * %LayoutGraph - The randomized layout graph.
        /// </summary>
        /// <param name="results">The pipeline results.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public bool ApplyStep(PipelineResults results, CancellationToken cancellationToken)
        {
            var randomSeed = results.GetArgument<RandomSeed>("RandomSeed");
            var graph = results.GetArgument<LayoutGraph>("LayoutGraph");
            results.SetOutput("LayoutGraph", RandomizeGraph(graph, randomSeed));
            return true;
        }

        /// <summary>
        /// Returns a randomized copy of the layout graph based on the available variations.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="randomSeed">The random seed.</param>
        public LayoutGraph RandomizeGraph(LayoutGraph graph, RandomSeed randomSeed)
        {
            Logger.Log("[Layout Graph Randomizer] Running layout graph randomizer...");
            var result = graph.GetVariation(randomSeed);
            Logger.Log("[Layout Graph Randomizer] Layout graph randomizer complete.");
            return result;
        }
    }
}
