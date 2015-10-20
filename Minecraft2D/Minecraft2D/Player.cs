using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D
{
    public class Player
    {
        public SpriteBatch spriteBatch { get; set; }

        public string Username { get; set; }
        public Vector2 ScreenPosition { get; set; }


        public Player()
        {
            ScreenPosition = new Vector2(32, 32);
        }

        public void Update(GameTime gameTime)
        {
            if(Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
            {
                ScreenPosition = new Vector2(ScreenPosition.X - 2, ScreenPosition.Y);
            }
        }

        public void Draw(GameTime gameTime) { }
    }
}
