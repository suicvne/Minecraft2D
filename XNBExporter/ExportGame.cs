using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace XNBExporter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ExportGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private string[] filesToExport;
        private string directoryForOutput;
        private SpriteFont sprFont;

        public ExportGame(string[] files, string outputDirectory)
        {
            filesToExport = files;
            directoryForOutput = outputDirectory;
            graphics = new GraphicsDeviceManager(this);
            if (files.Length == 0)
                Exit();
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            System.Windows.Forms.Form fromHandle = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(this.Window.Handle);
            fromHandle.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Content.RootDirectory = Path.GetDirectoryName(filesToExport[0]);
        }
        
        protected override void UnloadContent()
        {
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

            foreach (var file in filesToExport)
            {
                Window.Title = "Exporting " + Path.GetFileName(file);
                string saveAsPath = Path.Combine(directoryForOutput, Path.GetFileNameWithoutExtension(file) + ".png");
                Texture2D texture;
                try
                {
                    texture = Content.Load<Texture2D>(Path.GetFileNameWithoutExtension(file));
                }
                catch
                {
                    continue;
                }
                if (File.Exists(saveAsPath))
                    File.Delete(saveAsPath);
                using (Stream stream = File.Create(saveAsPath))
                {
                    if(texture != null)
                        texture.SaveAsPng(stream, texture.Width, texture.Height);
                }
            }

            Exit();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
