using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Entity
{
    public interface IAnimatedEntity
    {
        Vector2 Position { get; set; }
        Vector2 SpriteSize { get; set; }
        string EntityName { get; set; }
        Rectangle Hitbox { get; set; }

        void Draw(Graphics.Graphics graphics);
        void Update(GameTime gameTime);
    }
}
