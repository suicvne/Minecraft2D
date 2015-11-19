using Microsoft.Xna.Framework;
using Minecraft2D.Graphics;
using Minecraft2D.Map.SaveBackend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map
{
    public class WorldObjects
    {
        public Tile[,] ForegroundLayerTiles { get; set; }
        public Tile[,] BackgroundLayerTiles { get; set; }
        public int[,] Lightmap { get; set; }
        public Minecraft2DMeta MetaFile { get; set; }

        private PresetBlocks presets = new PresetBlocks();

        public int WorldIndex { get; set; }

        private World _worldRef;

        public WorldObjects(World wld)
        {
            MetaFile = new Minecraft2DMeta();

            MetaFile.WorldSize = new Vector2i(100 * 32, 256 * 32);
            MetaFile.WorldSizeInBlocks = new Vector2i(100, 256);
            ForegroundLayerTiles = new Tile[MetaFile.WorldSizeInBlocks.Y, MetaFile.WorldSizeInBlocks.X];
            BackgroundLayerTiles = new Tile[MetaFile.WorldSizeInBlocks.Y, MetaFile.WorldSizeInBlocks.X];
            Lightmap = new int[MetaFile.WorldSizeInBlocks.Y, MetaFile.WorldSizeInBlocks.X];
            MetaFile.WorldName = "World1";

            _worldRef = wld;
            WorldIndex = 0;
        }

        public void GenerateFlatlands()
        {
            Lightmap = new int[ForegroundLayerTiles.GetLength(0), ForegroundLayerTiles.GetLength(1)];
            
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if (y > 32)
                    {
                        if (y == 33)
                        {
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:grass").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            ForegroundLayerTiles[y, x] = t;
                            BackgroundLayerTiles[y, x] = t;
                            t = null;
                        }
                        else if (y <= 37)
                        {
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:dirt").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            ForegroundLayerTiles[y, x] = t;
                            BackgroundLayerTiles[y, x] = t;
                            t = null;
                        }
                        else if (y == 255)
                        {
                            //TODO: bedrock
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:bedrock").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            ForegroundLayerTiles[y, x] = t;
                            BackgroundLayerTiles[y, x] = t;
                            t = null;
                        }
                        else
                        {
                            Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:stone").AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            ForegroundLayerTiles[y, x] = t;
                            BackgroundLayerTiles[y, x] = t;
                            t = null;
                        }
                    }
                    else
                    {
                        Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:air").AsTile();
                        t.Position = new Vector2(x * 32, y * 32);
                        ForegroundLayerTiles[y, x] = t;
                        t = null;
                    }
                }
            }
        }

        public void GenerateBlankWorld()
        {
            for(int x = 0; x < ForegroundLayerTiles.GetLength(1); x++)
            {
                for (int y = 0; y < ForegroundLayerTiles.GetLength(0); y++)
                {
                    Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == "minecraft:air").AsTile();
                    t.Position = new Vector2(x * 32, y * 32);
                    ForegroundLayerTiles[y, x] = t;
                }
            }
        }

        public void GenerateInitialLightmap()
        {
            for (int x = 0; x < ForegroundLayerTiles.GetLength(1); x++)
            {
                for (int y = 0; y < ForegroundLayerTiles.GetLength(0); y++)
                {
                    Lightmap[y, x] = ForegroundLayerTiles[y, x].Light;
                }
            }
        }

        public void SaveWorld()
        {
            string saveDirectory = Path.Combine(MainGame.GameSaveDirectory, $"World{WorldIndex}");
            if (!Directory.Exists(MainGame.GameSaveDirectory))
            {
                Directory.CreateDirectory(MainGame.GameSaveDirectory);
            }
            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);

            BinaryMetaWriter bmw = new BinaryMetaWriter(Path.Combine(saveDirectory, "world.mc2dmeta"), MetaFile);
            bmw.WriteMeta();
            bmw = null;

            BinarySaveWriter bsw = new BinarySaveWriter(Path.Combine(saveDirectory, "foreground.mc2dbin"), ForegroundLayerTiles);
            bsw.WriteSave();
            bsw = new BinarySaveWriter(Path.Combine(saveDirectory, "background.mc2dbin"), BackgroundLayerTiles);
            bsw.WriteSave();

            bsw = null;
        }

        public void LoadWorld()
        {
            string saveDirectory = Path.Combine(MainGame.GameSaveDirectory, $"World{WorldIndex}");
            if (!Directory.Exists(saveDirectory))
            {
                throw new FileNotFoundException("Save located in " + saveDirectory + "does not exist!");
            }

            BinaryMetaReader bmr = new BinaryMetaReader(Path.Combine(saveDirectory, "world.mc2dmeta"));
            bmr.ReadMetaFile();
            MetaFile = bmr.ReadMeta;
            bmr = null;

            BinarySaveReader bsr = new BinarySaveReader(Path.Combine(saveDirectory, "foreground.mc2dbin"), MetaFile);
            bsr.ReadMap();
            ForegroundLayerTiles = bsr.ReadTiles;

            bsr = new BinarySaveReader(Path.Combine(saveDirectory, "background.mc2dbin"), MetaFile);
            bsr.ReadMap();
            BackgroundLayerTiles = bsr.ReadTiles;

            bsr = null;
        }
    }
}
