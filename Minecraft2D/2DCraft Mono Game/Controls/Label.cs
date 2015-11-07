using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Minecraft2D.Screens.TitleScreen;

namespace Minecraft2D.Controls
{
    public class Label : Control
    {
        public string Content { get; set; }
        public Rectangle Position { get; set; }
        public Color LabelColor { get; set; }

        public Label(string name)
        {
            LabelColor = Color.White;

            Name = name;
            Content = "Cool";
            Position = new Rectangle(0, 0, Content.Length * 16, 16);

            HasFocus = false;
            Enabled = true;
        }

        public Label(string name, string text)
        {
            LabelColor = Color.White;
            Name = name;
            Content = text;
            Position = new Rectangle(0, 0, Content.Length * 16, 16);

            HasFocus = false;
            Enabled = true;
        }

        public Label(string name, string text, Vector2 pos)
        {
            LabelColor = Color.White;
            Name = name;
            Content = text;
            Position = new Rectangle((int)pos.X, (int)pos.Y, Content.Length * 16, 16);

            HasFocus = false;
            Enabled = true;
        }

        public Label(string name, string text, Vector2 pos, Color tnt)
        {
            LabelColor = Color.White;
            Name = name;
            Content = text;
            Position = new Rectangle((int)pos.X, (int)pos.Y, Content.Length * 16, 16);

            HasFocus = false;
            Enabled = true;
            LabelColor = tnt;
        }

        public override void Update(GameTime gameTime)
        {}

        public override void Draw(GameTime gameTime)
        {
            DrawText(Content, new Vector2(Position.X, Position.Y), LabelColor);
        }
    }
}
