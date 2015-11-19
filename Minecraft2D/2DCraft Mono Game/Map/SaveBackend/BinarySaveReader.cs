using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map.SaveBackend
{
    public class BinarySaveReader
    {
        private BinaryReader reader;
        private Tile[,] tilemap;
        private Minecraft2DMeta metaFile;

        public Tile[,] ReadTiles { get { return tilemap; } }
        private bool isBg;

        public BinarySaveReader(string path, Minecraft2DMeta meta, bool bg = false)
        {
            reader = new BinaryReader(File.Open(path, FileMode.Open));
            isBg = bg;
            metaFile = meta;
        }

        public void ReadMap()
        {
            if (reader == null)
                throw new NullReferenceException("BinaryReader null!");
            tilemap = new Tile[metaFile.WorldSizeInBlocks.Y, metaFile.WorldSizeInBlocks.X];
            int worldSize = ((Convert.ToInt32(metaFile.WorldSizeInBlocks.X)) * (Convert.ToInt32(metaFile.WorldSizeInBlocks.Y)));

            int count = 0;
            while(count < worldSize)
            {
                string tileRead = reader.ReadString();
                string[] split = tileRead.Split(new char[] { ';' }, 4);
                int x, y;
                bool isBg = bool.Parse(split[3]);
                x = (int)Math.Floor((double)Int32.Parse(split[1]) / 32);
                y = (int)Math.Floor((double)Int32.Parse(split[2]) / 32);
                string tileDataName = split[0];

                Tile t = PresetBlocks.TilesList.Find(srch => srch.Name == split[0].Trim()).AsTile();
                t.Position = new Vector2(x * 32, y * 32);
                
                if (y > tilemap.GetLength(0))
                    continue;
                if (x > tilemap.GetLength(1))
                    continue;

                tilemap[y, x] = t;
                count++;
            }

            reader.Close();
            reader.Dispose();
        }
    }
}
