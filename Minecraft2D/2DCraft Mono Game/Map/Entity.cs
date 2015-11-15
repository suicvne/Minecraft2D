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
    [Serializable]
    public class Entity
    {
        private bool Moving { get; set; }
        private Skin skin { get; set; }
        public Rectangle Hitbox { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Movement { get; set; }
        public int Direction { get; set; }
        public string Name { get; set; }
        public int id { get; set; }

        public Entity()
        {
            Name = "Testificate";
            Position = new Vector2(22 * 32, 16 * 32);
            id = MainGame.RandomGenerator.Next(0001, 9999);
            Hitbox = new Rectangle((int)Position.X + 8, 0, 32, 64);
            Direction = 0;
            Movement = Vector2.Zero;
        }

        /// <summary>
        /// Moves to an absolute location.
        /// </summary>
        /// <param name="pos"></param>
        public void Move(Vector2 pos)
        {
            Position = pos;
        }

        public void Update(GameTime gameTime)
        {
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

        public void Draw(GameTime gameTime)
        {
            GraphicsHelper.DrawRectangle(new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height), Color.CornflowerBlue, 1f);
            if(MainGame.GameOptions.ShowDebugInformation)
            {
                DrawUsernameAbove();
                DrawNameAbove();
            }
        }

        private void DrawNameAbove()
        {
            float scale = .8f;
            Vector2 size = MainGame.CustomContentManager.SplashFont.MeasureString(Name);
            GraphicsHelper.DrawRectangle(new Rectangle((int)(Position.X - (size.X / 4)), (int)Position.Y - 32, (int)(size.X * scale) + 4, (int)(size.Y * scale)), Color.Gray, .3f);
            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.SplashFont, Name, new Vector2((int)(Position.X - (size.X / 4)), Position.Y - 32), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
        private void DrawUsernameAbove()
        {
            float scale = .8f;
            Vector2 size = MainGame.CustomContentManager.SplashFont.MeasureString(id.ToString());
            GraphicsHelper.DrawRectangle(new Rectangle((int)(Position.X - (size.X / 4)), (int)Position.Y - 16, (int)(size.X * scale) + 4, (int)(size.Y * scale)), Color.Gray, .3f);
            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.SplashFont, id.ToString(), new Vector2((int)(Position.X - (size.X / 4)), Position.Y - 16), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
