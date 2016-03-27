using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2DRebirth
{
    public static class Extensions
    {
        public static string ToString(this Vector2 vector)
        {
            return $"{vector.X}, {vector.Y}";
        }
    }
}
