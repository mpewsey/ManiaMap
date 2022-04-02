using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    public interface IGenerationStep
    {
        /// <summary>
        /// Performs the generation operations for this step.
        /// Artifacts should be written to the input dictionary.
        /// </summary>
        /// <param name="map">A dictionary of arguments.</param>
        void Generate(Dictionary<string, object> map);
    }
}
