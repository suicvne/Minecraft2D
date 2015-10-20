using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minecraft2D.Graphics;
using Microsoft.Xna.Framework;

namespace Minecraft2D.Map
{
    public class BlockTemplate
    {
        public TileType Type { get; set; }
        public Entity Drops { get; set; }
        public TileTransparency TransparencyOfTile { get; set; }
        public float Hardness { get; set; }
        public SkinRegion TextureRegion { get; set; }

        public int Light { get; set; }
        public int Absorb { get; set; }

        public BlockTemplate()
        {
            Hardness = 0;
            TransparencyOfTile = TileTransparency.FullyTransparent;
            Type = TileType.Air;
            Drops = null;
            Light = 1;
            Absorb = 15;
        }
        public Tile AsTile()
        {
            Tile returnTile = new Tile();
            returnTile.Type = this.Type;
            returnTile.Drops = Drops;
            returnTile.TextureRegion = TextureRegion;
            returnTile.Hardness = Hardness;
            returnTile.TransparencyOfTile = TransparencyOfTile;
            returnTile.Position = Vector2.Zero;
            returnTile.Light = this.Light;
            returnTile.Absorb = this.Absorb;
            return returnTile;
        }
    }

    /// <summary>
    /// This class defines some preset blocks.
    /// </summary>
    public class PresetBlocks
    {
        public static Tile Self = new Tile { IsSelfPlaceholder = true };
        public static BlockTemplate Stone = new BlockTemplate
        {
            Type = TileType.Stone,
            Hardness = 1.5f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = Self,
            TextureRegion = new SkinRegion(16 * 1, 16 * 0, 16, 16),
            Light = 0,
            Absorb = 6
        };

        public static BlockTemplate Air = new BlockTemplate
        {
            Type = TileType.Air,
            Hardness = 0f,
            TransparencyOfTile = TileTransparency.FullyTransparent,
            Drops = null,
            TextureRegion = null,
            Light = 1,
            Absorb = 15
        };

        public static BlockTemplate Dirt = new BlockTemplate
        {
            Type = TileType.Dirt,
            Hardness = .5f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = Self,
            TextureRegion = new SkinRegion(16 * 2, 16 * 0, 16, 16),
            Light = 0,
            Absorb = 6
        };

        public static BlockTemplate Grass = new BlockTemplate
        {
            Type = TileType.Grass,
            Hardness = .6f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = Dirt.AsTile(),
            TextureRegion = new SkinRegion(16 * 3, 16 * 0, 16, 16),
            Light = 0,
            Absorb = 6
        };
    }
}
