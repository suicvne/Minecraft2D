using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Options
{
    public class Options
    {
        public bool UseController { get; set; }
        public bool ShowDebugInformation { get; set; }

        public Keys JumpKey { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }

        public Options()
        {
            UseController = false;

            JumpKey = Keys.Space;
            MoveLeft = Keys.A;
            MoveRight = Keys.D;
            MoveUp = Keys.W;
            MoveDown = Keys.S;
        }
    }
}
