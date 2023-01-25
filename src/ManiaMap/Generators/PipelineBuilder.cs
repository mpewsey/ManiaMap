using MPewsey.Common.Pipelines;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// Contains methods for constructing pipelines.
    /// </summary>
    public static class PipelineBuilder
    {
        /// <summary>
        /// Returns a new pipeline with common default generators in the following order:
        /// 
        /// 1. LayoutGraphRandomizer
        /// 2. LayoutGenerator
        /// 3. CollectableGenerator
        /// </summary>
        public static Pipeline CreateDefaultPipeline()
        {
            return new Pipeline(new LayoutGraphRandomizer(), new LayoutGenerator(), new CollectableGenerator());
        }
    }
}
