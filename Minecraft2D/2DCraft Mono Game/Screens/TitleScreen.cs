using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minecraft2D.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Minecraft2D.Screens
{
    public class TitleScreen : Screen
    {
        List<Button> ButtonList = new List<Button>();
        SoundEffectInstance sei;

        public TitleScreen()
        {
            Button ExitButton = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDevice.Viewport.Height - 120), new Rectangle(0, 0, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Exit");
            Button PlayButton = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth), MainGame.GlobalGraphicsDevice.Viewport.Height - 260), new Rectangle(0, 0, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Load Test World");
            

            ExitButton.Clicked += ExitButton_Clicked;
            PlayButton.Clicked += () => 
            {
                
                MainGame.manager.PushScreen(GameScreens.GAME);
            };

            ButtonList.Add(ExitButton);
            ButtonList.Add(PlayButton);
        }

        private void ExitButton_Clicked()
        {
            MainGame.WriteSettings();
            MainGame.GameExiting = true;
        }

        public override void Draw(GameTime gameTime)
        {
            int tx, ty;
            tx = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Width / 32);
            ty = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Height / 32);

            MainGame.GlobalGraphicsDevice.Clear(Color.CornflowerBlue);
            
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            
            for(int x = 0; x < tx; x++)
                for(int y = 0; y < ty; y++)
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(16 * 2, 0, 16, 16), Color.Gray);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), new Rectangle((MainGame.GlobalGraphicsDevice.Viewport.Width / 2) - WidgetsMap.Minec.RegionWidth, 32, 155 , 44), WidgetsMap.Minec.ToRectangle(), Color.White);
            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("minecraft-logo"), new Rectangle((MainGame.GlobalGraphicsDevice.Viewport.Width / 2), 31, 119, 44), WidgetsMap.raft.ToRectangle(), Color.White);

            foreach (var button in ButtonList)
                button.Draw(gameTime);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32), Color.White);

            MainGame.GlobalSpriteBatch.DrawString(MainGame.CustomContentManager.GetFont("minecraft"),
                "Minecraft 2D Alpha", new Vector2(2, MainGame.GlobalGraphicsDevice.Viewport.Height - 18), Color.DarkGray);

            MainGame.GlobalSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var button in ButtonList)
                button.Update(gameTime);
        }
    }
}
