using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2DRebirth.Overlay
{
    public abstract class IOverlay
    {
        public abstract string OverlayName { get; internal set; }
        public abstract void Draw(Graphics.Graphics graphics);
        public abstract void Update(GameTime gameTime);
    }
}
