using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2DRebirth.Native;
using Minecraft2DRebirth.Screens.TestScreen;
using Minecraft2DRebirth.Graphics;
using Microsoft.Xna.Framework.Input;
using Minecraft2DRebirth.Entity;
using Minecraft2DRebirth.Scenes;

namespace Minecraft2DRebirth.Screens
{
    public class BlankScreen : IScreen
    {
        private BasicLightableSceneWithMap Scene;
        private AnnoyingLightEntityTest FakeSunEntity;

        public override string ScreenName
        {
            get
            {
                return "Testing Screen";
            }
            internal set { }
        }

        public PlayerTest TestEntity;

        public BlankScreen(Graphics.Graphics graphics)
        {
            TestEntity = new PlayerTest();
            TestEntity.Animating = false;

            Scene = new BasicLightableSceneWithMap(graphics);
            Scene.AmbientLight = Color.White;//new Color(20, 20, 20);
            TestEntity.LightSize = 0f;

            int x = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width / 2;
            int y = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height / 2;
            TestEntity.Position = new Vector2(x + (32 * 32), y + (25 * 32) - 64);

            Scene.AddEntity(TestEntity); //player entity
            //FakeSunEntity = new AnnoyingLightEntityTest();
            //Scene.AddEntity(FakeSunEntity); //lol
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            Scene.Draw(graphics);
        }

        private uint GameTime = 0;
        private int Angle = 270;
        private const uint MaxGameTime = 24000;
        private int IncrementMultiplier = 1;
        public override void Update(GameTime gameTime)
        {
            Scene.Update(gameTime);
            /*
            GameTime += (uint)(gameTime.ElapsedGameTime.Milliseconds * IncrementMultiplier);
            Angle += (int)(gameTime.ElapsedGameTime.Milliseconds);
            FakeSunEntity.Angle = (Angle / 78);

            if (GameTime > MaxGameTime)
                IncrementMultiplier = -1;
            else if (GameTime == 0)
                IncrementMultiplier = 1;

            int rgb = (int)Math.Min(GameTime / (255 / 2), 255);
            var colour = new Color(rgb, rgb, rgb);
            Scene.AmbientLight = new Color(rgb, rgb, rgb);
            */
        }
    }
}
