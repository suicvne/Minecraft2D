using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Graphics;
using Minecraft2DRebirth.Input;

namespace Minecraft2DRebirth.Controls
{
    public class Button : IControl
    {
        public bool Selected { get; set; }
        public string Text { get; set; }
        public event EventHandler<EventArgs> Clicked;

        public Button()
        {
            Enabled = true;
            Selected = false;
            PositionSize = new Rectangle(0, 0, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight);
        }
        public Button(Rectangle pos, string text)
        {
            Enabled = true;
            PositionSize = pos;
            Text = text;
        }
        public Button(Rectangle pos, string text, bool enabled)
        {
            Enabled = enabled;
            PositionSize = pos;
            Text = text;
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            int textX = (int)(PositionSize.Center.X - graphics.GetSpriteFontByName("minecraft").MeasureString(Text).X / 2);
            
            if (Enabled == true)
            {
                if (Selected)
                {
                    /*graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("widgets"),
                        new Rectangle(PositionSize.X, PositionSize.Y, (int)(PositionSize.Width * Math.Min(ScaleFactor, MaxScaleFactor)),
                            (int)(PositionSize.Height * Math.Min(ScaleFactor, MaxScaleFactor))),
                        new Rectangle(WidgetsMap.HighlightedButton.X, WidgetsMap.HighlightedButton.Y, WidgetsMap.HighlightedButton.RegionWidth, WidgetsMap.HighlightedButton.RegionHeight), Color.White);
                        */

                    graphics.DrawText(Text, new Rectangle(textX, PositionSize.Y + 8, PositionSize.Width, PositionSize.Height), Color.Yellow,
                        (float)Math.Min(ScaleFactor, MaxScaleFactor));
                }
                else
                {
                    /*graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("widgets"),
                        new Rectangle(PositionSize.X, PositionSize.Y, PositionSize.Width, PositionSize.Height),
                        new Rectangle(WidgetsMap.EnabledButton.X, WidgetsMap.EnabledButton.Y, WidgetsMap.EnabledButton.RegionWidth, WidgetsMap.EnabledButton.RegionHeight), Color.White);
                        */

                    graphics.DrawText(Text, new Rectangle(textX, PositionSize.Y + 8, PositionSize.Width, PositionSize.Height), Color.White, 
                        (float)Math.Min(ScaleFactor, MaxScaleFactor));
                }
            }
            else
            {
                /*graphics.GetSpriteBatch().Draw(graphics.GetTexture2DByName("widgets"),
                        new Rectangle(PositionSize.X, PositionSize.Y, PositionSize.Width, PositionSize.Height),
                        new Rectangle(WidgetsMap.DisabledButton.X, WidgetsMap.DisabledButton.Y, WidgetsMap.DisabledButton.RegionWidth, WidgetsMap.DisabledButton.RegionHeight), Color.White);
                        */

                graphics.DrawText(Text, new Vector2((textX * Constants.SpriteScale), (PositionSize.Y + 8) * Constants.SpriteScale), Color.Gray);
            }
        }

        private const double MaxScaleFactor = 2f;
        private double ScaleFactor = 1f;

        public override void Update(GameTime gameTime)
        {
            if (Enabled == true)
            {
                Rectangle mouseBounds = Minecraft2D.InputHelper.MousePosition.OffsetForMouseInput().ToRectangle(Constants.CursorSize);

                if (mouseBounds.Intersects(PositionSize))
                {
                    Selected = true;
                    ScaleFactor += Math.Abs(Math.Sin(3.ToRadians()));
                }
                else
                {
                    if (ScaleFactor > MaxScaleFactor)
                        ScaleFactor = MaxScaleFactor;
                    Selected = false;
                    ScaleFactor -= Math.Abs(Math.Sin(6.ToRadians()));
                    if (ScaleFactor < 1f)
                        ScaleFactor = 1f;
                }

                if (Selected && Minecraft2D.InputHelper.IsNewPress(MouseButtons.LeftButton))
                {
                    //MainGame.CustomContentManager.GetSoundEffect("click").Play();
                    if (Clicked != null)
                        Clicked(this, new EventArgs());
                }
            }
        }
        
    }
}
