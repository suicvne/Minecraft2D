using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Screens.TestScreen
{
    public class AnnoyingLightEntityTest : IDynamicLightEntity
    {
        #region Lighting
        public Color LightColor { get { return new Color(255, 165, 0); } set { } }
        public Vector2 LightOffset { get { return Vector2.Zero; } set { } }
        public float LightSize { get { return 0.75f; } set { } }
        #endregion
        #region Entity
        public Vector2 Position { get; set; }
        public string EntityName { get; set; } = "Annoying Light";
        public Rectangle Hitbox { get; set; } = Rectangle.Empty;
        public Vector2 SpriteSize { get; set; } = Vector2.Zero;
        #endregion

        public AnnoyingLightEntityTest()
        {
            Position = new Vector2(300, 100);
        }

        public void Draw(Graphics.Graphics graphics)
        {
            /*Nothing :D*/
        }

        public double Angle = 0d;
        private const double Amplitude = 1;
        private const double XMovementConstant = 0.85948;
        public void Update(GameTime gameTime)
        {
            //Angle += 2 * (gameTime.ElapsedGameTime.Milliseconds / 8);
            //Angle += 2;
            var position = Position;
            position.Y += (float)(Amplitude * Math.Sin(Angle.ToRadians()));
            position.X += (float)(Amplitude * Math.Cos(Angle.ToRadians()));
            ////position.X = (float)Angle.ToRadians().ToDegrees();
            //position.X += (float)(XMovementConstant * gameTime.ElapsedGameTime.Milliseconds);
            //if (position.X > 900)
            //    position.X = -200;

            Position = position;
        }
    }
}
