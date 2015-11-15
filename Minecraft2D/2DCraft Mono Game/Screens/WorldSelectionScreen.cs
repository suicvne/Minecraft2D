using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Minecraft2D.Controls;

namespace Minecraft2D.Screens
{
    public class WorldSelectionScreen : Screen
    {
        public WorldSelectionScreen()
        {
            //for now
            Button worldOne, worldTwo, worldThree, worldFour, worldFive;

            if(Directory.Exists("Saves"))
            {

            }
            else
            {
                Directory.CreateDirectory("Saves");
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int tx, ty;
            tx = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Width / 32);
            ty = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Height / 32);

            MainGame.GlobalGraphicsDevice.Clear(Color.CornflowerBlue);

            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            for (int x = 0; x < tx; x++)
                for (int y = 0; y < ty; y++)
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(16 * 2, 0, 16, 16), new Color(100, 100, 100));

            foreach (var control in ControlsList)
                control.Draw(gameTime);

            //TitleScreen.DrawText("Work in progress!", new Vector2(MainGame.GlobalGraphicsDevice.Viewport.Width - (("Work in progress!".Length * 14) * 2), 90), Color.Gray);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32), Color.White);

            MainGame.GlobalSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
