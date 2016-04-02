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
        private BasicLightableScene Scene;
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

            Scene = new BasicLightableScene(graphics);
            Scene.AmbientLight = new Color(20, 20, 20);
            Scene.AddEntity(TestEntity); //player entity
            FakeSunEntity = new AnnoyingLightEntityTest();
            Scene.AddEntity(FakeSunEntity); //lol

            //var screen = graphics.ScreenRectangle();
            //screen.Width *= Constants.TileSize;
            //screen.Height *= Constants.TileSize;
            //screen.X -= (screen.Width / 2);
            //screen.Y -= (screen.Height / 2);
            //Scene.AddStaticLight(new LightSource
            //{
            //    Color = Color.White,
            //    Size = screen
            //});
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
        }
    }
}
