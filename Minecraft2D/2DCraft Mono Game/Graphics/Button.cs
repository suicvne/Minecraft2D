using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
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
            Size = new Rectangle(Position.X, Position.Y, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight);
            ButtonText = this.ToString();
        }
        public Button(Vector2i pos, Rectangle size, string text)
        {
            Position = pos;
            Size = new Rectangle(pos.X, pos.Y, size.Width, size.Height);
            ButtonText = text;
        }

        public void Update(GameTime gameTime)
        {
            Rectangle mouseBounds = new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X + 8, 
                MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32);

            if (mouseBounds.Intersects(Size))
                Selected = true;
            else
                Selected = false;

            if (Selected && MainGame.GlobalInputHelper.IsNewPress(MouseButtons.LeftButton))
            {
                MainGame.CustomContentManager.GetSoundEffect("click").Play();
                if (Clicked != null)
                    Clicked();
            }
        }

        public void Draw(GameTime gameTime)
        {
            int textX = (int)(Size.Center.X - MainGame.CustomContentManager.GetFont("main-font").GetStringRectangle(ButtonText, Position.ToVector2()).Width / 2);
            
            if (Selected)
            {
                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                    new Rectangle(Position.X, Position.Y, Size.Width, Size.Height),
                    new Rectangle(WidgetsMap.HighlightedButton.X, WidgetsMap.HighlightedButton.Y, WidgetsMap.HighlightedButton.RegionWidth, WidgetsMap.HighlightedButton.RegionHeight), Color.White);
                MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), this.ButtonText, 
                    new Vector2(textX, Size.Y + 13), 
                    Color.White);
            }
            else
            {
                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                    new Rectangle(Position.X, Position.Y, Size.Width, Size.Height),
                    new Rectangle(WidgetsMap.EnabledButton.X, WidgetsMap.EnabledButton.Y, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight), Color.White);
                MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), this.ButtonText,
                    new Vector2(textX, Size.Y + 13),
                    Color.White);
            }
        }
    }
}
