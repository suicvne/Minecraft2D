using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Maps
{
    public interface ITile : IEntity
    {
        /// <summary>
        /// The name of the tile.
        /// </summary>
        //string Name { get; set; }

        /// <summary>
        /// The location of the Tile relative to the world.
        /// </summary>
        //Vector2 Position { get; set; }

        /// <summary>
        /// The name of the Texture.
        /// </summary>
        string SheetName { get; set; }

        /// <summary>
        /// The location of the tile on the sheet.
        /// </summary>
        Vector2 TileIndex { get; set; }

        /// <summary>
        /// Returns a rectangle object representing the tile's rectangle in the world.
        /// </summary>
        //Rectangle Bounds();

        /// <summary>
        /// x
        /// </summary>
        /// <returns></returns>
        float LeftSide();

        /// <summary>
        /// x + w
        /// </summary>
        /// <returns></returns>
        float RightSide();

        /// <summary>
        /// y
        /// </summary>
        /// <returns></returns>
        float TopSide();

        /// <summary>
        /// y + height
        /// </summary>
        /// <returns></returns>
        float BottomSide();
        
        //void Draw(Graphics.Graphics graphics);

        //void Update(GameTime gameTime);
    }
}
