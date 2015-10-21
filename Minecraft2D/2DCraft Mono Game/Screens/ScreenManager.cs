using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D
{
    public enum GameScreens
    {
        MAIN, SPLASH, CREDITS, GAME
    }

    public class ScreenManager
    {
        public GameScreens CurrentScreen { get; set; }

        private Minecraft2D.Screens.MainGameScreen mainGameScreen;

        public ScreenManager()
        {
            mainGameScreen = new MainGameScreen();
            MainGame.CustomContentManager = new Graphics.ContentManager();
            CurrentScreen = GameScreens.SPLASH;
            LoadContent();
        }
        public void LoadContent()
        {
            if (MainGame.GlobalContentManager != null)
            {
                MainGame.CustomContentManager.AddTexture("default", MainGame.GlobalContentManager.Load<Texture2D>("default"));
                MainGame.CustomContentManager.AddSpriteFont("minecraft", MainGame.GlobalContentManager.Load<SpriteFont>("minecraft"));
                MainGame.CustomContentManager.AddTexture("terrain", MainGame.GlobalContentManager.Load<Texture2D>("terrain"));
                MainGame.CustomContentManager.AddTexture("crosshair", MainGame.GlobalContentManager.Load<Texture2D>("crosshair"));
                MainGame.CustomContentManager.AddTexture("smoothlight", MainGame.GlobalContentManager.Load<Texture2D>("smoothlight"));
            }
        }

        public void RecalculateMinMax()
        {
            if (mainGameScreen != null)
                mainGameScreen.RecalculateMinMax();
        }

        public void PushScreen(GameScreens screen)
        {
            CurrentScreen = screen;
        }

        public void Dispose()
        {
            mainGameScreen = null;
        }

        public void Update(GameTime gameTime)
        {
            switch(CurrentScreen)
            {
                case (GameScreens.SPLASH):
                    mainGameScreen.Update(gameTime);
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            switch (CurrentScreen)
            {
                case GameScreens.SPLASH:
                    mainGameScreen.Draw(gameTime);
                    break;
            }
        }
    }
}
