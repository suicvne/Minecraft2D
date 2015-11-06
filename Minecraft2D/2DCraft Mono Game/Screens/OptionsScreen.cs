using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minecraft2D.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2D.Screens
{
    public class OptionsScreen : Screen
    {
        List<Button> ButtonList = new List<Button>();
        List<TextBox> TextBoxList = new List<TextBox>();

        public OptionsScreen()
        {
            Button donebutton = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth),
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 80),
                new Rectangle(0, 0, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Done");

            donebutton.Clicked += () => MainGame.manager.PushScreen(GameScreens.MAIN);

            TextBox testBox = new TextBox(new Rectangle(20, 300, 32 * 7, 32), true);
            testBox.Content = MainGame.GameOptions.Username;
            testBox.MouseClicked += () => 
            {
                foreach (var tb in TextBoxList)
                    if (tb != testBox)
                        tb.HasFocus = false;
            };

            TextBox testBox2 = new TextBox(new Rectangle(20, 200, 32 * 7, 32), true);
            testBox2.Content = MainGame.GameOptions.WindowState.ToString();
            testBox2.MouseClicked += () =>
            {
                foreach (var tb in TextBoxList)
                    if (tb != testBox2)
                        tb.HasFocus = false;
            };

            ButtonList.Add(donebutton);
            TextBoxList.Add(testBox);
            TextBoxList.Add(testBox2);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in ButtonList)
                button.Update(gameTime);

            foreach (var textb in TextBoxList)
                textb.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int tx, ty;
            tx = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Width / 32);
            ty = (int)Math.Floor((double)MainGame.GlobalGraphicsDevice.Viewport.Height / 32);

            MainGame.GlobalGraphicsDevice.Clear(Color.CornflowerBlue);

            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            for (int x = 0; x < tx; x++)
                for (int y = 0; y < ty; y++)
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(16 * 2, 0, 16, 16), Color.Gray);

            foreach (var button in ButtonList)
                button.Draw(gameTime);
            foreach (var textb in TextBoxList)
                textb.Draw(gameTime);

            TitleScreen.DrawText("Work in progress!", new Vector2(MainGame.GlobalGraphicsDevice.Viewport.Width - (("Work in progress!".Length * 14) * 2), 90), Color.Gray);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32), Color.White);

            MainGame.GlobalSpriteBatch.End();
        }
    }
}
