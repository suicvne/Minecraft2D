using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2DRebirth.Screens
{
    public class BlankScreen : IScreen
    {
        public override string ScreenName
        {
            get
            {
                return "Blank";
            }
            internal set { }
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            graphics.GetSpriteBatch().Begin();
            {
                graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.BlanchedAlmond);
                var mousePosition = OffsetMouse(Minecraft2D.inputHelper.MousePosition);
                graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("cursor"), mousePosition, Color.White);
            }
            graphics.GetSpriteBatch().End();
        }

        /// <summary>
        /// Used to correct the mouse position based on window position.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private Vector2 OffsetMouse(Vector2 input)
        {
			Vector2 output = input;
			Vector2 windowPosition = Minecraft2D.inputHelper.game.Window.Position.ToVector2();

			Console.WriteLine ($"{windowPosition.ToString()}");

            var graphicsDevice = Minecraft2D.graphics.GetGraphicsDeviceManager().GraphicsDevice;
			#if !LINUX //TODO: Test Mac
            output.X -= windowPosition.X;
            output.Y -= windowPosition.Y;
			#endif
            return output;
        }

        public override void Update(GameTime gameTime)
        {}
    }
}
