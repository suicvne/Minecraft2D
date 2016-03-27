using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minecraft2DRebirth.Graphics;
using Minecraft2DRebirth.Input;
using Minecraft2DRebirth.Screens;
using System;

namespace Minecraft2DRebirth
{
    public class Minecraft2D : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        #region My shit
        public static Graphics.Graphics graphics;
        public static InputHelper inputHelper;
        #endregion

        public Minecraft2D()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        //This is called first
        protected override void Initialize()
        {
            base.Initialize();

            Console.WriteLine("--Graphics Information--");
            Console.WriteLine($"Backbuffer: {_graphics.PreferredBackBufferWidth} x {_graphics.PreferredBackBufferHeight}");
        }

        //This is called second
        protected override void LoadContent()
        {
            Console.WriteLine("Load Content");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics = new Graphics.Graphics(_spriteBatch, this.Content, _graphics);
            graphics.LoadContent();

            inputHelper = new InputHelper(this); //this will not copy. my C++ instincts are kicking in ;)
#if DEBUG
            graphics.DebugModeStuff();
#endif
        }

        protected override void UnloadContent() => graphics.UnloadContent();

        protected override void Update(GameTime gameTime)
        {
            //For now
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
            inputHelper.Update();
            graphics.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            graphics.Draw();
        }
    }
}
