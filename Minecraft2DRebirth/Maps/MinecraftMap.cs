using Microsoft.Xna.Framework;
using Minecraft2DRebirth;
using Minecraft2DRebirth.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Maps
{
    // TODO: seperate background and foreground draw calls.
    public class MinecraftMap : IMap
    {
        public MapMetadata Metadata { get; set; }

        /// <summary>
        /// The tiles contained in the background layer of the map.
        /// </summary>
        public ITile[,] BackgroundTileMap { get; set; }

        /// <summary>
        /// The tiles contained in the foreground layer of the map.
        /// </summary>
        public ITile[,] TileMap { get; set; }

        public MinecraftMap()
        {
            Metadata = new MapMetadata
            {
                MapName = "World 1",
                Width = 256,
                Height = 256, //256 x 256 cube
            };
            TileMap = new ITile[Metadata.Height, Metadata.Width];
        }

        public void GenerateTestMap()
        {
            for(int y = 0; y < Metadata.Width; y++)
            {
                for(int x = 0; x < Metadata.Height; x++)
                {
                    if(y > 32)
                        TileMap[y, x] = new StoneBlock { Position = new Vector2(x * Constants.TileSize, y * Constants.TileSize) };
                }
            }
        }

        /// <summary>
        /// size 4 array, meant for tile index not abs X positions
        /// 0: minX
        /// 1: maxX
        /// 2: minY
        /// 3: maxY
        /// </summary>
        /// <returns>
        /// </returns>
        private int[] GetMaxRenderPoints(Graphics.Graphics graphics, Camera2D camera)
        {
            int minX = (int)Math.Ceiling(((float)camera.Position.X - ((Metadata.Width * Constants.TileSize) / 2)) / Constants.TileSize);
            int maxX = (int)Math.Ceiling(((float)camera.Position.X + ((Metadata.Width * Constants.TileSize) / 2)) / Constants.TileSize);
            int minY = (int)Math.Ceiling(((float)camera.Position.Y - ((Metadata.Height * Constants.TileSize) / 2)) / Constants.TileSize);
            int maxY = (int)Math.Ceiling(((float)camera.Position.Y + ((Metadata.Height * Constants.TileSize) / 2)) / Constants.TileSize);

            return new int[4] { Zeroize(minX), Zeroize(maxX), Zeroize(minY), Zeroize(maxY) }; ;
        }

        private int Zeroize(int number)
        {
            if (number < 0)
                return 0;
            return Math.Abs(number);
        }

        public void Draw(Graphics.Graphics graphics, Camera2D camera = null)
        {
            int tx = 0, ty = 0;
            int wBounds = Metadata.Width, hBounds = Metadata.Height;
            if(camera != null) //if given a full camera, let's only render a viewport so we don't use as many resources!
            {
                var renderPoints = GetMaxRenderPoints(graphics, camera);
                tx = renderPoints[0] - 1;
                wBounds = renderPoints[1] + 1;
                ty = renderPoints[2] - 1;
                hBounds = renderPoints[3] + 1;
            }

            for(int y = ty; y < hBounds; y++)
            {
                for(int x = tx; x < wBounds; x++)
                {
                    if (y < 0 || y > Metadata.Height - 1)
                        continue;
                    if (x < 0 || x > Metadata.Width - 1)
                        continue;

                    if (TileMap[y, x] != null)
                        TileMap[y, x].Draw(graphics);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int y = 0; y < Metadata.Width; y++)
            {
                for (int x = 0; x < Metadata.Height; x++)
                {
                    if (TileMap[y, x] != null)
                        TileMap[y, x].Update(gameTime);
                }
            }
        }

    }
}
