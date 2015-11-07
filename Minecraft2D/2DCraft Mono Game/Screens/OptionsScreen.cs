using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minecraft2D.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Controls;
using System.Threading;

namespace Minecraft2D.Screens
{
    public class OptionsScreen : Screen
    {
        public OptionsScreen()
        {
            Button donebutton = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width / 2 - (WidgetsMap.EnabledButton.RegionWidth),
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 50),
                new Rectangle(0, 0, WidgetsMap.EnabledButton.RegionWidth * 2, WidgetsMap.EnabledButton.RegionHeight * 2), "Done");

            donebutton.Clicked += () =>
            {
                Control f = ControlsList.Find(x => x.Name == "usrnametb");
                if(f != null)
                {
                    MainGame.GameOptions.Username = ((TextBox)f).Content;
                }
                MainGame.manager.PushScreen(GameScreens.MAIN);
            };

            #region Key mapping
            Button MoveLeftMod = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width - 580,
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 440),
                new Rectangle(0, 0, (WidgetsMap.EnabledButton.RegionWidth * 2) - 256, WidgetsMap.EnabledButton.RegionHeight * 2), MainGame.GameOptions.MoveLeft.ToString());
            MoveLeftMod.Clicked += () =>
            {
                new Thread(()=>
                {
                    while (MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys().Length == 0)
                    {
                        MoveLeftMod.ButtonText = "<waiting>";
                    }
                    if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                        MainGame.GameOptions.MoveLeft = Microsoft.Xna.Framework.Input.Keys.None;
                    else
                    {
                        MainGame.GameOptions.MoveLeft = MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys()[0];
                    }
                    MoveLeftMod.ButtonText = MainGame.GameOptions.MoveLeft.ToString();
                }).Start();
            };

            Button MoveRightMod = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width - 380,
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 440),
                new Rectangle(0, 0, (WidgetsMap.EnabledButton.RegionWidth * 2) - 256, WidgetsMap.EnabledButton.RegionHeight * 2), MainGame.GameOptions.MoveRight.ToString());
            MoveRightMod.Clicked += () =>
            {
                new Thread(() =>
                {
                    while (MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys().Length == 0)
                    {
                        MoveRightMod.ButtonText = "<waiting>";
                    }
                    if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                        MainGame.GameOptions.MoveRight = Microsoft.Xna.Framework.Input.Keys.None;
                    else
                    {
                        MainGame.GameOptions.MoveRight = MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys()[0];
                    }
                    MoveRightMod.ButtonText = MainGame.GameOptions.MoveRight.ToString();
                }).Start();
            };

            Button MoveDownMod = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width - 580,
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 370),
                new Rectangle(0, 0, (WidgetsMap.EnabledButton.RegionWidth * 2) - 256, WidgetsMap.EnabledButton.RegionHeight * 2), MainGame.GameOptions.MoveDown.ToString());
            MoveDownMod.Clicked += () =>
            {
                new Thread(() =>
                {
                    while (MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys().Length == 0)
                    {
                        MoveDownMod.ButtonText = "<waiting>";
                    }
                    if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                        MainGame.GameOptions.MoveDown = Microsoft.Xna.Framework.Input.Keys.None;
                    else
                    {
                        MainGame.GameOptions.MoveDown = MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys()[0];
                    }
                    MoveDownMod.ButtonText = MainGame.GameOptions.MoveDown.ToString();
                }).Start();
            };

            Button MoveUpMod = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width - 380,
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 370),
                new Rectangle(0, 0, (WidgetsMap.EnabledButton.RegionWidth * 2) - 256, WidgetsMap.EnabledButton.RegionHeight * 2), MainGame.GameOptions.MoveUp.ToString());
            MoveUpMod.Clicked += () =>
            {
                new Thread(() =>
                {
                    while (MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys().Length == 0)
                    {
                        MoveUpMod.ButtonText = "<waiting>";
                    }
                    if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                        MainGame.GameOptions.MoveUp = Microsoft.Xna.Framework.Input.Keys.None;
                    else
                    {
                        MainGame.GameOptions.MoveUp = MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys()[0];
                    }
                    MoveUpMod.ButtonText = MainGame.GameOptions.MoveUp.ToString();
                }).Start();
            };

            Button JumpMod = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width - 470,
                    MainGame.GlobalGraphicsDevice.Viewport.Height - 300),
                new Rectangle(0, 0, (WidgetsMap.EnabledButton.RegionWidth * 2) - 256, WidgetsMap.EnabledButton.RegionHeight * 2), MainGame.GameOptions.JumpKey.ToString());
            JumpMod.Clicked += () =>
            {
                new Thread(() =>
                {
                    while (MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys().Length == 0)
                    {
                        JumpMod.ButtonText = "<waiting>";
                    }
                    if (MainGame.GlobalInputHelper.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                        MainGame.GameOptions.JumpKey = Microsoft.Xna.Framework.Input.Keys.None;
                    else
                    {
                        MainGame.GameOptions.JumpKey = MainGame.GlobalInputHelper.CurrentKeyboardState.GetPressedKeys()[0];
                    }
                    JumpMod.ButtonText = MainGame.GameOptions.JumpKey.ToString();
                }).Start();
            };
            #endregion

            TextBox usernameTextBox = new TextBox(new Rectangle(MainGame.GlobalGraphicsDevice.Viewport.Width - 580, MainGame.GlobalGraphicsDevice.Viewport.Height - 230, 32 * 11, 32), true);
            usernameTextBox.Content = MainGame.GameOptions.Username;
            usernameTextBox.Name = "usrnametb";

            Button FullscreenButton = new Button(new Vector2i(MainGame.GlobalGraphicsDevice.Viewport.Width - 470,
                    MainGame.GlobalGraphicsDevice.Viewport.Height - (230 - 70)),
                new Rectangle(0, 0, (WidgetsMap.EnabledButton.RegionWidth * 2) - 256, WidgetsMap.EnabledButton.RegionHeight * 2), MainGame.GameOptions.Fullscreen.ToString());
            FullscreenButton.Clicked += () =>
            {
                MainGame.GlobalGraphicsDeviceManager.IsFullScreen = !MainGame.GlobalGraphicsDeviceManager.IsFullScreen;
                MainGame.GlobalGraphicsDeviceManager.ApplyChanges();
                MainGame.GameOptions.Fullscreen = MainGame.GlobalGraphicsDeviceManager.IsFullScreen;

                FullscreenButton.ButtonText = MainGame.GameOptions.Fullscreen.ToString();
            };

            AddControl(donebutton);
            AddButtonWithLabel(MoveLeftMod, new Label(name: "lbl", text: "Move Left", pos: Vector2.Zero, tnt: Color.Gray));
            AddButtonWithLabel(MoveRightMod, new Label(name: "lbl", text: "Move Right", pos: Vector2.Zero, tnt: Color.Gray));
            AddButtonWithLabel(MoveUpMod, new Label(name: "lbl", text: "Move Up", pos: Vector2.Zero, tnt: Color.Gray));
            AddButtonWithLabel(MoveDownMod, new Label(name: "lbl", text: "Move Down", pos: Vector2.Zero, tnt: Color.Gray));
            AddButtonWithLabel(JumpMod, new Label(name: "lbl", text: "Jump", pos: Vector2.Zero, tnt: Color.Gray));
            AddButtonWithLabel(FullscreenButton, new Label(name: "lbl", text: "Fullscreen", pos: Vector2.Zero, tnt: Color.Gray));
            AddTextBoxWithLabel(usernameTextBox, new Label(name: "lbl", text: "Username", pos: Vector2.Zero, tnt: Color.Gray));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var control in ControlsList)
                control.Update(gameTime);
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
                    MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"), new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(16 * 2, 0, 16, 16), new Color(100, 100, 100));

            foreach (var control in ControlsList)
                control.Draw(gameTime);

            //TitleScreen.DrawText("Work in progress!", new Vector2(MainGame.GlobalGraphicsDevice.Viewport.Width - (("Work in progress!".Length * 14) * 2), 90), Color.Gray);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(MainGame.GlobalInputHelper.CurrentMouseState.X, MainGame.GlobalInputHelper.CurrentMouseState.Y, 32, 32), Color.White);

            MainGame.GlobalSpriteBatch.End();
        }
    }
}
