using Minecraft2D.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Controls
{
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
