using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2D.Screens
{
    class SplashIntroScreen : Screen
    {
        private Texture2D mojanglogo, 
            selflogo;


        private int msCount = 0;
        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            MainGame.manager.PushScreen(GameScreens.MAIN);
#endif

            if (mojanglogo == null && selflogo == null)
            {
                mojanglogo = MainGame.CustomContentManager.GetTexture("mojang-logo");
                selflogo = MainGame.CustomContentManager.GetTexture("mikesantiago-logo");
            }

            MainGame.GlobalGraphicsDevice.Clear(Color.White);
            MainGame.GlobalSpriteBatch.Begin();

            if(msCount < 100)
            {
                MainGame.GlobalSpriteBatch.Draw(selflogo, new Rectangle((MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferWidth / 2) - (selflogo.Width / 2),
                    (MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 2) - (selflogo.Height / 2), selflogo.Width, selflogo.Height), Color.White);
            }
            else if(msCount > 100 && msCount < 200)
            {
                MainGame.GlobalSpriteBatch.Draw(mojanglogo, new Rectangle((MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferWidth / 2) - (mojanglogo.Width / 2),
                    (MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 2) - (mojanglogo.Height / 2), mojanglogo.Width, mojanglogo.Height), Color.White);
            }
            else if(msCount > 200)
            {
                MainGame.manager.PushScreen(GameScreens.MAIN);
            }

            MainGame.GlobalSpriteBatch.End();

            msCount++;

        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
