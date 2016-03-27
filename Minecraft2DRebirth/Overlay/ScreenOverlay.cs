using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Graphics;
using Minecraft2DRebirth.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2DRebirth.Overlay
{
    class DebugOverlay : IOverlay
    {
        private readonly ScreenManager screenManager;
        public DebugOverlay(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        public override string OverlayName
        {
            get
            {
                return "DebugOverlay";
            }
            internal set
            {}
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            SpriteFont fallbackFont = graphics.GetSpriteFontByName("fallback");
            //Current screen should never be null. Previous could, however.
            graphics.GetSpriteBatch().DrawString(fallbackFont, $"CurrentScreen: {screenManager.CurrentScreen.ScreenName}", Vector2.Zero, Color.Black);
            if(screenManager.PreviousScreen != null)
            {
                graphics.GetSpriteBatch().DrawString(fallbackFont, $"PreviousScreen: {screenManager.PreviousScreen.ScreenName}", new Vector2(0, 16), Color.Black);
            }

            graphics.GetSpriteBatch().DrawString(fallbackFont, $"Mouse Position: {Minecraft2D.inputHelper.MousePosition.ToString()}; Inside: {Minecraft2D.inputHelper.IsMouseInsideWindow()}"
                , new Vector2(0, 32), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
