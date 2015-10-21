using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public class Vector2i
    {
        public static Vector2i Zero = new Vector2i { X = 0, Y = 0 };

        public int X { get; set; }
        public int Y { get; set; }

        public Vector2i() { }
        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2i(Vector2 floatVector)
        {
            X = (int)floatVector.X;
            Y = (int)floatVector.Y;
        }

        public static Vector2i operator +(Vector2i first, Vector2i second)
        {
            return new Vector2i { X = first.X + second.X, Y = first.Y + second.Y };
        }

        public static Vector2i operator -(Vector2i first, Vector2i second)
        {
            return new Vector2i { X = first.X - second.X, Y = first.Y - second.Y };
        }
    }
}
