using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace XNBExporter
{
    public delegate void ErrorReceived(Exception e);
    public class PreviewGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private string fileToPreview;
        private Texture2D textureToPreview;
        public event ErrorReceived ErrorOccurred;

        public PreviewGame(string file)
        {
            fileToPreview = file;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Path.GetDirectoryName(file);
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (toContinue)
            {
                this.Window.Title = "Previewing " + Path.GetFileName(fileToPreview);
                this.Window.AllowUserResizing = true;
                this.IsMouseVisible = true;
            }
        }

        bool toContinue = true;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            try
            {
                textureToPreview = Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(fileToPreview));
            }
            catch(Exception ex)
            {
                if (ErrorOccurred != null)
                    ErrorOccurred(ex);
                toContinue = false;
            }

            if (toContinue)
            {
                System.Windows.Forms.Control windowFromHandle = System.Windows.Forms.Control.FromHandle(this.Window.Handle);


                if (textureToPreview.Width < windowFromHandle.MinimumSize.Width)
                    graphics.PreferredBackBufferWidth = textureToPreview.Width * 4;
                else
                    graphics.PreferredBackBufferWidth = textureToPreview.Width;

                if (textureToPreview.Height < windowFromHandle.MinimumSize.Height)
                    graphics.PreferredBackBufferWidth = textureToPreview.Height * 4;
                else
                    graphics.PreferredBackBufferHeight = textureToPreview.Height;

                graphics.ApplyChanges();
            }
        }
        
        protected override void UnloadContent()
        {
            textureToPreview.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(textureToPreview, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
