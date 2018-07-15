using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockSolidEngine.Maps
{
    /// <summary>
    /// The type of tool to break the block with.
    /// </summary>
    public enum ToolType : int
    {
        None = int.MaxValue, //no tool can break this.
        Hand = 0,
        Shovel = 1,
        Axe = 2,
        Pickaxe = 3
    }
    
    /// <summary>
    /// dont ask
    /// </summary>
    interface IMinecraftBlock : ITile
    {
        /// <summary>
        /// The <see cref="Name"/> of the tile this tile drops on being broken.
        /// </summary>
        string Drops { get; set; }

        string PlaceSoundName { get; set; }

        ToolType PreferredTool { get; set; }

        /// <summary>
        /// How hard the block is.
        /// -1 would be unbreakable.
        /// 100 would be for water/lava
        /// 50 for obsidian.
        /// </summary>
        float Hardness { get; set; }

        /// <summary>
        /// If true, the Tile is in the background layer and is darker than the foreground.
        /// </summary>
        bool InBackground { get; set; }

        //TODO: lighting maybe?
    }
}
