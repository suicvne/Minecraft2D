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
        private Skin skin { get; set; }
        public Rectangle Hitbox { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Movement { get; set; }
        public int Direction { get; set; }

        public Player()
        {
            Hitbox = new Rectangle(0, 0, 32, Convert.ToInt32(32 * 2));
            Position = new Vector2(22 * 32, 30 * 32);
            skin = new Skin(MainGame.CustomContentManager.GetTexture("default"));
            Direction = 0; //left
            Movement = Vector2.Zero;
        }

        public void Move(Vector2 pos)
        {
            Position += pos;
        }
        
        public void Update(GameTime gameTime)
        {
            CheckKeyboardMovement();
            AffectGravity();
            SimulateFriction();
            MoveIfPossible(gameTime);
            //UpdatePositionBasedOnMovement(gameTime);
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
                //if (!MainGameScreen.world.HasRoomForEntity(new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height)))
                //{
                //    Position = oldPosition;
                //}
                Position = MainGameScreen.world.WhereCanIGo(oldPosition, Position, Hitbox);
                Console.WriteLine(Position);
            }
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
            onePixelLower.Offset(0, 1);
            return !MainGameScreen.world.HasRoomForEntity(onePixelLower, true);
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
            { Movement += new Vector2(-.2f, 0); }
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(MainGame.GameOptions.MoveRight))
            { Movement += new Vector2(.2f, 0); }
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(MainGame.GameOptions.JumpKey) && IsOnFirmGround())
            {
                Movement = -Vector2.UnitY * 15;
            }
        }

        private int mod = 0;
        public void Draw(GameTime gameTime)
        {
            Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
            Vector2 worldMousePosition = Vector2.Transform(new Vector2(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y), inverseViewMatrix);

            float distanceX = worldMousePosition.X - Position.X;
            float distanceY = worldMousePosition.Y - Position.Y;
            float angle = (float)Math.Atan2(distanceY, distanceX);

            mod++;

            if (Direction == 0)
            {
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, new Rectangle(
                    (int)Position.X + 8, (int)Position.Y + 4, 16, 16
                    ), new Rectangle(skin.HeadLeft.X, skin.HeadLeft.Y, 8, 8), Color.White/*, (Math.Abs(-angle)), new Vector2(4, 8), SpriteEffects.None, 0f*/);
                //MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, new Rectangle(
                //    (int)Position.X + 16, (int)Position.Y + 16, 16, 16
                //    ), new Rectangle(skin.HeadLeft.X, skin.HeadLeft.Y, 8, 8), Color.White/*, (Math.Abs(-angle)), new Vector2(4, 8), SpriteEffects.None, 0f*/);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, 
                    new Rectangle((int)Position.X + 12, (int)Position.Y + 16, 8, 24), 
                    new Rectangle(skin.LeftTorso.X, skin.LeftTorso.Y, 6, 12), Color.White);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                    new Rectangle((int)Position.X + 16, (int)Position.Y + 20, 8, 12 * 2),
                    new Rectangle(skin.OutsideArm.X, skin.OutsideArm.Y, 4, 12), Color.White, (angle - MathHelper.ToRadians(90)), new Vector2(2, 0), SpriteEffects.None, 0f);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                    new Rectangle((int)Position.X + 12, (int)Position.Y + 40, 8, 24),
                    new Rectangle(skin.OutsideLeg.X, skin.OutsideLeg.Y, 6, 12), Color.White);
                MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), 
                    new Rectangle((int)Position.X + 20, (int)Position.Y + 40, 8, 8), MainGameScreen.PlacingTile.TextureRegion.ToRectangle(), Color.White, angle - MathHelper.ToRadians(90), new Vector2(2, 0), SpriteEffects.None, 0f);
            }

            if (mod > 360)
                mod = 0;
        }
    }
}
