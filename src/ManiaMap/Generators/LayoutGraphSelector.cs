﻿using MPewsey.Common.Pipelines;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Graphs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// A class for drawing a random LayoutGraph from multiple.
    /// </summary>
    public class LayoutGraphSelector : IPipelineStep
    {
        /// <summary>
        /// A delegate returning a LayoutGraph.
        /// </summary>
        public delegate LayoutGraph LayoutGraphDelegate();

        /// <summary>
        /// Draws a random layout graph and adds a copy to the results output dictionary.
        /// 
        /// The following arguments are required:
        /// * %LayoutGraphs - A list of layout graphs or functions that return layout graphs.
        /// * %RandomSeed - The random seed.
        /// 
        /// The following entries are added to the results output dictionary:
        /// * %LayoutGraph - The drawn layout graph.
        /// </summary>
        /// <param name="results">The pipeline results.</param>
        /// <param name="logger">The logging action. Ignored if null.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public bool ApplyStep(PipelineResults results, Action<string> logger, CancellationToken cancellationToken)
        {
            var randomSeed = results.GetArgument<RandomSeed>("RandomSeed");
            var layouts = results.GetArgument<object>("LayoutGraphs");
            results.SetOutput("LayoutGraph", DrawSelection(layouts, randomSeed, logger));
            return true;
        }

        /// <summary>
        /// Returns a copy of a random layout graph from the list.
        /// </summary>
        /// <param name="graphs">A list of layout graphs.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="logger">The logging action. Ignored if null.</param>
        public LayoutGraph DrawSelection(IList<LayoutGraph> graphs, RandomSeed randomSeed, Action<string> logger = null)
        {
            logger?.Invoke("[Layout Graph Selector] Running layout graph list selector...");
            var index = randomSeed.Next(0, graphs.Count);
            var result = graphs[index].Copy();
            logger?.Invoke("[Layout Graph Selector] Layout graph selector complete.");
            return result;
        }

        /// <summary>
        /// Returns a copy of a random layout graph from the list.
        /// </summary>
        /// <param name="functions">A list of functions returning a layout graph.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="logger">The logging action. Ignored if null.</param>
        public LayoutGraph DrawSelection(IList<LayoutGraphDelegate> functions, RandomSeed randomSeed, Action<string> logger = null)
        {
            logger?.Invoke("[Layout Graph Selector] Applying layout graph delegate list selector...");
            var index = randomSeed.Next(0, functions.Count);
            var result = functions[index].Invoke().Copy();
            logger?.Invoke("[Layout Graph Selector] Layout graph selector complete.");
            return result;
        }

        /// <summary>
        /// Returns a copy of a random layout graph from the list.
        /// </summary>
        /// <param name="graphs">A list of layout graphs or functions returning layout graphs.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="logger">The logging action. Ignored if null.</param>
        /// <exception cref="ArgumentException">Raised if the type of `graphs` is not handled.</exception>
        public LayoutGraph DrawSelection(object graphs, RandomSeed randomSeed, Action<string> logger = null)
        {
            logger?.Invoke("[Layout Graph Selector] Running layout graph selector...");

            switch (graphs)
            {
                case IList<LayoutGraph> list:
                    return DrawSelection(list, randomSeed, logger);
                case IList<LayoutGraphDelegate> functions:
                    return DrawSelection(functions, randomSeed, logger);
                default:
                    throw new ArgumentException($"Unhandled type for `graphs`: {graphs.GetType()}.");
            }
        }
    }
}
