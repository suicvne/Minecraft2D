using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2DRebirth.Native;

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
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            {
                graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.Black);
                var mousePosition = Minecraft2D.inputHelper.MousePosition.OffsetForMouseInput();
                DrawCursor(graphics);
            }
            graphics.GetSpriteBatch().End();
        }
        
        public override void Update(GameTime gameTime)
        {}
    }
}
