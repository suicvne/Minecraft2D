using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public delegate void MouseClicked();
    public class Button
    {
        public Vector2i Position { get; set; }
        public Rectangle Size { get; set; }
        public bool Selected { get; set; }
        public bool Enabled { get; set; }
        public string ButtonText { get; set; }
        public event MouseClicked Clicked;

        public Button()
        {
            Enabled = true;
            Selected = false;
            Position = new Vector2i(0, 0);
            Size = new Rectangle(0, 0, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight);
            ButtonText = this.ToString();
        }
        public Button(Vector2i pos, Rectangle size, string text)
        {
            Position = pos;
            Size = size;
            ButtonText = text;
        }

        public void Update(GameTime gameTime)
        {
            Rectangle mouseBounds = new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X + 8, 
                MainGame.GlobalInputHelper.CurrentMouseState.Y + 8, 32, 32);

            if (mouseBounds.Intersects(new Rectangle(Position.X, Position.Y, Size.Width, Size.Height)))
                Selected = true;
            else
                Selected = false;

            if (Selected && MainGame.GlobalInputHelper.CurrentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                if (Clicked != null)
                    Clicked();
        }

        public void Draw(GameTime gameTime)
        {
            if (Selected)
            {
                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                    new Rectangle(Position.X, Position.Y, Size.Width, Size.Height),
                    new Rectangle(WidgetsMap.HighlightedButton.X, WidgetsMap.HighlightedButton.Y, WidgetsMap.HighlightedButton.RegionWidth, WidgetsMap.HighlightedButton.RegionHeight), Color.White);
                MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("minecraft"), this.ButtonText, 
                    new Vector2(120, 120), 
                    Color.White);
            }
            else
            {
                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                    new Rectangle(Position.X, Position.Y, Size.Width, Size.Height),
                    new Rectangle(WidgetsMap.EnabledButton.X, WidgetsMap.EnabledButton.Y, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight), Color.White);
            }
        }
    }
}
