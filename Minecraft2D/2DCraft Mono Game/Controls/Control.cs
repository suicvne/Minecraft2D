using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Controls
{
    public abstract class Control
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public bool HasFocus { get; set; }
        public bool Enabled { get; set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
