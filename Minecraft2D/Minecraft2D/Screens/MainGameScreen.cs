using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minecraft2D.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minecraft2D.Map;

namespace Minecraft2D.Screens
{
    public class MainGameScreen : Screen
    {
        private Random ran = new Random(DateTime.Now.Millisecond);
        private Skin skinTest;
        private World world;
        private MouseState mouseState = Mouse.GetState();

        private RenderTarget2D worldRenderTarget;
        private RenderTarget2D worldLightmapPass;
        private RenderTarget2D allTogether;

        public MainGameScreen()
        {
            world = new World();

            worldRenderTarget = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            worldLightmapPass = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            allTogether = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
        }

        private int minX, minY, maxX, maxY;

        public void RecalculateMinMax()
        {
            int x = MainGame.GlobalGraphicsDevice.Viewport.Width / 2;
            int y = MainGame.GlobalGraphicsDevice.Viewport.Height / 2;
            minX = x;
            minY = y;
            maxX = (int)world.WorldSize.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2);
            maxY = (int)world.WorldSize.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2);

            if (MainGame.GameCamera.Pos.X > maxX)
            {
                MainGame.GameCamera.Pos = new Vector2(maxX, MainGame.GameCamera.Pos.Y);
            }
            if (MainGame.GameCamera.Pos.Y > maxY)
            {
                MainGame.GameCamera.Pos = new Vector2(MainGame.GameCamera.Pos.X, maxY);
            }
            if (MainGame.GameCamera.Pos.X < minX)
            {
                MainGame.GameCamera.Pos = new Vector2(minX, MainGame.GameCamera.Pos.Y);
            }
            if (MainGame.GameCamera.Pos.Y < minY)
            {
                MainGame.GameCamera.Pos = new Vector2(MainGame.GameCamera.Pos.X, minY);
            }
            worldRenderTarget = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            worldLightmapPass = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            allTogether = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            ElapsedTime += gameTime.ElapsedGameTime;

            if (ElapsedTime > TimeSpan.FromSeconds(1))
            {
                ElapsedTime -= TimeSpan.FromSeconds(1);
                framerate = framecounter;
                framecounter = 0;
            }
            if (!MainGame.GameOptions.UseController)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
                    Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix);

                    Tile airP = PresetBlocks.Air.AsTile();
                    airP.Position = new Vector2((float)Math.Floor(worldMousePosition.X / 32),
                        (float)Math.Floor(worldMousePosition.Y / 32));
                    world.SetTile((int)worldMousePosition.X, (int)worldMousePosition.Y, airP);
                }
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(MainGame.GameOptions.JumpKey))
                {
                    world.SaveWorld("World1.wld");
                    Environment.Exit(0);
                }
                else if(MainGame.GlobalInputHelper.CurrentMouseState.RightButton == ButtonState.Pressed)
                {
                    Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
                    Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix);

                    Tile toPlace = PresetBlocks.Stone.AsTile();
                    world.SetTile((int)worldMousePosition.X, (int)worldMousePosition.Y, toPlace);
                }
                else if(MainGame.GlobalInputHelper.CurrentMouseState.MiddleButton == ButtonState.Pressed)
                {
                    Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
                    Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix);

                    Tile toPlace = PresetBlocks.Stone.AsTile();
                    toPlace.IsBackground = true;
                    world.SetTile((int)worldMousePosition.X, (int)worldMousePosition.Y, toPlace);
                }
                else if (MainGame.GlobalInputHelper.IsNewPress(Keys.F3))
                {
                    MainGame.GameOptions.ShowDebugInformation = !MainGame.GameOptions.ShowDebugInformation;
                }
                else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(MainGame.GameOptions.MoveUp))
                {
                    MainGame.GameCamera.Move(new Vector2(0, -5));
                    if (MainGame.GameCamera.Pos.Y < minY)
                        MainGame.GameCamera.Pos = new Vector2(MainGame.GameCamera.Pos.X, minY);
                }
                else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(MainGame.GameOptions.MoveRight))
                {
                    MainGame.GameCamera.Move(new Vector2(5, 0));
                    if (MainGame.GameCamera.Pos.X > maxX)
                        MainGame.GameCamera.Pos = new Vector2(maxX, MainGame.GameCamera.Pos.Y);
                }
                else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(MainGame.GameOptions.MoveLeft))
                {
                    MainGame.GameCamera.Move(new Vector2(-5, 0));
                    if (MainGame.GameCamera.Pos.X < minX)
                        MainGame.GameCamera.Pos = new Vector2(minX, MainGame.GameCamera.Pos.Y);
                }
                else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(MainGame.GameOptions.MoveDown))
                {
                    MainGame.GameCamera.Move(new Vector2(0, 5));
                    if (MainGame.GameCamera.Pos.Y > maxY)
                        MainGame.GameCamera.Pos = new Vector2(MainGame.GameCamera.Pos.X, maxY);
                }
            }
            world.Update(gameTime);
        }

        private float elapsedMs;
        private Color color = Color.CornflowerBlue;
        #region Framerate Stuffs
        int framerate = 0;
        int framecounter = 0;
        TimeSpan ElapsedTime = TimeSpan.Zero;
        #endregion

        private void DrawWorldToTexture(GameTime gameTime)
        {
            MainGame.GlobalGraphicsDevice.SetRenderTarget(worldRenderTarget);

            MainGame.GlobalGraphicsDevice.Clear(world.SkyColor);

            world.Draw(gameTime);

            Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
            Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix);

            if (world.GetTile((int)worldMousePosition.X, (int)worldMousePosition.Y) != null)
            {
                Tile t = world.GetTile((int)worldMousePosition.X, (int)worldMousePosition.Y);

                //Special
                MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));
                if (t.Type != TileType.Air)
                    DrawRectangle(new Rectangle((int)t.Position.X, (int)t.Position.Y, 32, 32), Color.White);
                MainGame.GlobalSpriteBatch.End();
            }

            MainGame.GlobalGraphicsDevice.SetRenderTarget(null);
        }

        private void DrawLightmapToTexture(GameTime gameTime)
        {
            MainGame.GlobalGraphicsDevice.SetRenderTarget(worldLightmapPass);
            MainGame.GlobalGraphicsDevice.Clear(Color.Black);
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture,
                BlendState.Additive,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));
            for (int x = 0; x < world.Lightmap.GetLength(1); x++)
            {
                for (int y = 0; y < world.Lightmap.GetLength(0); y++)
                {
                    if(world.Lightmap[y, x] == 1f)
                        MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
                            new Rectangle(x * 32 - 64, y * 32 - 64, 32 * 5, 32 * 5), Color.White);
                }
            }
            //MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("smoothlight"),
            //    new Rectangle(0, 0, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height),
            //    Color.White);
            MainGame.GlobalSpriteBatch.End();
            MainGame.GlobalGraphicsDevice.SetRenderTarget(null);
        }


        public override void Draw(GameTime gameTime)
        {
            framecounter++;
            
            if (MainGame.GameCamera == null)
            {
                MainGame.GameCamera = new Camera2D();
                int x = MainGame.GlobalGraphicsDevice.Viewport.Width / 2;
                int y = MainGame.GlobalGraphicsDevice.Viewport.Height / 2;
                minX = x;
                minY = y;
                maxX = (int)world.WorldSize.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2);
                maxY = (int)world.WorldSize.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2);
                MainGame.GameCamera.Pos = new Vector2(x + (32 * 32), y + (25 * 32));
            }
            if(skinTest == null)
                skinTest = new Skin(MainGame.CustomContentManager.GetTexture("default"));

            DrawWorldToTexture(gameTime);
            DrawLightmapToTexture(gameTime);

            MainGame.GlobalGraphicsDevice.Clear(Color.White);
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture, MainGame.Multiply, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            MainGame.GlobalSpriteBatch.Draw(worldRenderTarget, new Rectangle(0, 0, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height), Color.White);
            MainGame.GlobalSpriteBatch.Draw(worldLightmapPass, new Rectangle(0, 0, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height), Color.White);
            MainGame.GlobalSpriteBatch.End();


            //MainGame.GlobalGraphicsDevice.Clear(Color.Black);
            //MainGame.GlobalSpriteBatch.Draw(worldRenderTarget, new Rectangle(0, 0, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height), Color.White);MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(mouseState.X, mouseState.Y, 32, 32), Color.White);
            MainGame.GlobalSpriteBatch.End();

            

            elapsedMs++;
        }

        private void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(MainGame.GlobalGraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            MainGame.GlobalSpriteBatch.Draw(rect, coords, color * .5f);
        }
    }
}
