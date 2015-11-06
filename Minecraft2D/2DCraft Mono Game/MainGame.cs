using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Graphics;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Minecraft2D.Screens;
using Microsoft.Xna.Framework.Input;

namespace Minecraft2D
{
    public delegate void GameWindowClosing();
    public delegate void TextInputReceived(TextInputEventArgs e);
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
        public static int V_WIDTH = 800, V_HEIGHT = 480;
        public static Rectangle ClientBounds { get; internal set; }
        public static bool GameExiting { get; set; }

        public static Random RandomGenerator = new Random(DateTime.Now.Millisecond);

        public static event GameWindowClosing WindowClosing;
        public static event TextInputReceived TextInputReceived;

        private System.Windows.Forms.Form FormReference;

        #region Ugly PInvoke Stuff
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        internal const UInt32 SC_CLOSE = 0xF060;
        internal const UInt32 MF_ENABLED = 0x00000000;
        internal const UInt32 MF_GRAYED = 0x00000001;
        internal const UInt32 MF_DISABLED = 0x00000002;
        internal const uint MF_BYCOMMAND = 0x00000000;
        #endregion

        private bool GameStarting = true;
        public MainGame()
        {
            GameExiting = false;
            FormReference = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += (sender, e) =>
            {
                if(manager != null)
                    manager.RecalculateMinMax();
            };

            FormReference.Resize += (sender, e) =>
            {
                if (!GameStarting)
                {
                    GameOptions.WindowState = FormReference.WindowState;
                    GameOptions.WindowSize = FormReference.Size;
                    GameOptions.WindowLocation = FormReference.Location;
                }
            };
            FormReference.FormClosing += (sender, e) => 
            {
                if (WindowClosing != null)
                    WindowClosing();
            };
            this.Window.TextInput += (sender, e) =>
            {
                if (TextInputReceived != null)
                    TextInputReceived(e);
            };

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
            //EnableOrDisableCloseButton(false);

            int x, y;
            x = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - FormReference.Width / 2;//- this.Window.ClientBounds.Width;
            y = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - FormReference.Height / 2;// - this.Window.ClientBounds.Height;
            FormReference.Location = new System.Drawing.Point(x, y);

            FormReference.WindowState = GameOptions.WindowState;
            if(GameOptions.WindowLocation != null)
                FormReference.Location = GameOptions.WindowLocation;
            if(GameOptions.WindowSize != null)
                FormReference.Size = GameOptions.WindowSize;

            graphics.SynchronizeWithVerticalRetrace = GameOptions.Vsync;
            this.IsFixedTimeStep = GameOptions.Vsync;
            graphics.IsFullScreen = GameOptions.Fullscreen;
            graphics.ApplyChanges();

            GameStarting = false;
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
            if(GameExiting)
            {
                try
                { this.Exit(); }
                catch { Console.WriteLine("Exit failed??"); }
                
            }

            GlobalInputHelper.Update();

            if(GlobalInputHelper.IsNewPress(Keys.F11))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
                GameOptions.Fullscreen = graphics.IsFullScreen;
            }

            if(GlobalInputHelper.IsNewPress(Keys.F12))
            {
                GameOptions.Vsync = !GameOptions.Vsync;
                graphics.SynchronizeWithVerticalRetrace = !graphics.SynchronizeWithVerticalRetrace;
                this.IsFixedTimeStep = !IsFixedTimeStep;
                graphics.ApplyChanges();
            }

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
