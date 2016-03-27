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
            var sprFont = contentManager.Load<SpriteFont>("Fallback");
            spriteFonts.Add("fallback", sprFont);

            textures.Add("cursor", contentManager.Load<Texture2D>("cursor"));

            Console.WriteLine("Loaded content.");
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
