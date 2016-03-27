using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2DRebirth.Screens
{
    public class ScreenManager
    {
        public IScreen CurrentScreen { get; internal set; } = new BlankScreen();
        public IScreen PreviousScreen { get; internal set; } = null;

        public ScreenManager()
        {}

        public void PushScreen(ref IScreen screen)
        {
            PreviousScreen = CurrentScreen;
            CurrentScreen = screen;
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public void Draw(Graphics.Graphics graphics)
        {
            CurrentScreen.Draw(graphics);
        }
    }
}
