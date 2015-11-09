using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2D.Saves
{
    public class Tile
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool BackgroundTile { get; set; }

        public Tile()
        {
            Name = "minecraft:air";
            X = 0;
            Y = 0;
            BackgroundTile = false;
        }
    }

    public class PlainTextSaveReader
    {
        private Tile[,] tiles = new Tile[256, 100];
        private StreamReader sr;

        public Tile[,] TileMap { get { return tiles; } }

        public PlainTextSaveReader(string path)
        {
            sr = new StreamReader(path);
        }

        public void ReadSave()
        {
            if (sr == null)
                throw new NullReferenceException("StreamReader null!");

            string input = "";
            while(!sr.EndOfStream)
            {
                input = sr.ReadLine();
                if (input.Trim() != string.Empty)
                {
                    string[] split = input.Split(new char[] { ';' }, 4);
                    int x, y;
                    bool isBg = bool.Parse(split[3]);
                    x = (int)Math.Floor((double)Int32.Parse(split[1]) / 32);
                    y = (int)Math.Floor((double)Int32.Parse(split[2]) / 32);
                    string tileDataName = split[0];
                    Tile t = new Tile { Name = split[0].Trim(), BackgroundTile = isBg, X = x * 32, Y = y * 32};
                    tiles[y, x] = t;
                }
            }
            sr.Close();
        }
    }
}
