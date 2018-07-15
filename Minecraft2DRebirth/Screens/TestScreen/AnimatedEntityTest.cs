using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RockSolidEngine.Entity;
using RockSolidEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Screens.TestScreen
{
    struct CollisionInfo
    {
        public bool Collided;
        public int tx, ty;
    }

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

        public const float MaxSpeedY = 0.325f;
        public const float Gravity = 0.0012f;
        public const float JumpGravity = 0.25f;

        public const int MaxAnimationFPS = 45;
        #endregion

        private bool IsOnGround { get; set; } = false;

        public new Rectangle Hitbox { get; private set; }

        public PlayerTest()
        {
            SheetName = "Luigi";
            AnimationFPS = 250;
            FrameCount = 3;
            YFrameIndex = 0;
            SpriteSize = new Vector2(17, 30);
            Position = new Vector2(200, 50);
            CurrentDirection = Direction.Right;
            Hitbox = new Rectangle(0, 0, 32, 64);
        }

        public new void Draw(Graphics.Graphics graphics)
        {
            if (!IsOnGround)
                CurrentFrameIndex = 2;

            base.Draw(graphics);
        }

        private CollisionInfo GetCollisionInfoFromMap(BasicTileMap map, Rectangle rectangle)
        {
            CollisionInfo info = new CollisionInfo
            {
                Collided = false,
                tx = 0,
                ty = 0
            };
            var tiles = map.GetCollidingTiles(rectangle);
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].Transparency == Maps.TileTransparency.FullyOpague)
                {
                    if (tiles[i].Hitbox.Intersects(Hitbox))
                    {
                        info.Collided = true;
                        info.tx = (int)Math.Floor(tiles[i].Position.X / Constants.TileSize);
                        info.ty = (int)Math.Floor(tiles[i].Position.Y / Constants.TileSize);
                        break;
                    }
                }
            }
            return info;
        }

        private float runningFrame = WalkingAcceleration;
        public float xVelocity = 0.0f;
        private void UpdateY(GameTime gameTime, BasicTileMap map)
        {
            float gravity = (velocity_y > 0f ? JumpGravity : Gravity);

            velocity_y = Math.Min(velocity_y + gravity * gameTime.ElapsedGameTime.Milliseconds, MaxSpeedY);

            float delta = velocity_y * gameTime.ElapsedGameTime.Milliseconds;

            if (delta > 0) //going down
            {
                if (!IsOnGround)
                {
                    CollisionInfo info = GetCollisionInfoFromMap(map, Hitbox);
                    if (info.Collided)
                    {
                        Position = new Vector2(Position.X, (info.ty * Constants.TileSize) - Hitbox.Height);
                        velocity_y = 0f;
                        IsOnGround = true;
                    }
                    else
                    {
                        Position = new Vector2(Position.X, Position.Y + delta);
                        IsOnGround = false;
                    }
                }
            }
            else //up
            {
                Position = new Vector2(Position.X, Position.Y + delta);
            }
        }
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

        private int XMovement = 0;
        private float velocity_y = 0f;
        
        public new void Update(GameTime gameTime) { throw new Exception(); }
        public void Update(GameTime gameTime, BasicTileMap map)
        {
            base.Update(gameTime);
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

            UpdateY(gameTime, map);
            if (Minecraft2D.InputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                Animating = true;
                CurrentDirection = Entity.AnimatedEntityTest.Direction.Right;
                XMovement = 1;
                UpdateX(gameTime);
            }
            else if (Minecraft2D.InputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
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
            
            if (Minecraft2D.InputHelper.CurrentKeyboardState.IsKeyDown(Keys.Z))
            {
                IsOnGround = false;
                velocity_y = (velocity_y == 0 ? -.5f : velocity_y);
            }
        }

    }
}
