using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Entity
{
    public interface IEntity
    {
        Vector2 Position { get; set; }
        string EntityName { get; set; }
        Rectangle Hitbox { get; set; }

        void Draw(Graphics.Graphics graphics);
        void Update(GameTime gameTime);
    }
}
