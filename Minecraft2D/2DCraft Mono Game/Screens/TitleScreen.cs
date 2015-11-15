﻿using System;
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

        #region Controls
        Button ExitButton, OptionsButton, PlayButton;
        #endregion

        public void AdvanceSplash()
        {
            ran.Next(0, splashMessages.Length);
            splashIndex = ran.Next(0, splashMessages.Length);
        }

        public TitleScreen()
        {
            ExitButton = new Button
                ( 
                    new Rectangle(MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferWidth / 2 - (WidgetsMap.EnabledButton.RegionWidth),
                    MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 120, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), 
                    "Exit"
                );

            OptionsButton = new Button(new Rectangle(MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 190, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Options");
            PlayButton = new Button(new Rectangle(MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 260, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Load Test World");

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

        private void RepositionButtons()
        {
            ExitButton.Position = new Rectangle(MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferWidth / 2 - (WidgetsMap.EnabledButton.RegionWidth),
                    MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 120, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2);
            OptionsButton.Position = new Rectangle(MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 190, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2);
            PlayButton.Position = new Rectangle(MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 260, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2);
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
            tx = (int)Math.Ceiling((double)MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferWidth / 32);
            ty = (int)Math.Ceiling((double)MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 32);

            MainGame.GlobalGraphicsDevice.Clear(Color.CornflowerBlue);
            
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            
            for(int x = 0; x < tx; x++)
                for(int y = 0; y < ty; y++)
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(16 * 2, 0, 16, 16), new Color(100, 100, 100));

            //MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), new Rectangle((MainGame.GlobalGraphicsDevice.Viewport.Width / 2) - WidgetsMap.Minec.RegionWidth, 32, 155 , 44), WidgetsMap.Minec.ToRectangle(), Color.White);
            //MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), new Rectangle((MainGame.GlobalGraphicsDevice.Viewport.Width / 2), 31, 119, 44), WidgetsMap.raft.ToRectangle(), Color.White);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), 
                new Rectangle((int)((MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferWidth / 2 - ((MainGame.CustomContentManager.GetTexture("minecraft-logo").Width * scale) / 2))), 40, 
                (int)(MainGame.CustomContentManager.GetTexture("minecraft-logo").Width * scale), 
                (int)(MainGame.CustomContentManager.GetTexture("minecraft-logo").Height * scale)), Color.White);

            foreach (var c in ControlsList)
                c.Draw(gameTime);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32), Color.White);

            GraphicsHelper.DrawText("Minecraft 2D Alpha " + MainGame.GameVersion.ToString(), new Vector2(2, MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight - 18), Color.DarkGray);

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
