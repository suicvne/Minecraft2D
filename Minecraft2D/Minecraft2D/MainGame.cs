using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Minecraft2D.Graphics;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Minecraft2D
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static ScreenManager manager;
        public static Camera2D GameCamera { get; set; }
        public static Camera2D HudCam { get; set; }
        public static GraphicsDevice GlobalGraphicsDevice { get; set; }
        public static SpriteBatch GlobalSpriteBatch { get; set; }
        public static Microsoft.Xna.Framework.Content.ContentManager GlobalContentManager { get; set; }
        public static Graphics.ContentManager CustomContentManager { get; set; }
        public static Options.Options GameOptions { get; set; }
        public static InputHelper GlobalInputHelper { get; set; }
        public static BlendState Multiply = new BlendState()
        {
            AlphaSourceBlend = Blend.DestinationAlpha,
            AlphaDestinationBlend = Blend.Zero,
            AlphaBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero,
            ColorBlendFunction = BlendFunction.Add
        }; 

        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        internal const UInt32 SC_CLOSE = 0xF060;
        internal const UInt32 MF_ENABLED = 0x00000000;
        internal const UInt32 MF_GRAYED = 0x00000001;
        internal const UInt32 MF_DISABLED = 0x00000002;
        internal const uint MF_BYCOMMAND = 0x00000000;

        public MainGame()
        {
            EnableOrDisableCloseButton(true);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += (sender, e)=>manager.RecalculateMinMax();

            GameOptions = new Options.Options();
            if(File.Exists("Settings.json"))
                LoadSettings();
            else
                WriteSettings();

            this.Window.Title = "Minecraft 2D Alpha";
        }


        public void EnableOrDisableCloseButton(bool Enabled)
        {
            IntPtr hSystemMenu = GetSystemMenu(this.Window.Handle, false);
            EnableMenuItem(hSystemMenu, SC_CLOSE, (uint)(MF_ENABLED | (Enabled ? MF_ENABLED : MF_GRAYED)));
        }

        public static void LoadSettings()
        {
            if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Settings.json"))
            {
                JsonSerializer js = new JsonSerializer();
                js.Formatting = Formatting.Indented;
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Settings.json"))
                {
                    using (JsonReader jsr = new JsonTextReader(sr))
                    {
                        GameOptions = js.Deserialize<Options.Options>(jsr);
                    }
                }
            }
        }

        public static void WriteSettings()
        {
            JsonSerializer js = new JsonSerializer();
            js.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Settings.json"))
            {
                using (JsonWriter jsw = new JsonTextWriter(sw))
                {
                    js.Serialize(jsw, GameOptions);
                }
            }

        }


        protected override void Initialize()
        {
            base.Initialize();
            GlobalInputHelper = new InputHelper();
            EnableOrDisableCloseButton(false);

#if DEBUG
            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
#endif
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GlobalSpriteBatch = spriteBatch;
            GlobalContentManager = Content;
            GlobalGraphicsDevice = GraphicsDevice;

            manager = new ScreenManager();
            manager.LoadContent();
        }
        
        protected override void UnloadContent()
        {
            manager.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {
            GlobalInputHelper.Update();
            manager.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            manager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
