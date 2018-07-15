using RockSolidEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Controls
{
    /// <summary>
    /// Defines locations for widgets and whatnot.
    /// </summary>
    public static class WidgetsMap
    {
        public static SkinRegion DisabledButton = new SkinRegion(0, 46, 200, 20);
        public static SkinRegion EnabledButton = new SkinRegion(0, 46 + 20, 200, 20);
        public static SkinRegion HighlightedButton = new SkinRegion(0, 46 + 20 + 20, 200, 20);

        public static SkinRegion SingleInventory = new SkinRegion(30, 23, 24, 23);

        public static SkinRegion Minec = new SkinRegion(0, 0, 155, 44);
        public static SkinRegion raft = new SkinRegion(0, 44, 119, 44);
    }
}
