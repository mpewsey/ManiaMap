using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for drawing a random LayoutGraph from multiple.
    /// </summary>
    public class LayoutGraphSelector : IGenerationStep
    {
        /// <summary>
        /// Draws a random layout graph and adds a copy to the artifacts.
        /// 
        /// The following arguments are required:
        /// * %LayoutGraphs - A list of layout graphs or functions that return layout graphs.
        /// * %RandomSeed - The random seed.
        /// 
        /// The following entries are added to the artifacts dictionary:
        /// * %LayoutGraph - The drawn layout graph.
        /// </summary>
        /// <param name="args">The pipeline arguments dictionary.</param>
        /// <param name="artifacts">The pipeline artifacts dictionary.</param>
        public void ApplyStep(Dictionary<string, object> args, Dictionary<string, object> artifacts)
        {
            var randomSeed = GenerationPipeline.GetArgument<RandomSeed>("RandomSeed", args, artifacts);
            var layouts = GenerationPipeline.GetArgument<object>("LayoutGraphs", args, artifacts);
            artifacts["LayoutGraph"] = DrawSelection(layouts, randomSeed);
        }

        /// <summary>
        /// Returns a copy of a random layout graph from the list.
        /// </summary>
        /// <param name="graphs">A list of layout graphs.</param>
        /// <param name="randomSeed">The random seed.</param>
        public LayoutGraph DrawSelection(IList<LayoutGraph> graphs, RandomSeed randomSeed)
        {
            var index = randomSeed.Random.Next(0, graphs.Count);
            return graphs[index].Copy();
        }

        /// <summary>
        /// Returns a copy of a random layout graph from the list.
        /// </summary>
        /// <param name="functions">A list of functions returning a layout graph.</param>
        /// <param name="randomSeed">The random seed.</param>
        public LayoutGraph DrawSelection(IList<Func<LayoutGraph>> functions, RandomSeed randomSeed)
        {
            var index = randomSeed.Random.Next(0, functions.Count);
            return functions[index].Invoke().Copy();
        }

        /// <summary>
        /// Returns a copy of a random layout graph from the list.
        /// </summary>
        /// <param name="graphs">A list of layout graphs or functions returning layout graphs.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <exception cref="ArgumentException">Raised if the type of `graphs` is not handled.</exception>
        public LayoutGraph DrawSelection(object graphs, RandomSeed randomSeed)
        {
            switch (graphs)
            {
                case IList<LayoutGraph> list:
                    return DrawSelection(list, randomSeed);
                case IList<Func<LayoutGraph>> functions:
                    return DrawSelection(functions, randomSeed);
                default:
                    throw new ArgumentException($"Unhandled type for `graphs`: {graphs.GetType()}.");
            }
        }
    }
}
