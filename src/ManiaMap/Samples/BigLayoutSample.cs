using System.Collections.Generic;

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
            foreach (var templates in templateGroups.GetItemGroups())
            {
                foreach (var template in templates)
                {
                    foreach (var cell in template.Cells.Array)
                    {
                        cell?.AddCollectableGroup(1, "Default");
                    }
                }
            }

            return templateGroups;
        }
    }
}
