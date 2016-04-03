using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minecraft2DRebirth.Entity;
using Minecraft2DRebirth.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Screens.TestScreen
{
    public class PlayerTest : AnimatedEntityTest, IDynamicLightEntity
    {
        #region Dynamic Light Stuff
        public Color LightColor
        {
            get
            {
                return Color.White; 
            }
            set { }
        }

        public Vector2 LightOffset
        {
            get
            {
                return new Vector2(SpriteSize.X / 2, SpriteSize.Y / 2);
            }
            set { }
        }

        public float LightSize
        {
            get
            {
                return 0.35f;
            }
            set { }
        }
        #endregion


        #region Physics "Constants"
        public static float Friction = 0.000049804687f; //0.00049804687f;
        public const float WalkingAcceleration = .00083007812f * 12;
        public const float RunningAcceleration = .00083007812f * 32;
        public const float MaxSpeedX = 0.25859375f;
        public const int MaxAnimationFPS = 45;
        #endregion


        public PlayerTest()
        {
            SheetName = "Luigi";
            AnimationFPS = 250;
            FrameCount = 3;
            YFrameIndex = 0;
            SpriteSize = new Vector2(17, 30);
            Position = new Vector2(200, 200);
            CurrentDirection = Direction.Right;
        }

        public new void Draw(Graphics.Graphics graphics)
        {
            base.Draw(graphics);
        }

        private float runningFrame = WalkingAcceleration;
        public float xVelocity = 0.0f;
        private void UpdateX(GameTime gameTime)
        {
            float actualAcceleration = 0.0f;
            if (XMovement > 0) //right
                //if (Minecraft2D.inputHelper.IsCurPress(Keys.X))
                //{
                //    runningFrame += 0.00012f;
                //    actualAcceleration = Math.Min(runningFrame, RunningAcceleration);
                //}
                //else
                {
                    actualAcceleration = WalkingAcceleration;
                }
            else if (XMovement < 0)
                //if (Minecraft2D.inputHelper.IsCurPress(Keys.X))
                //{
                //    runningFrame += 0.000012f * gameTime.ElapsedGameTime.Milliseconds;
                //    actualAcceleration = Math.Max(-runningFrame, -RunningAcceleration);
                //}
                //else
                {
                //runningFrame -= Math.Min(0.000012f * gameTime.ElapsedGameTime.Milliseconds, -WalkingAcceleration);
                actualAcceleration = -WalkingAcceleration;
                }


            xVelocity = actualAcceleration * gameTime.ElapsedGameTime.Milliseconds;

            if (XMovement > 0) //right
                xVelocity = Math.Min(xVelocity, MaxSpeedX);
            else if (XMovement < 0)
                xVelocity = Math.Max(xVelocity, -MaxSpeedX);
            //else if(XMovement == 0)
            else if(XMovement == 0)
                xVelocity = xVelocity > 0.0f ?
                    Math.Max(0.0f, xVelocity - Friction * gameTime.ElapsedGameTime.Milliseconds) :
                    Math.Min(0.0f, xVelocity + Friction * gameTime.ElapsedGameTime.Milliseconds);



            AnimationFPS = Math.Max((1 / Math.Abs(xVelocity / 4)), MaxAnimationFPS);

            float deltaX = xVelocity * gameTime.ElapsedGameTime.Milliseconds;

            Position = new Vector2(Position.X + deltaX,
                Position.Y);
        }

        public void UpdateY(GameTime gameTime)
        {

        }

        private int XMovement = 0;

        

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Minecraft2D.InputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                Animating = true;
                CurrentDirection = Entity.AnimatedEntityTest.Direction.Right;
                XMovement = 1;
                UpdateX(gameTime);
            }
            else if (Minecraft2D.InputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                Animating = true;
                CurrentDirection = Entity.AnimatedEntityTest.Direction.Left;
                XMovement = -1;
                UpdateX(gameTime);
            }
            else
            {
                Animating = false;
                CurrentFrameIndex = 0; //reset
                XMovement = 0;
            }
        }

    }
}
