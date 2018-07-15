using System;
using Microsoft.Xna.Framework;
using RockSolidEngine.Graphics;
using RockSolidEngine.Maps;
using System.Collections.Generic;

namespace RockSolidEngine.Screens.TestScreen
{
    public class BasicTileMap : IMap
    {
        public MapMetadata Metadata { get; set; }

        public ITile[,] TileMap { get; set; }

        public BasicTileMap(int width = 27, int height = 15)
        {
            Metadata = new MapMetadata
            {
                MapName = "Test Map",
                Width = width,
                Height = height
            };
            TileMap = new ITile[height, width];
        }

        public static BasicTileMap CreateTestMap()
        {
            BasicTileMap returnValue = new BasicTileMap();
            //h, w
            for (int y = 0; y < returnValue.Metadata.Height; y++)
                for (int x = 0; x < returnValue.Metadata.Width; x++)
                    if (y > 10)
                        returnValue.TileMap[y, x] = new StoneBlock { Position = new Vector2(x * Constants.TileSize, y * Constants.TileSize) };

            return returnValue;
        }

        private int GameToIndex(int game)
        {
            var returnValue = (int)Math.Floor((double)game / Constants.TileSize);
            if (returnValue < 0)
                returnValue = 0;
            return returnValue;
        }

        public List<ITile> GetCollidingTiles(Rectangle rectangle)
        {
            ///List of the colliding tiles
            List<ITile> tilesList = new List<ITile>();

            int beginning_x, beginning_y, end_x, end_y;
            beginning_x = GameToIndex(rectangle.Left);
            end_x = GameToIndex(rectangle.Right);
            beginning_y = GameToIndex(rectangle.Top);
            end_y = Math.Max(0, GameToIndex(rectangle.Bottom));

            if (beginning_y < 0)
                beginning_y = 0;
            if (end_y > Metadata.Height - 1)
                end_y = Metadata.Height - 1;
            if (beginning_x < 0)
                beginning_x = 0;
            if (end_x > Metadata.Width - 1)
                end_x = Metadata.Width - 1;

            for (int y = beginning_y; y <= end_y; y++)
                for (int x = beginning_x; x <= end_x; x++)
                    if (TileMap[y, x] != null && TileMap[y, x].Transparency == TileTransparency.FullyOpague)
                        if(rectangle.Intersects(TileMap[y, x].Hitbox))
                            tilesList.Add(TileMap[y, x]);


            return tilesList;
        }

        public ITile GetTileAtIndex(int x, int y)
        {
            if (x < 0 || x > Metadata.Width - 1)
                return null;
            if (y < 0 || y > Metadata.Height - 1)
                return null;

            return TileMap[y, x];
        }

        public void Draw(Graphics.Graphics graphics, Camera2D camera = null)
        {
            // TODO: camera shit
            for(int y = 0; y < Metadata.Height; y++)
            {
                for(int x = 0; x < Metadata.Width; x++)
                {
                    if(TileMap[y, x] != null)
                        TileMap[y, x].Draw(graphics);
                }
            }
        }
        public void Update(GameTime gameTime)
        {}
    }
}
