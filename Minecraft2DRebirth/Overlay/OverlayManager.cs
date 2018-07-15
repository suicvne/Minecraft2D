using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockSolidEngine.Overlay
{
    public class OverlayManager
    {
        public IOverlay CurrentOverlay { get; internal set; }
        public IOverlay PreviousOverlay { get; internal set; }

        public OverlayManager()
        {
            CurrentOverlay = null;
            PreviousOverlay = null;
        }

        public void PushOverlay(IOverlay overlay)
        {
            Console.WriteLine($"Being pushed {overlay.OverlayName}");
            PreviousOverlay = CurrentOverlay;
            CurrentOverlay = overlay;

            Console.WriteLine($"Current Overlay: {CurrentOverlay.OverlayName}");
        }

        public void Draw(Graphics.Graphics graphics)
        {
            graphics.GetSpriteBatch().Begin();
            if (CurrentOverlay != null)
                CurrentOverlay.Draw(graphics);
            graphics.GetSpriteBatch().End();
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentOverlay != null)
                CurrentOverlay.Update(gameTime);
        }
    }
}
