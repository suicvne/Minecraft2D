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
        public float X { get; internal set; }
        public float Y { get; internal set; }
        public string EntityName { get; internal set; }

        public abstract void Draw(Graphics.Graphics graphics);
        public abstract void Update(GameTime gameTime);
    }
}
