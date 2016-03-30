using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2DRebirth.Native;
using Minecraft2DRebirth.Screens.TestScreen;
using Minecraft2DRebirth.Graphics;

namespace Minecraft2DRebirth.Screens
{
    internal class PhysicsConstants
    {
        public const float Friction = 0.0049804687f; //0.00049804687f;
        public const float WalkingAcceleration = 0.0093007812f;
        public const float MaxSpeedX = 0.55859375f;
    }

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

        private AnimatedEntityTest TestEntity;
        private RenderTarget2D renderTarget;
        private Lighted LightsRenderer;

        public BlankScreen(Graphics.Graphics graphics)
        {
            TestEntity = new AnimatedEntityTest();
            TestEntity.Animating = false;

            renderTarget = new RenderTarget2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, 
                graphics.ScreenRectangle().Width,
                graphics.ScreenRectangle().Height
            );

            LightsRenderer = new Lighted(graphics);
            LightsRenderer.AmbientLight = new Color(20, 20, 20   ); //Pure darkness
            graphics.ResolutionChanged += (sender, e) =>
            {
                Console.WriteLine($"[DebugScreen] Recreating render targets (New size: {e.Width}x{e.Height}");
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
        
        private void UpdateX(GameTime gameTime)
        {
            float actualAcceleration = 0.0f;
            if (TestEntity.CurrentDirection == Entity.IAnimatedEntity.Direction.Right) //right
                actualAcceleration = PhysicsConstants.WalkingAcceleration;
            else if (TestEntity.CurrentDirection == Entity.IAnimatedEntity.Direction.Left)
                actualAcceleration = -PhysicsConstants.WalkingAcceleration;

            float xVelocity = actualAcceleration * gameTime.ElapsedGameTime.Milliseconds;

            if (TestEntity.CurrentDirection == Entity.IAnimatedEntity.Direction.Right) //right
                xVelocity = Math.Min(xVelocity, PhysicsConstants.MaxSpeedX);
            else if (TestEntity.CurrentDirection == Entity.IAnimatedEntity.Direction.Left)
                xVelocity = Math.Max(xVelocity, -PhysicsConstants.MaxSpeedX);

            ///Always on ground so, always calculating this

            xVelocity = xVelocity > 0.0f ? 
                Math.Max(0.0f, xVelocity - PhysicsConstants.Friction * gameTime.ElapsedGameTime.Milliseconds) : 
                Math.Min(0.0f, xVelocity + PhysicsConstants.Friction * gameTime.ElapsedGameTime.Milliseconds);

            TestEntity.AnimationFPS = (1 / Math.Abs(xVelocity / 4));

            float deltaX = xVelocity * gameTime.ElapsedGameTime.Milliseconds;

            TestEntity.Position = new Vector2(TestEntity.Position.X + deltaX,
                TestEntity.Position.Y);
        }

        public override void Update(GameTime gameTime)
        {
            if (Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                TestEntity.Animating = true;
                TestEntity.CurrentDirection = Entity.IAnimatedEntity.Direction.Right;
                UpdateX(gameTime);
            }
            else if (Minecraft2D.inputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                TestEntity.Animating = true;
                TestEntity.CurrentDirection = Entity.IAnimatedEntity.Direction.Left;
                UpdateX(gameTime);
            }
            else
            {
                TestEntity.Animating = false;
                TestEntity.CurrentFrameIndex = 0; //reset
            }

            TestEntity.Update(gameTime);
        }
    }
}
