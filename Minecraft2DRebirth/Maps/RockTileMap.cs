using System;
using System.IO;
using Microsoft.Xna.Framework;
using RockSolidEngine.Graphics;

namespace RockSolidEngine.Maps
{
    public class RockTileMap : IMap
    {
        public static char HEADER_0 = 'M';
        public static char HEADER_1 = 'S';
        public static short VERSION = 05;
        public ITile[,] TileMap { get; set; }
        public MapMetadata Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RockSolidEngine.Maps.RockTileMap"/> class with a default size of 32x32.
        /// </summary>
        public RockTileMap()
        {
            Metadata = new MapMetadata()
            {
                Width = 32,
                Height = 32,
                MapName = "TestMap"
            };
            TileMap = new ITile[Metadata.Height, Metadata.Width];
        }

        public RockTileMap(int width, int height, string name = "New Map")
        {
            Metadata = new MapMetadata()
            {
                Width = width,
                Height = height,
                MapName = name
            };
            TileMap = new ITile[Metadata.Height, Metadata.Width];
        }

        private string ReadStringFromBinary( BinaryReader reader)
        {
            short stringLength = reader.ReadInt16();

            if (stringLength > 0)
            {
                string returnValue = "";
                for (short i = 0; i < stringLength; i++)
                {
                    char readChar = reader.ReadChar();
                    returnValue += readChar.ToString();
                }
                reader.ReadChar(); //to skip over the null termination at the end of the string, C# doesn't need it.
                return returnValue.Trim();
            }
            else
                return null;
        }

        public void LoadMapFromFile(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                char[] header = { 'A', 'A' };

                // First step is to read in the header and verify this is the correct file type
                header[0] = reader.ReadChar();
                header[1] = reader.ReadChar();

                if(header[0] == HEADER_0 && header[1] == HEADER_1) // we're all set and we can move on
                {
                    short version = reader.ReadInt16();
                    if (version >= 4 || version == VERSION)
                    {
                        string mapName = ReadStringFromBinary(reader);
                        string tilesetName = ReadStringFromBinary(reader);
                        string foregroundTilesetName = "NONE";
                        if (version >= 5)
                            foregroundTilesetName = ReadStringFromBinary(reader);
                        int width, height;
                        width = reader.ReadInt32();
                        height = reader.ReadInt32();

                        Console.WriteLine($"------\nMap Name: {mapName}\nTileset Name: {tilesetName}\nTileset2: {foregroundTilesetName}\nSize: {width} x {height}\n------");

                        MapMetadata ReadMapMetadata = new MapMetadata();

                        ReadMapMetadata.MapName = mapName;
                        ReadMapMetadata.Width = width;
                        ReadMapMetadata.Height = height;

                        this.Metadata = ReadMapMetadata;

                        TileMap = new ITile[ReadMapMetadata.Height, ReadMapMetadata.Width];

                        int totalTiles = Metadata.Width * Metadata.Height;
                        for (int i = 0; i < totalTiles; i++)
                        {
                            short id = reader.ReadInt16();
                            short idLayer2 = -1;
                            if (version >= 5)
                                idLayer2 = reader.ReadInt16();
                            short angle = 0;
                            byte angleChar = reader.ReadByte();
                            switch (angleChar)
                            {
                                case 1:
                                    angle = 0;
                                    break;
                                case 2:
                                    angle = 90;
                                    break;
                                case 3:
                                    angle = 180;
                                    break;
                                case 4:
                                    angle = 270;
                                    break;
                            }
                            //TODO: an enum for tile collision? also TODO: check that tile collision isn't already a thing idk 
                            byte collision = reader.ReadByte();

                            Point pointIn2DArray = IndexToPoint(i, Metadata.Width, Metadata.Height);
                            if (id >= 0)
                            {
                                ITile tile = new RockTile();
                                TileMap[pointIn2DArray.Y, pointIn2DArray.X] = tile;
                            }

                        }
                    }
                    else
                        throw new Exception($"Wrong version in map file (Reading {VERSION}, map file was {version}).");
                }
                else
                {
                    throw new Exception($"Wrong file type (Expected {HEADER_0}{HEADER_1}, got {header[0]}{header[1]}).");
                }
            }
        }

        private Point IndexToPoint(int index, int width, int height)
        {
            int x = (index % width);
            int y = (index / width);

            return new Point(x, y);
        }

        public void Draw(Graphics.Graphics graphics, Camera2D camera = null)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
