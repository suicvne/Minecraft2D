using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minecraft2D.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.BitmapFonts;
using Minecraft2D.Controls;

namespace Minecraft2D.Screens
{
    public class TitleScreen : Screen
    {
        private string[] splashMessages = new string []
        {
            "Terraria!",
            "This is a long message btw",
            "20 GOTO 10!",
            "10% bug free!",
            "Follow the train, CJ!",
            "Not Terraria!",
            "Jaclyn is qt!",
            "3.14159",
            "All assets belong to Mojang!",
            "Windows 10 is eh!",
            "Tune lower!",
            "00-00-001~",
            "!!",
            "Can't see me!"
        };
        
        float scale = 1.3f;
        Random ran = new Random(DateTime.Now.Millisecond);
        int splashIndex = 0;

        public void AdvanceSplash()
        {
            ran.Next(0, splashMessages.Length);
            splashIndex = ran.Next(0, splashMessages.Length);
        }

        public TitleScreen()
        {
            Button ExitButton = new Button
                ( 
                    new Rectangle(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth),
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 120, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), 
                    "Exit"
                );

            Button OptionsButton = new Button(new Rectangle(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDevice.Viewport.Height - 190, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Options");
            Button PlayButton = new Button(new Rectangle(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDevice.Viewport.Height - 260, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Load Test World");

            OptionsButton.Enabled = true;

            ExitButton.Clicked += ExitButton_Clicked;
            PlayButton.Clicked += () => 
            {
                MainGame.manager.PushScreen(GameScreens.GAME);
            };
            OptionsButton.Name = "OptionsButton";
            OptionsButton.Clicked += () =>
            {
                if (OptionsButton.ButtonText == "Options")
                    MainGame.manager.PushScreen(GameScreens.OPTIONS);
                else
                    MainGame.manager.PushScreen(GameScreens.DEBUGINFO);
            };

            AddControl(ExitButton);
            AddControl(OptionsButton);
            AddControl(PlayButton);

            ran.Next(0, splashMessages.Length);
            ran.Next(0, splashMessages.Length);
            splashIndex = ran.Next(0, splashMessages.Length);
        }

        private void ExitButton_Clicked()
        {
            MainGame.WriteSettings();
            MainGame.GameExiting = true;
        }

        float textScale = 1f;
        public override void Draw(GameTime gameTime)
        {
            int tx, ty;
            tx = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Width / 32);
            ty = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Height / 32);

            MainGame.GlobalGraphicsDevice.Clear(Color.CornflowerBlue);
            
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            
            for(int x = 0; x < tx; x++)
                for(int y = 0; y < ty; y++)
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(16 * 2, 0, 16, 16), new Color(100, 100, 100));

            //MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), new Rectangle((MainGame.GlobalGraphicsDevice.Viewport.Width / 2) - WidgetsMap.Minec.RegionWidth, 32, 155 , 44), WidgetsMap.Minec.ToRectangle(), Color.White);
            //MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), new Rectangle((MainGame.GlobalGraphicsDevice.Viewport.Width / 2), 31, 119, 44), WidgetsMap.raft.ToRectangle(), Color.White);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), 
                new Rectangle((int)((MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - ((MainGame.CustomContentManager.GetTexture("minecraft-logo").Width * scale) / 2))), 40, 
                (int)(MainGame.CustomContentManager.GetTexture("minecraft-logo").Width * scale), 
                (int)(MainGame.CustomContentManager.GetTexture("minecraft-logo").Height * scale)), Color.White);

            foreach (var c in ControlsList)
                c.Draw(gameTime);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32), Color.White);

            DrawText("Minecraft 2D Alpha " + MainGame.GameVersion.ToString(), new Vector2(2, MainGame.GlobalGraphicsDevice.Viewport.Height - 18), Color.DarkGray);

            int splashX = 480 - (int)((MainGame.CustomContentManager.SplashFont.MeasureString(splashMessages[splashIndex]).X) / 16f);


            Vector2 expansionCenter = MainGame.CustomContentManager.SplashFont.MeasureString(splashMessages[splashIndex]);
            expansionCenter.X = expansionCenter.X / 4;

            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.SplashFont, splashMessages[splashIndex], 
                new Vector2(splashX, 120), Color.Yellow, rotation: -.25f, origin: expansionCenter, scale: textScale, effects: SpriteEffects.None, layerDepth: 0f
                    );
            //DrawText(splashMessages[splashIndex], new Vector2(200, 200), Color.Yellow);

            //MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"),
            //    "Minecraft 2D Alpha", new Vector2(2, MainGame.GlobalGraphicsDevice.Viewport.Height - 18), Color.DarkGray);

            if (reverseTextScale)
                textScale = (float)(textScale - .003f * gameTime.ElapsedGameTime.TotalMilliseconds);
            else
                textScale = (float)(textScale + .003f * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (textScale > 2.0f)
                reverseTextScale = true;
            if (textScale < 1f)
                reverseTextScale = false;
            

            MainGame.GlobalSpriteBatch.End();
        }
        bool reverseTextScale = false;

        public static void DrawText(string text, Vector2 position, Color tint)
        {
            if (tint == null)
                tint = Color.White;

            Vector2 offsetPos = new Vector2(position.X + 2, position.Y + 2);
            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), text, offsetPos, Color.Black
                    );
            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("main-font"), text, position, tint
                    );
        }

        public override void Update(GameTime gameTime)
        {
            if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
            {
                Button optionsButton = (Button)ControlsList.Find(x => x.Name == "OptionsButton");
                optionsButton.ButtonText = "Debug Info";
            }
            else
            {
                Button optionsButton = (Button)ControlsList.Find(x => x.Name == "OptionsButton");
                optionsButton.ButtonText = "Options";
            }
            foreach (var c in ControlsList)
                c.Update(gameTime);
        }
    }
}
