using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Minecraft2D.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minecraft2D.Map;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        int currentTileIndex = 0;
        private BlockTemplate PlacingTile { get; set; }
        private BlockTemplate[] TilesList { get; set; }

        public MainGameScreen()
        {
            world = new World();

            MainGame.WindowClosing += () =>
            {
                world.SaveWorld("World1.wld");
                MainGame.WriteSettings();
            };

            worldRenderTarget = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            worldLightmapPass = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            allTogether = new RenderTarget2D(MainGame.GlobalGraphicsDevice, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height, false, MainGame.GlobalGraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            TilesList = PresetBlocks.BlocksAsArray();
            PlacingTile = TilesList[currentTileIndex];
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
                MainGame.GameCamera.Pos = new Vector2i(maxX, MainGame.GameCamera.Pos.Y);
            }
            if (MainGame.GameCamera.Pos.Y > maxY)
            {
                MainGame.GameCamera.Pos = new Vector2i(MainGame.GameCamera.Pos.X, maxY);
            }
            if (MainGame.GameCamera.Pos.X < minX)
            {
                MainGame.GameCamera.Pos = new Vector2i(minX, MainGame.GameCamera.Pos.Y);
            }
            if (MainGame.GameCamera.Pos.Y < minY)
            {
                MainGame.GameCamera.Pos = new Vector2i(MainGame.GameCamera.Pos.X, minY);
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
                if (MainGame.GlobalInputHelper.CurrentMouseState.ScrollWheelValue > MainGame.GlobalInputHelper.LastMouseState.ScrollWheelValue)
                {
                    currentTileIndex++;
                    if (currentTileIndex >= TilesList.Length)
                        currentTileIndex = 0;
                    PlacingTile = TilesList[currentTileIndex];
                }
                if (MainGame.GlobalInputHelper.CurrentMouseState.ScrollWheelValue < MainGame.GlobalInputHelper.LastMouseState.ScrollWheelValue)
                {
                    currentTileIndex--;
                    if (currentTileIndex < 0)
                        currentTileIndex = TilesList.Length - 1;
                    PlacingTile = TilesList[currentTileIndex];
                }

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if(MainGame.GameCamera != null)
                    {
                        Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
                        Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix);

                        Tile airP = PresetBlocks.Air.AsTile();
                        airP.Position = new Vector2((float)Math.Floor(worldMousePosition.X / 32),
                            (float)Math.Floor(worldMousePosition.Y / 32));
                        world.SetTile((int)worldMousePosition.X, (int)worldMousePosition.Y, airP);
                    }
                }
                else if(MainGame.GlobalInputHelper.IsNewPress(MouseButtons.RightButton))
                {
                    Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
                    Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix);

                    Tile toPlace = PlacingTile.AsTile();
                    toPlace.Position = new Vector2((int)worldMousePosition.X, (int)worldMousePosition.Y);
                    world.SetTile((int)worldMousePosition.X, (int)worldMousePosition.Y, toPlace);
                }
                else if(MainGame.GlobalInputHelper.CurrentMouseState.MiddleButton == ButtonState.Pressed)
                {
                    Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
                    Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix);

                    Tile toPlace = PlacingTile.AsTile();
                    toPlace.IsBackground = true;
                    toPlace.Position = new Vector2((int)worldMousePosition.X, (int)worldMousePosition.Y);
                    world.SetTile((int)worldMousePosition.X, (int)worldMousePosition.Y, toPlace);
                }
                if (MainGame.GlobalInputHelper.IsNewPress(Keys.F3))
                {
                    MainGame.GameOptions.ShowDebugInformation = !MainGame.GameOptions.ShowDebugInformation;
                }
                if(MainGame.GlobalInputHelper.IsNewPress(Keys.Escape))
                {
                    world.SaveWorld("World1.wld");
                    MainGame.manager.PushScreen(GameScreens.MAIN);
                }
                if(MainGame.GlobalInputHelper.IsCurPress(Keys.Left))
                {
                    world.player.Move(new Vector2(-2, 0));
                }
                if (MainGame.GlobalInputHelper.IsCurPress(Keys.Right))
                {
                    world.player.Move(new Vector2(2, 0));
                }
                if (Keyboard.GetState().IsKeyDown(MainGame.GameOptions.MoveUp))
                {
                    MainGame.GameCamera.Move(new Vector2i(0, -5));
                    if (MainGame.GameCamera.Pos.Y < minY)
                        MainGame.GameCamera.Pos = new Vector2i(MainGame.GameCamera.Pos.X, minY);
                }
                if (Keyboard.GetState().IsKeyDown(MainGame.GameOptions.MoveRight))
                {
                    MainGame.GameCamera.Move(new Vector2i(5, 0));
                    if (MainGame.GameCamera.Pos.X > maxX)
                        MainGame.GameCamera.Pos = new Vector2i(maxX, MainGame.GameCamera.Pos.Y);
                }
                if (Keyboard.GetState().IsKeyDown(MainGame.GameOptions.MoveLeft))
                {
                    MainGame.GameCamera.Move(new Vector2i(-5, 0));
                    if (MainGame.GameCamera.Pos.X < minX)
                        MainGame.GameCamera.Pos = new Vector2i(minX, MainGame.GameCamera.Pos.Y);
                }
                if (Keyboard.GetState().IsKeyDown(MainGame.GameOptions.MoveDown))
                {
                    MainGame.GameCamera.Move(new Vector2i(0, 5));
                    if (MainGame.GameCamera.Pos.Y > maxY)
                        MainGame.GameCamera.Pos = new Vector2i(MainGame.GameCamera.Pos.X, maxY);
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

        #region Drawing Code
        private void DrawWorldToTexture(GameTime gameTime)
        {
            MainGame.GlobalGraphicsDevice.SetRenderTarget(worldRenderTarget);

            MainGame.GlobalGraphicsDevice.Clear(world.SkyColor);

            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));

            world.Draw(gameTime);

            Matrix inverseViewMatrix = Matrix.Invert(MainGame.GameCamera.get_transformation(MainGame.GlobalGraphicsDevice));
            Vector2i worldMousePosition = new Vector2i(Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseViewMatrix));

            if (world.GetTile((int)worldMousePosition.X, (int)worldMousePosition.Y) != null)
            {
                Tile t = world.GetTile((int)worldMousePosition.X, (int)worldMousePosition.Y);
                
                if (t.Type != TileType.Air)
                    DrawRectangle(new Rectangle((int)t.Position.X, (int)t.Position.Y, 32, 32), Color.White);
            }

            MainGame.GlobalSpriteBatch.End();

            MainGame.GlobalGraphicsDevice.SetRenderTarget(null);
        }

        private void DrawLightmapToTexture(GameTime gameTime)
        {
            MainGame.GlobalGraphicsDevice.SetRenderTarget(worldLightmapPass);

            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture,
                BlendState.Additive,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));

            world.DrawLightmap(gameTime);

            MainGame.GlobalSpriteBatch.End();

            MainGame.GlobalGraphicsDevice.SetRenderTarget(null);
        }
        
        public override void Draw(GameTime gameTime)
        {
            framecounter++;

            #region Misc. Variable Initialization, done once on game startup
            if (MainGame.GameCamera == null)
            {
                MainGame.GameCamera = new Graphics.Camera2D();
                int x = MainGame.GlobalGraphicsDevice.Viewport.Width / 2;
                int y = MainGame.GlobalGraphicsDevice.Viewport.Height / 2;
                minX = x;
                minY = y;
                maxX = (int)world.WorldSize.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2);
                maxY = (int)world.WorldSize.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2);
                MainGame.GameCamera.Pos = new Vector2i(x + (32 * 32), y + (25 * 32));
            }
            if(skinTest == null)
                skinTest = new Skin(MainGame.CustomContentManager.GetTexture("default"));
            #endregion

            #region Individual Target2D Rendering
            DrawWorldToTexture(gameTime);
            DrawLightmapToTexture(gameTime);
            #endregion

            #region 2 pass system for lighting. Begin and end are kind of needed here to account for the modified blend state we have
            MainGame.GlobalGraphicsDevice.Clear(Color.White);
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture, MainGame.Multiply, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            MainGame.GlobalSpriteBatch.Draw(worldRenderTarget, new Rectangle(0, 0, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height), Color.White);
            MainGame.GlobalSpriteBatch.Draw(worldLightmapPass, new Rectangle(0, 0, MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height), Color.White);
            MainGame.GlobalSpriteBatch.End();
            #endregion

            #region Text drawing, crosshair drawing
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            TitleScreen.DrawText("Minecraft 2D", new Vector2(0, 2), Color.White);

            MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("crosshair"), new Rectangle(mouseState.X, mouseState.Y, 32, 32), Color.White);

            MainGame.GlobalSpriteBatch.Draw
                (
                    MainGame.CustomContentManager.GetTexture("widgets"), 
                    new Rectangle(2,
                        MainGame.GlobalGraphicsDevice.Viewport.Height - WidgetsMap.SingleInventory.RegionHeight * 2 - 2, 
                        24 * 2, 
                        23 * 2),
                    WidgetsMap.SingleInventory.ToRectangle(), Color.White
                );

            MainGame.GlobalSpriteBatch.Draw
                (
                    MainGame.CustomContentManager.GetTexture("terrain"),
                    new Rectangle(8,
                    (MainGame.GlobalGraphicsDevice.Viewport.Height - WidgetsMap.SingleInventory.RegionHeight * 2 - 2) + 6, 
                    32, 
                    32),
                    PlacingTile.TextureRegion.ToRectangle(),
                    Color.White
                );

            /**
            These lines really need to stay together
            */
            DrawDebugText();
            if (!MainGame.GameOptions.ShowDebugInformation)
                MainGame.GlobalSpriteBatch.End();
            /**
            */
            #endregion

            elapsedMs++;
        }

        private void DrawDebugText()
        {
            if (MainGame.GameOptions.ShowDebugInformation)
            {

                TitleScreen.DrawText("Cam X: " + MainGame.GameCamera.Pos.X, new Vector2(0, 18), Color.White);
                TitleScreen.DrawText("Cam Y: " + MainGame.GameCamera.Pos.Y, new Vector2(0, 18 * 2), Color.White);
                TitleScreen.DrawText("FPS: " + framerate, new Vector2(0, 18 * 3), Color.White);
                TitleScreen.DrawText("World Time: " + world.WorldTime, new Vector2(0, 18 * 4), Color.White);
                TitleScreen.DrawText("World Size: " + world.WorldSize.X + " x " + world.WorldSize.Y, new Vector2(0, 18 * 5), Color.White);

                string WorldArea = string.Format("{0} x {1}", world.viewportRect.Width, world.viewportRect.Height);
                TitleScreen.DrawText("Rendered Area: " + WorldArea, new Vector2(0, 18 * 6), Color.White);
                TitleScreen.DrawText("Rendered Lights: " + world.RenderedLights, new Vector2(0, 18 * 7), Color.White);
                TitleScreen.DrawText("VSync Enabled: " + MainGame.GameOptions.Vsync, new Vector2(0, 18 * 8), Color.White);

                MainGame.GlobalSpriteBatch.End();


                MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));
                DrawRectangle(new Rectangle((int)world.player.Position.X,
                    (int)world.player.Position.Y,
                    world.player.Hitbox.Width,
                    world.player.Hitbox.Height), Color.Green);
                MainGame.GlobalSpriteBatch.End();
            }
        }

        private void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(MainGame.GlobalGraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            MainGame.GlobalSpriteBatch.Draw(rect, coords, color * .5f);
        }
        #endregion
    }
}
