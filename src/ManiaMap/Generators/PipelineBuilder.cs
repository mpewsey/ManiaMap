using MPewsey.Common.Pipelines;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// Contains methods for constructing pipelines.
    /// </summary>
    public static class PipelineBuilder
    {
        /// <summary>
        /// Creates the default layout generation pipeline.
        /// </summary>
        public static Pipeline CreateDefaultPipeline()
        {
            return new Pipeline(new LayoutGraphRandomizer(), new LayoutGenerator(), new CollectableGenerator());
        }
    }
}
