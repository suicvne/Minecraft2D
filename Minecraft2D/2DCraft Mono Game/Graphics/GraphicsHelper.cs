using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Graphics
{
    public class TexturePoint
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public TexturePoint(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }
    }

    public static class GraphicsHelper
    {
        public static void DrawRectangle(Rectangle coords, Color color, float opacityMod = 1f)
        {
            var rect = new Texture2D(MainGame.GlobalGraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            MainGame.GlobalSpriteBatch.Draw(rect, coords, color * opacityMod);
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

        private static RenderTarget2D textureStichingTarget;
        public static Texture2D BuildTextureFromParts(TexturePoint[] textures)
        {
            int totalWidth= 0, totalHeight = 0;
            foreach (var t in textures)
            {
                totalWidth += (int)t.Position.X > (int)textures[0].Position.X ? (int)t.Position.X : 0;
                totalHeight += (int)t.Position.Y > (int)textures[0].Position.Y ? (int)t.Position.Y : 0;
            }

            textureStichingTarget = new RenderTarget2D(MainGame.GlobalGraphicsDevice, totalWidth, totalHeight);

            MainGame.GlobalGraphicsDevice.SetRenderTarget(textureStichingTarget);
            using (SpriteBatch b = new SpriteBatch(MainGame.GlobalGraphicsDevice))
            {
                b.Begin();

                foreach (var t in textures)
                    b.Draw(t.Texture, new Rectangle((int)t.Position.X, (int)t.Position.Y, t.Texture.Width, t.Texture.Height), Color.White);

                b.End();
            }
            MainGame.GlobalGraphicsDevice.SetRenderTarget(null);

            return textureStichingTarget;
        }

        /// <summary>
        /// All points are relative to the texture.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        public static TexturePoint BuildTexturePointFromTexture(Texture2D text, Vector2 post)
        {
            return new TexturePoint(text, post);
        }

        

    }
}
