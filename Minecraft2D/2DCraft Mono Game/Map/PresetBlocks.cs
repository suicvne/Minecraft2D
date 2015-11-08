using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minecraft2D.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;

namespace Minecraft2D.Map
{
    public class BlockTemplate
    {
        public string Name { get; set; }
        public TileType Type { get; set; }
        public string Drops { get; set; }
        public TileTransparency TransparencyOfTile { get; set; }
        public float Hardness { get; set; }
        public SkinRegion TextureRegion { get; set; }
        public string PlaceSoundName { get; set; }

        public int Light { get; set; }

        public BlockTemplate()
        {
            Name = "minecraft:air";
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
            returnTile.Name = this.Name;
            returnTile.Type = this.Type;
            returnTile.Drops = this.Name;
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
        //public static Tile Self = new Tile { IsSelfPlaceholder = true };
        #region Initial Generation Crap
        public static BlockTemplate Stone = new BlockTemplate
        {
            Name = "minecraft:stone",
            Type = TileType.Stone,
            Hardness = 1.5f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = "minecraft:stone",
            TextureRegion = new SkinRegion(16 * 1, 16 * 0, 16, 16),
            Light = 0,
            PlaceSoundName = "stone{0}"
        };

        public static BlockTemplate Air = new BlockTemplate
        {
            Name = "minecraft:air",
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
            Name = "minecraft:dirt",
            Type = TileType.Dirt,
            Hardness = .5f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = "minecraft:dirt",
            TextureRegion = new SkinRegion(16 * 2, 16 * 0, 16, 16),
            Light = 0,
            PlaceSoundName = "gravel{0}"
        };

        public static BlockTemplate Grass = new BlockTemplate
        {
            Name = "minecraft:grass",
            Type = TileType.Grass,
            Hardness = .6f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = "minecraft:dirt",
            TextureRegion = new SkinRegion(16 * 3, 16 * 0, 16, 16),
            Light = 0,
            PlaceSoundName = "grass{0}"
        };

        public static BlockTemplate Torch = new BlockTemplate
        {
            Name = "minecraft:torch",
            Type = TileType.Torch,
            Hardness = .2f,
            TransparencyOfTile = TileTransparency.PassThroughBreakable,
            Drops = "minecraft:torch",
            TextureRegion = new SkinRegion(16 * 0, 16 * 5, 16, 16),
            Light = 12,
            PlaceSoundName = "wood{0}"
        };

        public static BlockTemplate JackOLantern = new BlockTemplate
        {
            Name = "minecraft:jackolantern",
            Type = TileType.Jack,
            Hardness = .2f,
            TransparencyOfTile = TileTransparency.FullyOpague,
            Drops = "minecraft:jackolantern",
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
        #endregion

        public static void WriteBlocksList()
        {
            List<BlockTemplate> test = new List<BlockTemplate>();
            test.Add(Stone);
            test.Add(Air);
            test.Add(Grass);
            test.Add(Dirt);
            test.Add(Torch);
            test.Add(JackOLantern);

            BlockTemplate[] testArray = test.ToArray<BlockTemplate>();

            JsonSerializer js = new JsonSerializer();
            js.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "blocks1.json"))
            {
                using (JsonWriter jsw = new JsonTextWriter(sw))
                {
                    js.Serialize(jsw, testArray);
                }
            }
        }

        public static BlockTemplate[] TilesListAsArray()
        {
            List<BlockTemplate> tempList = new List<BlockTemplate>();
            foreach (var bltmp in TilesList)
                if (!bltmp.Name.Contains("air"))
                    if(!bltmp.Name.Contains("bedrock"))
                        tempList.Add(bltmp);

            return tempList.ToArray<BlockTemplate>();
        }

        public static List<BlockTemplate> TilesList
        {
            get; set;
        }

        public static void LoadBlocksList()
        {
            TilesList = new List<BlockTemplate>();

            if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "blocks.json"))
            {
                JsonSerializer js = new JsonSerializer();
                js.Formatting = Formatting.Indented;
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "blocks.json"))
                {
                    using (JsonReader jsr = new JsonTextReader(sr))
                    {
                        BlockTemplate[] blocksArr = js.Deserialize<BlockTemplate[]>(jsr);
                        TilesList = ArrayToList(blocksArr);
                    }
                }
            }
        }

        private static List<BlockTemplate> ArrayToList(BlockTemplate[] arr)
        {
            List<BlockTemplate> returnVal = new List<BlockTemplate>();

            foreach (var btmpl in arr)
                returnVal.Add(btmpl);

            return returnVal;
        }

    }
}
