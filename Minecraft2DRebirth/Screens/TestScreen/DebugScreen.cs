using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2DRebirth.Native;
using Minecraft2DRebirth.Screens.TestScreen;

namespace Minecraft2DRebirth.Screens
{
    internal class PhysicsConstants
    {
        public const float Friction = 0.00049804687f;
        public const float WalkingAcceleration = 0.00083007812f;
        public const float MaxSpeedX = 0.15859375f;
    }

    public class BlankScreen : IScreen
    {
        public override string ScreenName
        {
            get
            {
                return "Blank";
            }
            internal set { }
        }

        private AnimatedEntityTest TestEntity;

        public BlankScreen()
        {
            TestEntity = new AnimatedEntityTest();
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            {
                graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.AntiqueWhite);

                TestEntity.Draw(graphics);

                var mousePosition = Minecraft2D.inputHelper.MousePosition.OffsetForMouseInput();
                DrawCursor(graphics);
            }

            graphics.GetSpriteBatch().End();
        }
        
        private void UpdateX(GameTime gameTime)
        {
            float actualAcceleration = 0.0f;
            if (TestEntity.Direction > 0) //right
                actualAcceleration = PhysicsConstants.WalkingAcceleration;
            else if (TestEntity.Direction < 0)
                actualAcceleration = -PhysicsConstants.WalkingAcceleration;

            float xVelocity = actualAcceleration * gameTime.ElapsedGameTime.Milliseconds;

            if (TestEntity.Direction > 0) //right
                xVelocity = Math.Min(xVelocity, PhysicsConstants.MaxSpeedX);
            else if (TestEntity.Direction < 0)
                xVelocity = Math.Max(xVelocity, -PhysicsConstants.MaxSpeedX);

            ///Always on ground so, always calculating this

            xVelocity = xVelocity > 0.0f ? 
                Math.Max(0.0f, xVelocity - PhysicsConstants.Friction * gameTime.ElapsedGameTime.Milliseconds) : 
                Math.Min(0.0f, xVelocity + PhysicsConstants.Friction * gameTime.ElapsedGameTime.Milliseconds);

            TestEntity.AnimationFPS = 1 / xVelocity;

            float deltaX = xVelocity * gameTime.ElapsedGameTime.Milliseconds;

            TestEntity.Position = new Vector2(TestEntity.Position.X + deltaX,
                TestEntity.Position.Y);
        }

        public override void Update(GameTime gameTime)
        {
            if (Minecraft2D.inputHelper.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Space))
                TestEntity.Animating = !TestEntity.Animating;
            if (Minecraft2D.inputHelper.IsCurPress(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                TestEntity.Direction = 1;
            }
            else if (Minecraft2D.inputHelper.IsCurPress(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                TestEntity.Direction = -1;
            }

            UpdateX(gameTime);

            TestEntity.Update(gameTime);
        }
    }
}
