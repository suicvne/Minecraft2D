using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minecraft2DRebirth.Graphics;
using Minecraft2DRebirth.Maps;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Entity;
using Microsoft.Xna.Framework.Input;

namespace Minecraft2DRebirth.Scenes
{
    // TODO abstract entities list into a double array restricted to the bounds of the map
    //  might be okay at this point to just make a special map inheriting scene and stuff
    // TODO abstract entities list into the map class.
    public class BasicLightableSceneWithMap : BasicLightableScene
    {
        private MinecraftMap _Map;
        public IMap Map { get { return _Map; } set { _Map = (MinecraftMap)value; } }

        public BasicLightableSceneWithMap(Graphics.Graphics graphics) : base(graphics)
        {
            Map = new MinecraftMap();
            _Map.GenerateTestMap();
            Camera = new Camera2D();
            int x = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Width / 2;
            int y = graphics.GetGraphicsDeviceManager().GraphicsDevice.Viewport.Height / 2;
            //minX = x;
            //minY = y;
            //maxX = (int)world.WorldSize.X - (MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferWidth / 2);
            //maxY = (int)world.WorldSize.Y - (MainGame.GlobalGraphicsDeviceManager.PreferredBackBufferHeight / 2);
            Camera.Position = new Vector2(x + (32 * 32), y + (25 * 32));
        }

        public void DrawBaseScene(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(BaseScene);
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone,
                transformMatrix: Camera.Transformation(graphics.GetGraphicsDeviceManager().GraphicsDevice));

            
            Map.Draw(graphics, Camera);
            //Draws the regular entites and their sprites and whatnot.
            ((List<IAnimatedEntity>)Entities).ForEach(entity =>
            {
                entity.Draw(graphics);
            });

            graphics.GetSpriteBatch().End();

            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }

        public void DrawLights(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(LightScene);
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(AmbientLight);
            graphics.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, transformMatrix: Camera.Transformation(graphics.GetGraphicsDeviceManager().GraphicsDevice));

            var texture = graphics.GetTexture2DByName("circle");

            ((List<LightSource>)StaticLights).ForEach(light =>
            {
                graphics.GetSpriteBatch().Draw(
                    texture, light.Size, light.Color
                );
            });

            //The dynamic lights
            foreach (var entity in Entities.Where(x => x is IDynamicLightEntity))
            {
                if (entity is IDynamicLightEntity)
                {
                    var entityAsLight = (IDynamicLightEntity)entity;
                    var drawingPoint = entityAsLight.Position.ToRectangle();
                    drawingPoint.Width = texture.Width;
                    drawingPoint.Height = texture.Height;
                    if (entityAsLight.LightSize != 1.0f)
                    {
                        drawingPoint.Width = (int)(drawingPoint.Width * entityAsLight.LightSize);
                        drawingPoint.Height = (int)(drawingPoint.Height * entityAsLight.LightSize);
                    }

                    if (entityAsLight.LightOffset != null)
                    {
                        drawingPoint.X += (int)(entityAsLight.LightOffset.X - (drawingPoint.Width / 2));
                        drawingPoint.Y += (int)(entityAsLight.LightOffset.Y - (drawingPoint.Height / 2));
                    }

                    graphics.GetSpriteBatch().Draw(texture, drawingPoint, entityAsLight.LightColor);
                }
            }

            graphics.GetSpriteBatch().End();

            graphics.GetGraphicsDeviceManager().GraphicsDevice.SetRenderTarget(null);
        }

        private void UpdateCameraBounds(Rectangle screenRect, IAnimatedEntity player)
        {
            if (player.Position.X > -1)
            {
                var newPosition = Camera.Position;
                newPosition.X = (int)Math.Min(
                    Math.Max(
                        player.Position.X + (player.SpriteSize.X ),
                        0 + (screenRect.Width / 2) //half the viewport
                        // TODO: correct this to account for scaling vs resolution change
                    ),
                    (Map.Metadata.Width * Constants.TileSize) - ((Map.Metadata.Width * Constants.TileSize) / 2)
                );
                Camera.Position = newPosition;
            }
        }

        private Rectangle ScreenRect = new Rectangle(0, 0, 0, 0);
        public new void Draw(Graphics.Graphics graphics)
        {
            DrawBaseScene(graphics);
            DrawLights(graphics);

            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.White);
            graphics.GetSpriteBatch().Begin(blendState: Lighted.Multiply, samplerState: SamplerState.PointClamp);

            var screenRect = graphics.ScreenRectangle();
            graphics.GetSpriteBatch().Draw(BaseScene, screenRect, Color.White);
            ScreenRect = screenRect;

            if (RenderLights)
                graphics.GetSpriteBatch().Draw(LightScene, screenRect, Color.White);

            graphics.GetSpriteBatch().End();
        }

        public new void Update(GameTime gameTime)
        {
            ((List<IAnimatedEntity>)Entities).ForEach(entity =>
            {
                entity.Update(gameTime);
                if (entity is AnimatedEntityTest)
                    UpdateCameraBounds(ScreenRect, entity);
            });
            
            // Static lights don't change. If dynamic/entity lights want to change/update, they can.

            /* This was for debugging positions and whatnot.
            if (Minecraft2D.InputHelper.IsCurPress(Keys.Right))
                Camera.Move(Vector2.UnitX);
            if (Minecraft2D.InputHelper.IsCurPress(Keys.Left))
                Camera.Move(-Vector2.UnitX);
            if (Minecraft2D.InputHelper.IsCurPress(Keys.Up))
                Camera.Move(-Vector2.UnitY);
            if (Minecraft2D.InputHelper.IsCurPress(Keys.Down))
                Camera.Move(Vector2.UnitY);

            if (Minecraft2D.InputHelper.IsCurPress(Keys.A))
                offx -= 1;
            if (Minecraft2D.InputHelper.IsCurPress(Keys.D))
                offx += 1;
            if (Minecraft2D.InputHelper.IsCurPress(Keys.S))
                offy += 1;
            if (Minecraft2D.InputHelper.IsCurPress(Keys.W))
                offy -= 1;

            if (Minecraft2D.InputHelper.IsCurPress(Keys.R))
                Camera.Zoom = 1f;

            if (Minecraft2D.InputHelper.IsCurPress(Keys.OemOpenBrackets))
                Camera.Zoom += 0.1f;
            if (Minecraft2D.InputHelper.IsCurPress(Keys.OemCloseBrackets))
                Camera.Zoom -= 0.1f;
                */
        }
    }
}
