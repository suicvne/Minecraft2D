using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public delegate void TextBoxClicked();
    public class TextBox
    {
        public string Content { get; set; }
        public bool HasFocus { get; set; }
        public bool Enabled { get; set; }
        
        public Rectangle Position { get; set; }
        private Rectangle BackgroundRectangle;

        public event TextBoxClicked MouseClicked;

        public TextBox()
        {
            Content = "";
            HasFocus = false;
            Enabled = true;

            Position = new Rectangle(0, 0, 32 * 5, 32);
            BackgroundRectangle = new Rectangle(Position.X - 2, Position.Y - 2, Position.Width + 16, Position.Height + 16);

            MainGame.TextInputReceived += (e) =>
            {
                if (HasFocus && Enabled)
                {
                    if (e.Character == '\b')
                    {
                        if (Content.Length > 0)
                            Content = Content.Substring(0, Content.Length - 1);
                    }
                    else
                    {
                        Content += e.Character.ToString();
                    }
                }
            };
        }

        public TextBox(Rectangle pos, bool enabl)
        {
            Content = "";
            HasFocus = false;
            Enabled = enabl;

            Position = pos;
            BackgroundRectangle = new Rectangle(pos.X - 2, pos.Y - 2, pos.Width + 4, pos.Height + 4);

            MainGame.TextInputReceived += (e) =>
            {
                if (HasFocus && Enabled)
                {
                    if (e.Character == '\b')
                    {
                        if (Content.Length > 0)
                            Content = Content.Substring(0, Content.Length - 1);
                    }
                    else
                    {
                        Content += e.Character.ToString();
                    }
                }
            };
        }

        public void Update(GameTime gameTime)
        {
            if(HasFocus && Enabled)
            {
                //if(MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys() != null)
                //{
                //    Content += MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys().ToString();
                //}
            }
            if (Enabled)
            {
                if (MainGame.GlobalInputHelper.CurrentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    Rectangle mouseBounds = new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X + 8,
                        MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32);

                    if (mouseBounds.Intersects(Position))
                    {
                        if (MouseClicked != null)
                            MouseClicked();
                        HasFocus = true;
                    }
                    else
                        HasFocus = false;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if(Enabled)
            {
                DrawRectangle(BackgroundRectangle, Color.White, 1f);
                DrawRectangle(Position, Color.Black, 1f);

                //for(int i = 0; i < Content.Length; i++)
                //{
                //    if (i * 16 > Position.Width)
                //        break;

                //}

                if(Content.Length > (Position.Width / 8))
                    MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), Content.Substring(0, Position.Width / 8),
                        new Vector2(Position.X + 1, Position.Y + 12), Color.White);
                else
                    MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), Content,
                            new Vector2(Position.X + 1, Position.Y + 12), Color.White);

                if (HasFocus)
                {
                    if (Content.Length < (Position.Width / 8))
                        MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), 
                        "_", 
                        new Vector2(Position.X + (Content.Length * 8) + 8, Position.Y + 14), Color.White);
                }
            }
            else
            {
                DrawRectangle(BackgroundRectangle, Color.Gray, 1f);
                DrawRectangle(Position, Color.DarkGray, 1f);
            }
        }

        private void DrawRectangle(Rectangle coords, Color color, float opMod)
        {
            var rect = new Texture2D(MainGame.GlobalGraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            MainGame.GlobalSpriteBatch.Draw(rect, coords, color * opMod);
        }
    }
}
