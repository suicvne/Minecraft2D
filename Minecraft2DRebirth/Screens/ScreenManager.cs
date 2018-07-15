using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RockSolidEngine.Screens
{
    public class ScreenManager
    {
        public IScreen CurrentScreen { get; internal set; } //TODO: make proper blank screen without a graphics argument
        public IScreen PreviousScreen { get; internal set; } = null;

        public ScreenManager()
        {}

        public void PushScreen(IScreen screen)
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
