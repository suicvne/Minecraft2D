using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map.Entities
{
    public class EntityCthulu : Entity
    {
        public EntityCthulu() : base()
        {
            Hitbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 256, 100);
            Name = "Cthulu";
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
        }

        public new void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
