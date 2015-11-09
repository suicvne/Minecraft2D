using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2D.Saves
{
    public class BinarySaveReader
    {
        private BinaryReader reader;
        private Tile[,] tilemap;

        public Tile[,] ReadTiles { get { return tilemap; } }

        public BinarySaveReader(string path)
        {
            reader = new BinaryReader(File.Open(path, FileMode.Open));
        }

        public void ReadMap()
        {
            if (reader == null)
                throw new NullReferenceException("BinaryReader null!");

            string sizeRead = reader.ReadString();
            string[] split = sizeRead.Split(new char[] { '=', 'x' });
            int worldSize = ((Convert.ToInt32(split[1])) * (Convert.ToInt32(split[2])));
            tilemap = new Tile[int.Parse(split[1]), int.Parse(split[2])];

            int count = 0;
            while(count < worldSize)
            {
                string tileRead = reader.ReadString();
                string[] split2 = tileRead.Split(new char[] { ';' }, 4);
                int x, y;
                bool isBg = bool.Parse(split2[3]);
                x = (int)Math.Floor((double)Int32.Parse(split2[1]) / 32);
                y = (int)Math.Floor((double)Int32.Parse(split2[2]) / 32);
                string tileDataName = split2[0];
                Tile t = new Tile { Name = split2[0].Trim(), BackgroundTile = isBg, X = x * 32, Y = y * 32 };
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
