using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2DRebirth.Maps
{
    /// <summary>
    /// The "transparency" of a tile defining whether or not you can fully pass through it or not.
    /// </summary>
    public enum TileTransparency : int
    {
        FullyOpague, //cant be passed through
        FullyTransparent, //can be passed through
        PassThroughBreakable //can pass through but can't break
    }

    public interface ITile : IAnimatedEntity
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
        /// The transparency of the tile (eg: passable, etc)
        /// </summary>
        TileTransparency Transparency { get; set; }

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
