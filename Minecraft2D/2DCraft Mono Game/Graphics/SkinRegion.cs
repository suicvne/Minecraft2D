using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    [Serializable]
    public class SkinRegion
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int RegionWidth { get; set; }
        public int RegionHeight { get; set; }

        public SkinRegion()
        {
            X = 0;
            Y = 0;
            RegionHeight = 0;
            RegionWidth = 0;
        }

        public SkinRegion(int x, int y)
        {
            X = x;
            Y = y;
        }

        public SkinRegion(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            RegionWidth = width;
            RegionHeight = height;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(X, Y, RegionWidth, RegionHeight);
        }
    }
}
