using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2DRebirth.Native;
using Minecraft2DRebirth.Screens.TestScreen;
using Minecraft2DRebirth.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minecraft2DRebirth.Screens
{
    public class BlankScreen : IScreen
    {
        public override string ScreenName
        {
            get
            {
                return "Testing Screen";
            }
            internal set { }
        }

        public PlayerTest TestEntity;
        private RenderTarget2D renderTarget;
        public Lighted LightsRenderer;

        public BlankScreen(Graphics.Graphics graphics)
        {
            TestEntity = new PlayerTest();
            TestEntity.Animating = false;

            renderTarget = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, 
                graphics.ScreenRectangle().Width,
                graphics.ScreenRectangle().Height
            );

            LightsRenderer = new Lighted(graphics);
            LightsRenderer.AmbientLight = new Color(20, 20, 20   ); //Pure darkness
            graphics.ResolutionChanged += (sender, e) =>
            {
                Console.WriteLine($"[DebugScreen] Recreating render targets (New size: {e.Width}x{e.Height})");
                renderTarget.Dispose();
                renderTarget = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
                    graphics.ScreenRectangle().Width,
                    graphics.ScreenRectangle().Height
                );
            };
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(renderTarget);
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            {
                graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.AntiqueWhite);

                TestEntity.Draw(graphics);

                var mousePosition = Minecraft2D.inputHelper.MousePosition.OffsetForMouseInput();
                DrawCursor(graphics);
            }

            graphics.GetSpriteBatch().End();
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);

            LightsRenderer.BaseScene = renderTarget;
            LightsRenderer.Draw(graphics);
        }

        

        public override void Update(GameTime gameTime)
        {
            if(Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R)) //modify R
            {
                Color light = LightsRenderer.CursorLightColor;
                light.R += 1;
                if (light.R > 255)
                    light.R = 0;

                LightsRenderer.CursorLightColor = light;
            }
            if (Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G)) //modify G
            {
                Color light = LightsRenderer.CursorLightColor;
                light.G += 1;
                if (light.G > 255)
                    light.G = 0;

                LightsRenderer.CursorLightColor = light;
            }
            if (Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B)) //modify B
            {
                Color light = LightsRenderer.CursorLightColor;
                light.B += 1;
                if (light.B > 255)
                    light.B = 0;

                LightsRenderer.CursorLightColor = light;
            }

            if(Minecraft2D.inputHelper.IsNewPress(Input.MouseButtons.LeftButton) && Minecraft2D.inputHelper.IsMouseInsideWindow())
            {
                var point = Minecraft2D.inputHelper.MousePosition;
                point.X -= (512 / 2);
                point.Y -= (512 / 2);

                ((List<LightSource>)LightsRenderer.Lights).Add(LightSource.MakeLightSource(point.ToPoint(), LightsRenderer.CursorLightColor));
            }

            if (Minecraft2D.inputHelper.IsNewPress(Keys.Space))
                LightsRenderer.RenderLights = !LightsRenderer.RenderLights;

            TestEntity.Update(gameTime);
        }
    }
}
