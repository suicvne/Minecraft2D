using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2DRebirth.Entity
{
    public class IAnimatedEntity : IEntity
    {
        //TODO: abstract this
        public enum Direction
        {
            Left,
            Right,
            Neutral
        }

        #region Interface implementation
        private string _EntityName;
        public string EntityName { get { return _EntityName; } set { _EntityName = value; } }

        private Vector2 _Position;
        public Vector2 Position { get { return _Position; } set { _Position = value; } }

        private Rectangle _Hitbox;
        public Rectangle Hitbox
        {
            get { return _Hitbox; }
            set { _Hitbox = value; }
        }
        #endregion

        public Direction CurrentDirection { get; set; } = Direction.Right;

        /// <summary>
        /// The amount of frames in one animation.
        /// </summary>
        public int FrameCount { get; set; }

        /// <summary>
        /// Zero based index of the vertical index of animations.
        /// Ex: top row of sprites is FrameIndex=0
        /// </summary>
        public int YFrameIndex { get; set; }

        public float AnimationFPS { get; set; }

        public string SheetName { get; set; }

        /// <summary>
        /// The size of one sprite.
        /// </summary>
        public Vector2 SpriteSize { get; set; }

        /// <summary>
        /// The current frame being drawn in the animation.
        /// </summary>
        public int CurrentFrameIndex { get; internal set; }

        public bool Animating { get; internal set; } = true;

        private int ElapsedTime;

        public void Draw(Graphics.Graphics graphics)
        {
            Rectangle sourceRectangle = new Rectangle
            (
                (int)SpriteSize.X * CurrentFrameIndex,
                (int)SpriteSize.Y * YFrameIndex,
                (int)SpriteSize.X,
                (int)SpriteSize.Y
            );
            Rectangle destinationRectangle = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Math.Floor(SpriteSize.X * Constants.SpriteScale),
                (int)Math.Floor(SpriteSize.Y * Constants.SpriteScale)
            );
            var texture = graphics.GetTexture2DByName(SheetName);
            graphics.GetSpriteBatch().Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, 
                CurrentDirection == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
            0);
        }

        public void Update(GameTime gameTime)
        {
            if (Animating)
            {
                ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (ElapsedTime > AnimationFPS)
                {
                    CurrentFrameIndex++;
                    ElapsedTime = 0;
                    if (CurrentFrameIndex > FrameCount - 1)
                        CurrentFrameIndex = 0;
                }
            }
        }
    }
}
