using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Graphics;

namespace Minecraft2DRebirth.Controls
{
    public class SplashLabel : IControl
    {
        /// <summary>
        /// The scale for the text.
        /// </summary>
        private double Scale = 1f;

        private float SinAngle = 90; 
        private float AngularVelocity = 210 / 1000f; //45 degrees per second

        private Random rng = new Random((int)DateTime.Now.Ticks);

        private string[] splashMessages = new string[]
        {
            "Terraria!",
            "This is a long message btw",
            "20 GOTO 10!",
            "10% bug free!",
            "Follow the train, CJ!",
            "Not Terraria!",
            "3.14159",
            "All assets belong to Mojang!",
            "Windows 10 is eh!",
            "Tune lower!",
            "!!",
            "Can't see me!",
            "Your son is a nuke!",
            "Eat me? Yes.",
            "LinusTechTips",
            "Mono is eh!",
            "DirectX 9! I think..",
            "doot doot",
            $"You're the star, {typeof(Minecraft2D).ToString()}",
            "using MonoGame.Framework!",
            "Indie!",
            "idspispopd!",
            "Some of these are Notch's!",
            "C#6 is a lifesaver!",
            "Only 5gb on the CD!",
            "missingno"
        };

        private string Text { get; set; }

        public SplashLabel(bool useRandomSplash = true)
        {
            if(useRandomSplash)
            {
                Text = splashMessages[rng.Next(0, splashMessages.Length - 1)];
            }
        }

        public override void Draw(Graphics.Graphics graphics)
        {
            var spriteFont = graphics.GetSpriteFontByName("minecraft");
            var textSize = spriteFont.MeasureString(Text);
            var center = textSize / 2;
            graphics.GetSpriteBatch().DrawString (
                spriteFont, 
                Text, 
                PositionSize.ToVector2(), 
                Color.Yellow, 
                (float)DegreesToRadians(-20),
                center, 
                (float)Scale,
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f
            );
        }

        private double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        private const int Amplitude = 6;
        public override void Update(GameTime gameTime)
        {
            SinAngle += AngularVelocity * gameTime.ElapsedGameTime.Milliseconds;
            Scale = 1 + Math.Abs(2 * (Math.Sin(DegreesToRadians(SinAngle))));
        }
    }
}
