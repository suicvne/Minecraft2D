using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RockSolidEngine.Graphics;
using RockSolidEngine.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace RockSolidEngine.Overlay
{
    class DebugOverlay : IOverlay
    {
        #region FPS Stuffs
        private int framerate = 0;
        private int frameCounter = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        #endregion

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
            frameCounter++;

            SpriteFont fallbackFont = graphics.GetSpriteFontByName("fallback");
            //Current screen should never be null. Previous could, however.
            graphics.GetSpriteBatch().DrawString(fallbackFont, $"CurrentScreen: {screenManager.CurrentScreen.ScreenName}", Vector2.Zero, Color.Black);
            if(screenManager.PreviousScreen != null)
            {
                graphics.GetSpriteBatch().DrawString(fallbackFont, $"PreviousScreen: {screenManager.PreviousScreen.ScreenName}", new Vector2(0, 16), Color.Black);
            }

            graphics.GetSpriteBatch().DrawString(fallbackFont,
                    $"FPS: " + framerate, new Vector2(0, 64), Color.Black);

            if (screenManager.CurrentScreen != null && screenManager.CurrentScreen.GetType() == typeof(BlankScreen))
            {
                var debugScreen = (BlankScreen)screenManager.CurrentScreen;
                //graphics.GetSpriteBatch().DrawString(fallbackFont,
                //    $"Lights: " + debugScreen.LightsRenderer.Lights.Count(), new Vector2(0, 64 + 16), Color.Black);
                graphics.GetSpriteBatch().DrawString(fallbackFont,
                    $"X-Velocity: " + debugScreen.TestEntity.xVelocity, new Vector2(0, 64 + 32), Color.Black);
            }

            graphics.GetSpriteBatch().DrawString(fallbackFont, $"Mouse Position: {Minecraft2D.InputHelper.MousePosition.ToString()}; Inside: {Minecraft2D.InputHelper.IsMouseInsideWindow()}"
                , new Vector2(0, 32), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if(elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                framerate = frameCounter;
                frameCounter = 0;
            }
        }
    }
}
