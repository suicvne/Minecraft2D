using Microsoft.Xna.Framework;
using Minecraft2D.Screens;
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


        int tickCount = 0;
        public new void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            Move(new Vector2(MainGameScreen.world.GetClientPlayer().Position.X - 32 * 4, MainGameScreen.world.GetClientPlayer().Position.Y - 32 * 4));
        }

        public new void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
