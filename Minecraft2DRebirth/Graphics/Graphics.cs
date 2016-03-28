using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2DRebirth.Overlay;
using Minecraft2DRebirth.Screens;
using System;
using System.Collections.Generic;

namespace Minecraft2DRebirth.Graphics
{
    /// <summary>
    /// Handles graphics related things like special drawing, content management, etc.
    /// </summary>
    public class Graphics
    {
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> spriteFonts;

        private SpriteBatch spriteBatch;
        private ContentManager contentManager;
        private GraphicsDeviceManager graphicsDeviceManager;

        private ScreenManager screenManager;
        private OverlayManager overlayManager;

        public Graphics(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;
            this.graphicsDeviceManager = graphicsDeviceManager;

            screenManager = new ScreenManager();
            overlayManager = new OverlayManager();

            textures = new Dictionary<string, Texture2D>();
            spriteFonts = new Dictionary<string, SpriteFont>();
        }

        public void DebugModeStuff()
        {
            IOverlay screenOverlay = new DebugOverlay(screenManager);
            overlayManager.PushOverlay(screenOverlay);

            IScreen titleScreen = new TitleScreen();
            screenManager.PushScreen(titleScreen);
        }

        public void Draw()
        {
            screenManager.Draw(this);
            overlayManager.Draw(this);
        }

        public void Update(GameTime gameTime)
        {
            screenManager.Update(gameTime);
            overlayManager.Update(gameTime);
        }

        public void LoadContent()
        {
            spriteFonts.Add("fallback", contentManager.Load<SpriteFont>("Fallback"));
            spriteFonts.Add("minecraft", contentManager.Load<SpriteFont>("minecraft-fnt"));

            contentManager.RootDirectory = "Content";

            textures.Add("cursor", contentManager.Load<Texture2D>("crosshair"));
            textures.Add("widgets", contentManager.Load<Texture2D>("widgets"));
            textures.Add("terrain", contentManager.Load<Texture2D>("terrain"));

            Console.WriteLine("Loaded content.");
        }

        public void DrawText(string text, Vector2 position, Color tint)
        {
            if (tint == null)
                tint = Color.White;

            Vector2 offsetPos = new Vector2(position.X + 2, position.Y + 2); //offset used for shadow
            spriteBatch.DrawString(GetSpriteFontByName("minecraft"), text, offsetPos, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(GetSpriteFontByName("minecraft"), text, position, tint, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void DrawText(string text, Rectangle size, Color tint, float scale)
        {
            var textSize = GetSpriteFontByName("minecraft").MeasureString(text) * scale;
            if (tint == null)
                tint = Color.White;

            Vector2 origin;
            if (scale > 1f)
            {
                origin = textSize / 128;
                Console.WriteLine($"TextSize: " + textSize.ToString());
                Console.WriteLine("Origin: " + origin.ToString());
            }
            else
                origin = Vector2.Zero;


            Vector2 offsetPos = new Vector2(size.X + 2, size.Y + 2); //offset used for shadow
            spriteBatch.DrawString(GetSpriteFontByName("minecraft"), text, offsetPos, Color.Black, 0, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(GetSpriteFontByName("minecraft"), text, size.ToVector2(), tint, 0, origin, scale, SpriteEffects.None, 0f);
        }

        public Rectangle GetScreenViewport()
        {
            return GetGraphicsDeviceManager().GraphicsDevice.Viewport.ToRectangle();
        }

        public void UnloadContent()
        {
            foreach(var texture in textures)
                texture.Value.Dispose();
            foreach (var sprFont in spriteFonts)
                sprFont.Value.Texture.Dispose();

            textures = null;
            spriteFonts = null;
        }

        public SpriteBatch GetSpriteBatch() => spriteBatch;
        public GraphicsDeviceManager GetGraphicsDeviceManager() => graphicsDeviceManager;

        public Texture2D GetTexture2DByName(string name)
        {
            if (textures.ContainsKey(name))
                return textures[name];

            return null;
        }

        public SpriteFont GetSpriteFontByName(string name)
        {
            if (spriteFonts.ContainsKey(name))
                return spriteFonts[name];

            return null;
        }
    }
}
