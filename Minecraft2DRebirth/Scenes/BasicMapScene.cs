using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Entity;
using Minecraft2DRebirth.Graphics;
using Minecraft2DRebirth.Screens;
using Minecraft2DRebirth.Screens.TestScreen;
using Microsoft.Xna.Framework.Graphics;

namespace Minecraft2DRebirth.Scenes
{
    public class BasicMapScene : IScene
    {
        private List<IAnimatedEntity> _Entities;
        public IEnumerable<IAnimatedEntity> Entities
        {
            get
            {
                return _Entities;
            }
            set
            {
                _Entities = (List<IAnimatedEntity>)value;
            }
        }
        public Camera2D Camera { get; set; }
        public IScreen Parent { get; set; }

        private BasicTileMap Map { get; set; }
        private PlayerTest TestPlayer;

        public BasicMapScene()
        {
            Map = BasicTileMap.CreateTestMap();
            Camera = new Camera2D();
            TestPlayer = new PlayerTest();
        }

        private Texture2D _Rectangle;
        public Texture2D GetRectangle(Graphics.Graphics graphics)
        {
            if (_Rectangle == null)
            {
                _Rectangle = new Texture2D(graphics.GetGraphicsDeviceManager().GraphicsDevice, 1, 1);
                _Rectangle.SetData<Color>(new Color[] { Color.White });
            }
            return _Rectangle;
        }

        public void Draw(Graphics.Graphics graphics)
        {
            graphics.GetGraphicsDeviceManager().GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.GetSpriteBatch().Begin(sortMode: Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred,
                /*transformMatrix: Camera.Transformation(graphics.GetGraphicsDeviceManager().GraphicsDevice)
                ,*/ depthStencilState: DepthStencilState.None, samplerState: SamplerState.PointClamp);
            Map.Draw(graphics);

            int tx, ty;
            tx = (int)Math.Floor(Minecraft2D.InputHelper.MousePosition.X / Constants.TileSize);
            ty = (int)Math.Floor(Minecraft2D.InputHelper.MousePosition.Y / Constants.TileSize);
            if(Map.GetTileAtIndex(tx, ty) != null)
                graphics.GetSpriteBatch().Draw(GetRectangle(graphics), new Rectangle(tx * Constants.TileSize, ty * Constants.TileSize, Constants.TileSize, Constants.TileSize), Color.Gray * .32f);
            TestPlayer.Draw(graphics);
            graphics.GetSpriteBatch().End();
        }

        public void Update(GameTime gameTime)
        {
            Map.Update(gameTime);
            TestPlayer.Update(gameTime, Map);

            int tx, ty;
            tx = (int)Math.Floor(Minecraft2D.InputHelper.MousePosition.X / Constants.TileSize);
            ty = (int)Math.Floor(Minecraft2D.InputHelper.MousePosition.Y / Constants.TileSize);
        }
    }
}
