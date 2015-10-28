using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minecraft2D.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Minecraft2D.Map
{
    public enum TileType
    {
        Air, Grass, Dirt, Stone, Torch, Jack
    }

    public enum TileTransparency
    {
        FullyOpague, FullyTransparent, PassThroughBreakable
    }

    [Serializable]
    public class Tile : Entity
    {
        public TileType Type { get; set; }
        public Entity Drops { get; set; }
        public Vector2 Position { get; set; }
        public string PlaceSoundName { get; set; }

        /// <summary>
        /// If this is true, the player will pass through the blocks like water.
        /// </summary>
        public TileTransparency TransparencyOfTile { get; set; }

        /// <summary>
        /// The hardness of the block
        /// 
        /// -1 would be unbreakable, like Bedrock in Minecraft.
        /// 100 is the value of water and lava.
        /// 50 is the value of Obsidian.
        /// </summary>
        public float Hardness { get; set; }
        public SkinRegion TextureRegion { get; set; }
        public bool IsSelfPlaceholder { get; set; }
        public bool IsBackground { get; set; }

        public int Light { get; set; }

        public int LightOffset { get; set; }


        public Tile() 
        {
            Hardness = 0; //instant break
            TransparencyOfTile = TileTransparency.FullyTransparent;
            Type = TileType.Air;
            Drops = null;
            IsSelfPlaceholder = false;
            IsBackground = false;
            PlaceSoundName = null;
        }
        public Tile Copy()
        {
            Tile tile = new Tile();
            tile.Drops = this.Drops;
            tile.Hardness = this.Hardness;
            tile.Position = this.Position;
            tile.Type = this.Type;
            tile.TransparencyOfTile = this.TransparencyOfTile;
            tile.TextureRegion = this.TextureRegion;
            return tile;
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
