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
using Minecraft2DRebirth.Entity;
using Minecraft2DRebirth.Scenes;

namespace Minecraft2DRebirth.Screens
{
    //TODO: Create an 'IScene' interface for a basic 2D scene involving entities
    //TODO: Create an 'ILightableScene' interface implementing everything in 'ILight' for direct integration with lighting.

    public class BlankScreen : IScreen
    {
        private BasicLightableScene Scene;

        public override string ScreenName
        {
            get
            {
                return "Testing Screen";
            }
            internal set { }
        }

        public PlayerTest TestEntity;

        public BlankScreen(Graphics.Graphics graphics)
        {
            TestEntity = new PlayerTest();
            TestEntity.Animating = false;

            Scene = new BasicLightableScene(graphics);
            Scene.AmbientLight = new Color(20, 20, 20);
            ((List<IEntity>)Scene.Entities).Add(TestEntity); //player entity
            ((List<IEntity>)Scene.Entities).Add(new AnnoyingLightEntityTest()); //lol

            //LightsRenderer.AmbientLight = new Color(20, 20, 20   ); //Pure darkness
            //graphics.ResolutionChanged += (sender, e) =>
            //{
            //    Console.WriteLine($"[DebugScreen] Recreating render targets (New size: {e.Width}x{e.Height})");
            //    renderTarget.Dispose();
            //    renderTarget = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice,
            //        graphics.ScreenRectangle().Width,
            //        graphics.ScreenRectangle().Height
            //    );
            //};
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            Scene.Draw(graphics);
        }
        
        public override void Update(GameTime gameTime)
        {
            Scene.Update(gameTime);
            //if(Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R)) //modify R
            //{
            //    Color light = LightsRenderer.CursorLightColor;
            //    light.R += 1;
            //    if (light.R > 255)
            //        light.R = 0;

            //    LightsRenderer.CursorLightColor = light;
            //}
            //if (Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G)) //modify G
            //{
            //    Color light = LightsRenderer.CursorLightColor;
            //    light.G += 1;
            //    if (light.G > 255)
            //        light.G = 0;

            //    LightsRenderer.CursorLightColor = light;
            //}
            //if (Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B)) //modify B
            //{
            //    Color light = LightsRenderer.CursorLightColor;
            //    light.B += 1;
            //    if (light.B > 255)
            //        light.B = 0;

            //    LightsRenderer.CursorLightColor = light;
            //}

            //if(Minecraft2D.inputHelper.IsNewPress(Input.MouseButtons.LeftButton) && Minecraft2D.inputHelper.IsMouseInsideWindow())
            //{
            //    var point = Minecraft2D.inputHelper.MousePosition;
            //    point.X -= (512 / 2);
            //    point.Y -= (512 / 2);

            //    ((List<LightSource>)LightsRenderer.Lights).Add(LightSource.MakeLightSource(point.ToPoint(), LightsRenderer.CursorLightColor));
            //}

            //if (Minecraft2D.inputHelper.IsNewPress(Keys.Space))
            //    LightsRenderer.RenderLights = !LightsRenderer.RenderLights;

            //TestEntity.Update(gameTime);
        }
    }
}
