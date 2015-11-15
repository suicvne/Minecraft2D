using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Graphics;
using Minecraft2D.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map
{
    public class Player
    {
        private bool Moving { get; set; }
        private Skin skin { get; set; }
        public Rectangle Hitbox { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Movement { get; set; }
        public int Direction { get; set; }
        public bool IsClientPlayer { get; set; }
        public string Username { get; set; }

        public Player()
        {
            IsClientPlayer = false;
            Position = new Vector2(22 * 32, 16 * 32);
            Hitbox = new Rectangle((int)Position.X + 8, 0, 32, 64);
            if (MainGame.GameOptions != null)
            {
                MainGame.GameOptions.TryGetSkinFromServers();
                if (MainGame.GameOptions.SkinOverride != null)
                    skin = new Skin(MainGame.GameOptions.SkinOverride);
                else
                    skin = new Skin(MainGame.CustomContentManager.GetTexture("default"));
            }
            Direction = 0; //left
            Movement = Vector2.Zero;
            if (IsClientPlayer)
                Username = MainGame.GameOptions.Username;
            else
                Username = "Connected Player";

        }

        /// <summary>
        /// Moves to an absolute position.
        /// </summary>
        /// <param name="pos"></param>
        public void Move(Vector2 pos)
        {
            Position = pos;
        }
        
        public void Update(GameTime gameTime)
        {
            if (IsClientPlayer)
            {
                CheckKeyboardMovement();
            }
                AffectGravity();
                SimulateFriction();
                MoveIfPossible(gameTime);
        }

        private void AffectGravity()
        {
            Movement += Vector2.UnitY * .5f;
        }

        private void MoveIfPossible(GameTime gameTime)
        {
            Vector2 oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            if (MainGameScreen.world != null)
            {
                Position = MainGameScreen.world.WhereCanIGo(oldPosition, Position, Hitbox);
            }
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
            onePixelLower.Offset(0, 1);
            return !MainGameScreen.world.HasRoomForEntity(onePixelLower, true, false);
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void SimulateFriction()
        {
            Movement -= Movement * Vector2.One * .1f;
        }

        private void CheckKeyboardMovement()
        {
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(MainGame.GameOptions.MoveLeft))
            { Movement += new Vector2(-.2f, 0); Moving = true; }
            else { Moving = false; }
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(MainGame.GameOptions.MoveRight))
            { Movement += new Vector2(.2f, 0); Moving = true; }
            else { Moving = false; }
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(MainGame.GameOptions.JumpKey) && IsOnFirmGround())
            {
                Movement = -Vector2.UnitY * 15;
                Moving = true;
            }
            else
            {
                Moving = false;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (IsClientPlayer)
            {
                DrawClientPlayer();
            }
            else
            {
                GraphicsHelper.DrawRectangle(new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height), Color.Red, 1f);
            }
            DrawUsernameAbove();
        }

        private void DrawUsernameAbove()
        {
            float scale = .8f;
            Vector2 size = MainGame.CustomContentManager.SplashFont.MeasureString(this.Username);
            GraphicsHelper.DrawRectangle(new Rectangle((int)(Position.X - (size.X / 4)), (int)Position.Y - 16, (int)(size.X * scale) + 4, (int)(size.Y * scale)), Color.Gray, .3f);
            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.SplashFont, Username, new Vector2((int)(Position.X - (size.X / 4)), Position.Y - 16), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        private void DrawClientPlayer()
        {
            Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
            Vector2 worldMousePosition = Vector2.Transform(new Vector2(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y), inverseViewMatrix);

            if (IsClientPlayer)
            {
                float distanceX = worldMousePosition.X - Position.X;
                float distanceY = worldMousePosition.Y - Position.Y;
                float angle = (float)Math.Atan2(distanceY, distanceX);


                if (Direction == 0)
                {
                    bool hFlip = worldMousePosition.X > Position.X ? true : false;
                    //Body
                    MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                        new Rectangle((int)Position.X + 12, (int)Position.Y + 16, 8, 24),
                        new Rectangle(skin.LeftTorso.X - 4, skin.LeftTorso.Y, 6, 12), Color.White, 0, Vector2.Zero, hFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                    //Head

                    MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, new Rectangle(
                        (int)Position.X + 8, (int)Position.Y + 4, 16, 16
                        ), new Rectangle(skin.HeadLeft.X - 4, skin.HeadLeft.Y, 8, 8), Color.White, 0, Vector2.Zero, hFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                    //Leg
                    MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                        new Rectangle((int)Position.X + 12, (int)Position.Y + 40, 8, 24),
                        new Rectangle(skin.OutsideLeg.X - 4, skin.OutsideLeg.Y, 6, 12), Color.White, 0, Vector2.Zero, hFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                    //Arm
                    MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                        new Rectangle((int)Position.X + 16, (int)Position.Y + 24, 8, 12 * 2),
                        new Rectangle(skin.OutsideArm.X, skin.OutsideArm.Y, 4, 12), Color.White, (angle - MathHelper.ToRadians(90)), new Vector2(2, 0), SpriteEffects.None, 0f);
                    //MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"),
                    //    new Rectangle((int)Position.X + 18, (int)Position.Y + 40, 12, 12),
                    //    (MainGameScreen.PlacingTile.TextureRegion.ToRectangle()), Color.White, (angle - MathHelper.ToRadians(90)), new Vector2(2, 0), SpriteEffects.None, 0f);
                }
            }
        }
        
        private static void RotatePoints(ref Vector2 origin, float radians,
    ref Vector2[] Vectors)
        {
            Matrix myRotationMatrix = Matrix.CreateRotationZ(radians);

            for (int i = 0; i < 9; i++)
            {
                // Rotate relative to origin.
                Vector2 rotatedVector =
                    Vector2.Transform(Vectors[i] - origin, myRotationMatrix);

                // Add origin to get final location.
                Vectors[i] = rotatedVector + origin;
            }
        }
    }
}
