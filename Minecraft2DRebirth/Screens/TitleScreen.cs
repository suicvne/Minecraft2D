using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Graphics;
using Minecraft2DRebirth.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2DRebirth.Screens
{
    public class TitleScreen : IScreen
    {
        public override string ScreenName
        {
            get
            {
                return "TitleScreen";
            }
            internal set
            {}
        }

        public TitleScreen()
        {
            Button testButton = new Button(new Rectangle(0, 0, 320, 40), "I'm constantly centered!");
            testButton.DrawCentered = true;

            Button testButton2 = new Button(new Rectangle(0, 0, 320, 40), "One above");
            testButton2.DrawCentered = true;
            testButton2.Offset = new Vector2(0, -32 - 16);

            Button testButton3 = new Button(new Rectangle(0, 0, 320, 40), "Exit");
            testButton3.DrawCentered = true;
            testButton3.Offset = new Vector2(0, 32 + 16);

            testButton3.Clicked += (sender, e) =>
            {
                Minecraft2D.InputHelper.CallExit();
            };

            SplashLabel splashLabel = new SplashLabel();
            splashLabel.DrawCentered = true;
            splashLabel.Offset = new Vector2(128, -128);

            AddControl(testButton);
            AddControl(testButton2);
            AddControl(testButton3);
            AddControl(splashLabel);
        }

        private void DrawBackground(Graphics.Graphics graphics)
        {
            var texture = graphics.GetTexture2DByName("terrain");
            int tx, ty;
            tx = (int)Math.Ceiling((double)graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width / 32);
            ty = (int)Math.Ceiling((double)graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height / 32);

            Vector2 textureIndex = new Vector2(13 * Constants.SpriteSize, 7 * Constants.SpriteSize);

            for (int x = 0; x < tx; x++)
                for (int y = 0; y < ty; y++)
                    graphics.GetSpriteBatch().Draw(texture, new Vector2(x * Constants.TileSize, y * Constants.TileSize).ToRectangle(), 
                        textureIndex.ToRectangle(Constants.SpriteSize), Color.White);
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.Black);
            DrawBackground(graphics);
            DrawControls(graphics);

            DrawCursor(graphics);
            graphics.GetSpriteBatch().End();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateControls(gameTime);
        }
    }
}
