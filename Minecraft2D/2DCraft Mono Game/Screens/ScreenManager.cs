using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Screens
{
    public enum GameScreens
    {
        MAIN, SPLASH, CREDITS, GAME
    }

    public class ScreenManager
    {
        public GameScreens CurrentScreen { get; set; }

        private Minecraft2D.Screens.MainGameScreen mainGameScreen;
        private TitleScreen titleScreen;

        public ScreenManager()
        {
            mainGameScreen = new MainGameScreen();
            titleScreen = new TitleScreen();
            MainGame.CustomContentManager = new Graphics.ContentManager();
            CurrentScreen = GameScreens.MAIN;
            LoadContent();
        }
        public void LoadContent()
        {
            if (MainGame.GlobalContentManager != null)
            {
                MainGame.CustomContentManager.AddTexture("default", MainGame.GlobalContentManager.Load<Texture2D>("default"));
                MainGame.CustomContentManager.AddSpriteFont("minecraft", MainGame.GlobalContentManager.Load<SpriteFont>("minecraft-fnt"));
                MainGame.CustomContentManager.AddTexture("terrain", MainGame.GlobalContentManager.Load<Texture2D>("terrain"));
                MainGame.CustomContentManager.AddTexture("crosshair", MainGame.GlobalContentManager.Load<Texture2D>("crosshair"));
                MainGame.CustomContentManager.AddTexture("smoothlight", MainGame.GlobalContentManager.Load<Texture2D>("smoothlight"));
                MainGame.CustomContentManager.AddTexture("minecraft-logo", MainGame.GlobalContentManager.Load<Texture2D>("minecraft"));
                MainGame.CustomContentManager.AddTexture("mojang-logo", MainGame.GlobalContentManager.Load<Texture2D>("mojang"));
                MainGame.CustomContentManager.AddTexture("widgets", MainGame.GlobalContentManager.Load<Texture2D>("widgets"));
            }
        }

        public void RecalculateMinMax()
        {
            if(CurrentScreen == GameScreens.GAME)
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
                case (GameScreens.GAME):
                    mainGameScreen.Update(gameTime);
                    break;
                case (GameScreens.MAIN):
                    titleScreen.Update(gameTime);
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            switch (CurrentScreen)
            {
                case GameScreens.GAME:
                    mainGameScreen.Draw(gameTime);
                    break;
                case (GameScreens.MAIN):
                    titleScreen.Draw(gameTime);
                    break;
            }
        }
    }
}
