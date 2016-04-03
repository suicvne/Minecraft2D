using Microsoft.Xna.Framework;
using Minecraft2DRebirth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Maps
{
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
                    //if(y > 32)
                        TileMap[y, x] = new StoneBlock { Position = new Vector2(x * Constants.TileSize, y * Constants.TileSize) };
                }
            }
        }

        public void Draw(Graphics.Graphics graphics)
        {
            for(int y = 0; y < Metadata.Width; y++)
            {
                for(int x = 0; x < Metadata.Height; x++)
                {
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
