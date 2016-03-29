using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Entity
{
    public abstract class IEntity
    {
        public Vector2 Position { get; set; }
        public string EntityName { get; set; }

        public abstract void Draw(Graphics.Graphics graphics);
        public abstract void Update(GameTime gameTime);
    }
}
