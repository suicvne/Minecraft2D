using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Minecraft2D.Options
{
    public class Options
    {
        [NonSerialized]
        public Texture2D SkinOverride;
        
        public bool UseController { get; set; }
        public bool ShowDebugInformation { get; set; }
        public bool Fullscreen { get; set; }
        public bool Vsync { get; set; }
        public System.Windows.Forms.FormWindowState WindowState { get; set; }
        public Point WindowLocation { get; set; }
        public Size WindowSize { get; set; }

        public Keys JumpKey { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }

        public string Username { get; set; }

#if DEBUG
        [NonSerialized]
        public bool LightsDisabled = false;
#endif

        public Options()
        {
            UseController = false;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;

            JumpKey = Keys.Space;
            MoveLeft = Keys.A;
            MoveRight = Keys.D;
            MoveUp = Keys.W;
            MoveDown = Keys.S;
            Vsync = true;
            Fullscreen = false;

            Username = "Player" + MainGame.RandomGenerator.Next(1000, 9000);
        }

        public void TryGetSkinFromServers()
        {
            //http://skins.minecraft.net/MinecraftSkins/%s.png
            try
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] bytes = wc.DownloadData("http://skins.minecraft.net/MinecraftSkins/" + Username.Trim() + ".png");
                    MemoryStream ms = new MemoryStream(bytes);
                    SkinOverride = Texture2D.FromStream(MainGame.GlobalGraphicsDevice, ms);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Couldn't get skin for username {Username.Trim()} from skins.minecraft.net\n    {ex.Message}"
                    );
                //http://s3.amazonaws.com/MinecraftSkins/%s.png
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        byte[] bytes = wc.DownloadData("http://s3.amazonaws.com/MinecraftSkins/" + Username.Trim() + ".png");
                        MemoryStream ms = new MemoryStream(bytes);
                        SkinOverride = Texture2D.FromStream(MainGame.GlobalGraphicsDevice, ms);
                    }
                }
                catch(Exception ex2)
                {
                    Console.WriteLine($"Couldn't get skin for username {Username.Trim()} from old skin server\n    {ex2.Message}"
                    );
                }
            }
        }
    }
}
