using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2DRebirth.Screens
{
    public abstract class IScreen
    {
        public abstract string ScreenName { get; internal set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(Graphics.Graphics graphics);
    }
}
