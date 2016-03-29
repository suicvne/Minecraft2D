using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Screens.TestScreen
{
    public class AnimatedEntityTest : IAnimatedEntity
    {
        /// <summary>
        /// 1 = right
        /// 0, -1 = left.
        /// </summary>
        public int Direction { get; internal set; } = 1;

        public AnimatedEntityTest()
        {
            SheetName = "Luigi";
            AnimationFPS = 250;
            FrameCount = 3;
            YFrameIndex = 0;
            SpriteSize = new Vector2(17, 30);
            Position = new Vector2(200, 200);
        }

        public new void Draw(Graphics.Graphics graphics)
        {
            base.Draw(graphics);
        }

        public new void Update(GameTime gameTime)
        {
            if (Direction > 0)
                this.YFrameIndex = 0;
            else if (Direction <= 0)
                this.YFrameIndex = 1;
            base.Update(gameTime);
        }
    }
}
