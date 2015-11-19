using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map.SaveBackend
{
    public class BinarySaveWriter
    {
        private BinaryWriter writer;
        private Tile[,] tilemap;

        public BinarySaveWriter(string path, Tile[,] tiles)
        {
            if(File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error writing binary: " + ex.Message);
                }
            }

            writer = new BinaryWriter(File.Open(path, FileMode.CreateNew));
            tilemap = tiles;
        }

        public void WriteSave()
        {

            for(int x = 0; x < tilemap.GetLength(1); x++)
            {
                for(int y = 0; y < tilemap.GetLength(0); y++)
                {
                    Tile t = tilemap[y, x];
                    if (t == null)
                    {
                        writer.Write($"minecraft:air;{x * 32};{y * 32};{false}");
                        continue;
                    }
                    writer.Write($"{t.Name};{t.Position.X};{t.Position.Y};{t.IsBackground}");
                }
            }

            writer.Flush();
            writer.Close();
            writer.Dispose();
        }
    }
}
