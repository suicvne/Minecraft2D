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
        public Rectangle Position { get; set; }
        public bool Selected { get; set; }
        public string ButtonText { get; set; }
        public event MouseClicked Clicked;

        public Button()
        {
            Enabled = true;
            Selected = false;
            Position = new Rectangle(0, 0, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight);
            ButtonText = this.ToString();
        }
        public Button(Rectangle pos, string text)
        {
            Enabled = true;
            Position = pos;
            ButtonText = text;
        }
        public Button(Rectangle pos, string text, bool enabled)
        {
            Enabled = enabled;
            Position = pos;
            ButtonText = text;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (Enabled == true)
            {
                Rectangle mouseBounds = new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X + 8,
                    MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32);

                if (mouseBounds.Intersects(Position))
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
            int textX = (int)(Position.Center.X - MainGame.CustomContentManager.SplashFont
                .MeasureString(ButtonText).X / 2);
                //.GetStringRectangle(ButtonText, new Vector2(Position.X, Position.Y)).Width / 2);

            if (Enabled == true)
            {
                if (Selected)
                {
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                        new Rectangle(Position.X, Position.Y, Position.Width, Position.Height),
                        new Rectangle(WidgetsMap.HighlightedButton.X, WidgetsMap.HighlightedButton.Y, WidgetsMap.HighlightedButton.RegionWidth, WidgetsMap.HighlightedButton.RegionHeight), Color.White);

                    GraphicsHelper.DrawText(ButtonText, new Vector2(textX, Position.Y + 8), Color.White);
                    //MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), this.ButtonText, 
                    //    new Vector2(textX, Size.Y + 13), 
                    //    Color.White);
                }
                else
                {
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                        new Rectangle(Position.X, Position.Y, Position.Width, Position.Height),
                        new Rectangle(WidgetsMap.EnabledButton.X, WidgetsMap.EnabledButton.Y, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight), Color.White);

                    GraphicsHelper.DrawText(ButtonText, new Vector2(textX, Position.Y + 8), Color.White);
                    //MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), this.ButtonText,
                    //    new Vector2(textX, Size.Y + 13),
                    //    Color.White);
                }
            }
            else
            {
                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("widgets"),
                        new Rectangle(Position.X, Position.Y, Position.Width, Position.Height),
                        new Rectangle(WidgetsMap.DisabledButton.X, WidgetsMap.DisabledButton.Y, WidgetsMap.DisabledButton.RegionWidth, WidgetsMap.DisabledButton.RegionHeight), Color.White);

                GraphicsHelper.DrawText(ButtonText, new Vector2(textX, Position.Y + 13), Color.Gray);
            }
        }
    }
}
