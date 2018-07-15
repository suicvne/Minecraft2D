using Microsoft.Xna.Framework;
using RockSolidEngine.Graphics;
using RockSolidEngine.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine
{
    // TODO: do i want map to implement ILightable or rely on the scene to provide lighting..?

    public struct MapMetadata
    {
        /// <summary>
        /// The width of the map, in tiles.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of the map, in tiles.
        /// </summary>
        public int Height;

        /// <summary>
        /// The name of the map.
        /// </summary>
        public string MapName;
        
    }

    public interface IMap
    {
        ITile[,] TileMap { get; set; }
        MapMetadata Metadata { get; set; }

        void Draw(Graphics.Graphics graphics, Camera2D camera = null);
        void Update(GameTime gameTime);
    }
}
