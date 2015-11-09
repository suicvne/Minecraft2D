using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2D.Saves
{
    public class BinarySaveWriter
    {
        private BinaryWriter writer;
        private Tile[,] tilemap;
        public BinarySaveWriter(string path, Tile[,] tiles)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error writing binary: " + ex.Message);
                }
            }
            writer = new BinaryWriter(File.Open(path, FileMode.CreateNew));
            tilemap = tiles;
        }

        public void WriteSave()
        {
            if (writer == null)
                throw new NullReferenceException("BinaryWriter is null!");
            writer.Write($"size={tilemap.GetLength(0)}x{tilemap.GetLength(1)}");
            foreach(var t in tilemap)
            {
                writer.Write($"{t.Name};{t.X};{t.Y};{t.BackgroundTile.ToString()}");
            }
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }
    }
}
