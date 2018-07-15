using System;
using Microsoft.Xna.Framework;
using RockSolidEngine.Graphics;
using RockSolidEngine.Maps;

namespace RockSolidEngine.Screens.TestScreen
{
    public class RockTestScreen : IScreen
    {
        RockTileMap tileMapTest = new RockTileMap();

        public RockTestScreen()
        {
            tileMapTest.LoadMapFromFile("level.bin");
        }

        public override string ScreenName { get; internal set; } = "Rock Test Screen";

        public override void Draw(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.Bisque);
            graphics.GetSpriteBatch().Begin();
            graphics.GetSpriteBatch().End();
        }

        public override void Update(GameTime gameTime)
        {
            //nope
        }
    }
}
