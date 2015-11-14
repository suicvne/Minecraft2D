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
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            { Movement += new Vector2(-2, 0); }
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            { Movement += new Vector2(1, 0); }
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z))
            { Movement += new Vector2(0, -2); }
        }

        public void Draw(GameTime gameTime)
        {
            if (Direction == 0)
            {
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, new Rectangle(
                    (int)Position.X, (int)Position.Y, 24, 24
                    ), new Rectangle(skin.HeadLeft.X, skin.HeadLeft.Y, 8, 8), Color.White);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin, 
                    new Rectangle((int)Position.X + 4, (int)Position.Y + 24, 6 * 3,  12 * 3), 
                    new Rectangle(skin.LeftTorso.X, skin.LeftTorso.Y, 6, 12), Color.White);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                    new Rectangle((int)Position.X + 8, (int)Position.Y + 24, 4 * 2, 12 * 2),
                    new Rectangle(skin.OutsideArm.X, skin.OutsideArm.Y, 4, 12), Color.White);
                MainGame.GlobalSpriteBatch.Draw(skin.FullSkin,
                    new Rectangle((int)Position.X + 8, (int)Position.Y + 60, 4 * 2, 12 * 2),
                    new Rectangle(skin.OutsideLeg.X, skin.OutsideLeg.Y, 4, 12), Color.White);
            }
        }
    }
}
