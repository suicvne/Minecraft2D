using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Controls
{
    public abstract class IControl
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public Rectangle PositionSize { get; set; }
        public bool HasFocus { get; set; }
        public bool Enabled { get; set; }
        public bool DrawCentered { get; set; } = false;
        public Vector2 Offset { get; set; } = Vector2.Zero;

        internal void UpdateCenterLocation(GameTime gameTime)
        {
            if(DrawCentered)
            {
                PositionSize = PositionSize.AsCenterPosition();
                if(Offset != Vector2.Zero)
                {
                    PositionSize = new Rectangle (
                        PositionSize.X + (int)(Offset.X),
                        PositionSize.Y + (int)(Offset.Y),
                        PositionSize.Width,
                        PositionSize.Height
                    );
                }
            }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(Graphics.Graphics graphics);
    }
}
