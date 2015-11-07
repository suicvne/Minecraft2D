using Microsoft.Xna.Framework;
using Minecraft2D.Graphics;
using Minecraft2D.Screens;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Controls
{
    public delegate void MouseClicked();
    public class Button : Control
    {
        public Vector2i Position { get; set; }
        public Rectangle Size { get; set; }
        public bool Selected { get; set; }
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
            Enabled = true;
            Position = pos;
            Size = new Rectangle(pos.X, pos.Y, size.Width, size.Height);
            ButtonText = text;
        }
        public Button(Vector2i pos, Rectangle size, string text, bool enabled)
        {
            Enabled = enabled;
            Position = pos;
            Size = new Rectangle(pos.X, pos.Y, size.Width, size.Height);
            ButtonText = text;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (Enabled == true)
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
        }

        public override void Draw(GameTime gameTime)
        {
            int textX = (int)(Size.Center.X - MainGame.CustomContentManager.GetFont("main-font").GetStringRectangle(ButtonText, Position.ToVector2()).Width / 2);

            if (Enabled == true)
            {
                if (Selected)
                {
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                        new Rectangle(Position.X, Position.Y, Size.Width, Size.Height),
                        new Rectangle(WidgetsMap.HighlightedButton.X, WidgetsMap.HighlightedButton.Y, WidgetsMap.HighlightedButton.RegionWidth, WidgetsMap.HighlightedButton.RegionHeight), Color.White);

                    TitleScreen.DrawText(ButtonText, new Vector2(textX, Size.Y + 13), Color.White);
                    //MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), this.ButtonText, 
                    //    new Vector2(textX, Size.Y + 13), 
                    //    Color.White);
                }
                else
                {
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                        new Rectangle(Position.X, Position.Y, Size.Width, Size.Height),
                        new Rectangle(WidgetsMap.EnabledButton.X, WidgetsMap.EnabledButton.Y, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight), Color.White);

                    TitleScreen.DrawText(ButtonText, new Vector2(textX, Size.Y + 13), Color.White);
                    //MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), this.ButtonText,
                    //    new Vector2(textX, Size.Y + 13),
                    //    Color.White);
                }
            }
            else
            {
                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                        new Rectangle(Position.X, Position.Y, Size.Width, Size.Height),
                        new Rectangle(WidgetsMap.DisabledButton.X, WidgetsMap.DisabledButton.Y, WidgetsMap.DisabledButton.RegionWidth, WidgetsMap.DisabledButton.RegionHeight), Color.White);

                TitleScreen.DrawText(ButtonText, new Vector2(textX, Size.Y + 13), Color.Gray);
            }
        }
    }
}
