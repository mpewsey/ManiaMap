using System.Collections.Generic;
using System.Drawing;

namespace MPewsey.ManiaMap
{
    public interface IRoomSource
    {
        Uid RoomId { get; }
        string Name { get; set; }
        Color Color { get; set; }
        int Z { get; set; }
        List<string> TemplateGroups { get; }
    }
}
