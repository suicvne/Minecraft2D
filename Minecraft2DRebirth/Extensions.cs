using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2DRebirth.Native;
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

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
        }

        /// <summary>
        /// Offsets a <see cref="Vector2"/>'s position for use as a mouse cursor.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 OffsetForMouseInput(this Vector2 vector)
        {
            Vector2 output = vector;
            Vector2 windowPosition = Minecraft2D.InputHelper.game.Window.ClientBounds.ToVector2();

            var graphicsDevice = Minecraft2D.graphics.GetGraphicsDeviceManager().GraphicsDevice;
            if (!OperatingSystemDetermination.IsOnUnix())
            {
                output.X -= windowPosition.X;
                output.Y -= windowPosition.Y;
            }
            return output;
        }

        /// <summary>
        /// Returns as a rectangle given the <see cref="Constants.TileSize"/>
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Rectangle ToRectangle(this Vector2 vector)
        {
            return new Rectangle((int)vector.X, (int)vector.Y, Constants.TileSize, Constants.TileSize);
        }

        /// <summary>
        /// Returns as a rectangle given the size.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Rectangle ToRectangle(this Vector2 vector, int size)
        {
            return new Rectangle((int)vector.X, (int)vector.Y, size, size);
        }

        public static Rectangle AsCenterPosition(this Rectangle input)
        {
            Rectangle output = input;
            int screenWidth, screenHeight;
            screenWidth = Minecraft2D.graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width;
            screenHeight = Minecraft2D.graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height;
            output.X = (screenWidth / 2) - ((input.Width) / 2);
            output.Y = (screenHeight / 2) - ((input.Height) / 2);

            return output;
        }
        
        public static Vector2 ToVector2(this Rectangle input)
        {
            return new Vector2(input.X, input.Y);
        }

        public static double ToRadians(this double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
        public static double ToRadians(this int degrees)
        {
            return (degrees * Math.PI) / 180;
        }
        public static double ToDegrees(this double rads)
        {
            return (rads * 180) / Math.PI;
        }

        public static Rectangle ToRectangle(this Viewport vp)
        {
            return new Rectangle(vp.X, vp.Y, vp.Width, vp.Height);
        }
    }
}
