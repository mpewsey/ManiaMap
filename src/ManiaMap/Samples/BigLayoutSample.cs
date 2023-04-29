using MPewsey.Common.Mathematics;
using MPewsey.Common.Pipelines;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Generators;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MPewsey.ManiaMap.Samples
{
    /// <summary>
    /// Contains methods for the big layout sample.
    /// </summary>
    public static class BigLayoutSample
    {
        /// <summary>
        /// Returns the template groups for the big layout sample.
        /// </summary>
        public static TemplateGroups BigLayoutTemplateGroups()
        {
            var templateGroups = new TemplateGroups();

            var square2x2 = TemplateLibrary.Squares.Square2x2Template();
            var square3x3 = TemplateLibrary.Squares.Square3x3Template();
            var rect2x3 = TemplateLibrary.Rectangles.Rectangle2x3Template();
            var rect2x4 = TemplateLibrary.Rectangles.Rectangle2x4Template();

            var rooms = new List<List<RoomTemplate>>
            {
                square2x2.UniqueVariations(),
                square3x3.UniqueVariations(),
                rect2x3.UniqueVariations(),
                rect2x4.UniqueVariations(),
            };

            rooms.ForEach(x => templateGroups.Add("Rooms", x));

            var square1x1 = TemplateLibrary.Squares.Square1x1Template();
            var rect1x2 = TemplateLibrary.Rectangles.Rectangle1x2Template();
            var rect1x3 = TemplateLibrary.Rectangles.Rectangle1x3Template();
            var rect1x4 = TemplateLibrary.Rectangles.Rectangle1x4Template();
            var angle3x4 = TemplateLibrary.Angles.Angle3x4();

            var paths = new List<List<RoomTemplate>>
            {
                square1x1.UniqueVariations(),
                rect1x2.UniqueVariations(),
                rect1x3.UniqueVariations(),
                rect1x4.UniqueVariations(),
                angle3x4.UniqueVariations(),
            };

            paths.ForEach(x => templateGroups.Add("Paths", x));

            // Set Default template group to all cells
            var seed = new RandomSeed(12345);

            foreach (var template in templateGroups.GetAllTemplates())
            {
                for (int i = 0; i < template.Cells.Rows; i++)
                {
                    for (int j = 0; j < template.Cells.Columns; j++)
                    {
                        template.AddCollectableSpot(seed.Next(), new Vector2DInt(i, j), "Default");
                    }
                }
            }

            var savePoint = TemplateLibrary.Squares.Square1x1SavePointTemplate();
            templateGroups.Add("SavePoint", savePoint);

            return templateGroups;
        }

        /// <summary>
        /// Generates the big layout using default parameters and returns the results.
        /// </summary>
        /// <param name="seed">The random seed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static PipelineResults Generate(int seed, CancellationToken cancellationToken = default)
        {
            var random = new RandomSeed(seed);
            var graph = GraphLibrary.BigGraph();
            var templateGroups = BigLayoutTemplateGroups();
            var collectableGroups = new CollectableGroups();
            collectableGroups.Add("Default", Enumerable.Range(0, 10));

            var inputs = new Dictionary<string, object>
            {
                { "LayoutId", 1 },
                { "LayoutGraph", graph },
                { "TemplateGroups", templateGroups },
                { "CollectableGroups", collectableGroups },
                { "RandomSeed", random },
            };

            var pipeline = PipelineBuilder.CreateDefaultPipeline();
            return pipeline.Run(inputs, cancellationToken);
        }

        /// <summary>
        /// Generates the big layout asynchronously using default parameters and returns the results.
        /// </summary>
        /// <param name="seed">The random seed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static Task<PipelineResults> GenerateAsync(int seed, CancellationToken cancellationToken = default)
        {
            var random = new RandomSeed(seed);
            var graph = GraphLibrary.BigGraph();
            var templateGroups = BigLayoutTemplateGroups();
            var collectableGroups = new CollectableGroups();
            collectableGroups.Add("Default", Enumerable.Range(0, 10));

            var inputs = new Dictionary<string, object>
            {
                { "LayoutId", 1 },
                { "LayoutGraph", graph },
                { "TemplateGroups", templateGroups },
                { "CollectableGroups", collectableGroups },
                { "RandomSeed", random },
            };

            var pipeline = PipelineBuilder.CreateDefaultPipeline();
            return pipeline.RunAsync(inputs, cancellationToken);
        }
    }
}
