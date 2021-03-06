using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Graphics;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Minecraft2D.Screens;
using Microsoft.Xna.Framework.Input;
using Minecraft2D.Map;
using System.Reflection;
using Microsoft.Xna.Framework.Input.Touch;

namespace Minecraft2D
{
    public delegate void GameWindowClosing();
    public delegate void TextInputReceived(TextInputEventArgs e);
    public delegate void GameWindowResized(Vector2 NewSize);
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static string GameSaveDirectory { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Minecraft2D"); } }
        public static ScreenManager manager;
        public static Camera2D GameCamera { get; set; }
        public static Camera2D HudCam { get; set; }
        public static GraphicsDevice GlobalGraphicsDevice { get; set; }
        public static GraphicsDeviceManager GlobalGraphicsDeviceManager { get; set; }
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
        public static event GameWindowResized WindowResized;

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

        public static Version GameVersion = Assembly.GetEntryAssembly().GetName().Version;

        public static bool LINUX = false;

        public MainGame(bool Linux)
        {
            LINUX = Linux;
            try
            {
                PresetBlocks.LoadBlocksList();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("CRITICAL ERROR OCCURRED\n" + ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            graphics = new GraphicsDeviceManager(this);
            GlobalGraphicsDeviceManager = graphics;

            GameExiting = false;
            if (!LINUX)
                FormReference = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            else
            {
            }
            
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;

            
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
            if (!LINUX)
            {
                x = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - FormReference.Width / 2;//- this.Window.ClientBounds.Width;
                y = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - FormReference.Height / 2;// - this.Window.ClientBounds.Height;
                FormReference.Location = new System.Drawing.Point(x, y);

                FormReference.WindowState = GameOptions.WindowState;
                
            }
            GlobalGraphicsDeviceManager.SynchronizeWithVerticalRetrace = GameOptions.Vsync;
            //this.IsFixedTimeStep = GameOptions.Vsync;
            GlobalGraphicsDeviceManager.IsFullScreen = GameOptions.Fullscreen;
            GlobalGraphicsDeviceManager.ApplyChanges();

            GameStarting = false;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GlobalSpriteBatch = spriteBatch;
            GlobalContentManager = Content;
            GlobalGraphicsDevice = GraphicsDevice;

            CustomContentManager = new ContentManager();

            manager = new ScreenManager();
            manager.LoadContent();
        }
        
        protected override void UnloadContent()
        {
            manager.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {            
                if (GameExiting)
                {
                    try
                    { this.Exit(); }
                    catch { Console.WriteLine("Exit failed??"); }

                }

                GlobalInputHelper.Update();

                if (GlobalInputHelper.IsNewPress(Keys.F11))
                {
                    GlobalGraphicsDeviceManager.IsFullScreen = !GlobalGraphicsDeviceManager.IsFullScreen;
                    this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                    GlobalGraphicsDeviceManager.ApplyChanges();
                    GameOptions.Fullscreen = GlobalGraphicsDeviceManager.IsFullScreen;

                }

                if (GlobalInputHelper.IsNewPress(Keys.F12))
                {
                    GameOptions.Vsync = !GameOptions.Vsync;
                    GlobalGraphicsDeviceManager.SynchronizeWithVerticalRetrace = !GlobalGraphicsDeviceManager.SynchronizeWithVerticalRetrace;
                    //this.IsFixedTimeStep = !IsFixedTimeStep;
                    GlobalGraphicsDeviceManager.ApplyChanges();
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
