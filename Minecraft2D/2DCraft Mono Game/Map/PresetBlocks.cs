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
        public string PlaceSoundName { get; set; }

        public int Light { get; set; }

        public BlockTemplate()
        {
            Hardness = 0;
            TransparencyOfTile = TileTransparency.FullyTransparent;
            Type = TileType.Air;
            Drops = null;
            Light = 5;
            PlaceSoundName = null;
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
            returnTile.PlaceSoundName = this.PlaceSoundName;
            returnTile.LightOffset = (int)(Math.Floor((float)Light / 2) * 32);
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
            PlaceSoundName = "stone{0}"
        };

        public static BlockTemplate Air = new BlockTemplate
        {
            Type = TileType.Air,
            Hardness = 0f,
            TransparencyOfTile = TileTransparency.FullyTransparent,
            Drops = null,
            TextureRegion = null,
            Light = 5,
            PlaceSoundName = null
        };

        public static BlockTemplate Dirt = new BlockTemplate
        {
            Type = TileType.Dirt,
            Hardness = .5f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = Self,
            TextureRegion = new SkinRegion(16 * 2, 16 * 0, 16, 16),
            Light = 0,
            PlaceSoundName = "gravel{0}"
        };

        public static BlockTemplate Grass = new BlockTemplate
        {
            Type = TileType.Grass,
            Hardness = .6f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = Dirt.AsTile(),
            TextureRegion = new SkinRegion(16 * 3, 16 * 0, 16, 16),
            Light = 0,
            PlaceSoundName = "grass{0}"
        };

        public static BlockTemplate Torch = new BlockTemplate
        {
            Type = TileType.Torch,
            Hardness = .2f,
            TransparencyOfTile = TileTransparency.PassThroughBreakable,
            Drops = Self,
            TextureRegion = new SkinRegion(16 * 0, 16 * 5, 16, 16),
            Light = 12,
            PlaceSoundName = "wood{0}"
        };

        public static BlockTemplate JackOLantern = new BlockTemplate
        {
            Type = TileType.Jack,
            Hardness = .2f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = Self,
            TextureRegion = new SkinRegion(16 * 8, 16 * 7, 16, 16),
            Light = 3,
            PlaceSoundName = "wood{0}"
        };

        public static BlockTemplate[] BlocksAsArray()
        {
            List<BlockTemplate> tilesList = new List<BlockTemplate>();
            tilesList.Add(PresetBlocks.Dirt);
            tilesList.Add(Grass);
            tilesList.Add(Stone);
            tilesList.Add(Torch);
            tilesList.Add(JackOLantern);

            return tilesList.ToArray<BlockTemplate>();
        }
    }
}
