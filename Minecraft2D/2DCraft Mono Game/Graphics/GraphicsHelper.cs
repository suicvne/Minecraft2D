using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public static class GraphicsHelper
    {
        public static void DrawRectangle(Rectangle coords, Color color, float opMod)
        {
            var rect = new Texture2D(MainGame.GlobalGraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            MainGame.GlobalSpriteBatch.Draw(rect, coords, color * opMod);
        }

        public static void DrawText(string text, Vector2 position, Color tint)
        {
            if (tint == null)
                tint = Color.White;

            Vector2 offsetPos = new Vector2(position.X + 2, position.Y + 2);
            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.SplashFont, text, offsetPos, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f
                    );
            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.SplashFont, text, position, tint, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f

                    );
        }

    }
}
