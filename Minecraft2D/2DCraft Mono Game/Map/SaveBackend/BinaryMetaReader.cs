using Microsoft.Xna.Framework;
using Minecraft2D.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map.SaveBackend
{
    public class Minecraft2DMeta
    {
        public Vector2i WorldSize { get; set; }
        public Vector2i WorldSizeInBlocks { get; set; }
        public string WorldName { get; set; }
        public Vector2 PlayerLocation { get; set; }
    }

    public class BinaryMetaReader
    {
        private BinaryReader reader;
        private Minecraft2DMeta metaFile;

        public Minecraft2DMeta ReadMeta { get { return metaFile; } }

        public BinaryMetaReader(string path)
        {
            reader = new BinaryReader(File.Open(path, FileMode.Open));
        }

        public void ReadMetaFile()
        {
            if (reader == null)
                throw new NullReferenceException("BinaryReader null!");

            metaFile = new Minecraft2DMeta();

            int count = 0;
            while(count < 2) //two lines atm
            {
                switch(count)
                {
                    case 0: //World Size
                        string worldSizeInput = reader.ReadString();
                        string[] split = worldSizeInput.Split(new char[] { 'x' }, 2);
                        metaFile.WorldSize = new Vector2i(int.Parse(split[0]) * 32, int.Parse(split[1]) * 32);
                        metaFile.WorldSizeInBlocks = new Vector2i(int.Parse(split[0]), int.Parse(split[1]));
                        break;
                    case 1: //World name
                        string worldNameInput = reader.ReadString();
                        metaFile.WorldName = worldNameInput.Trim('\"');
                        break;
                    case 2: //Player save position
                        string playerPositionInput = reader.ReadString();
                        string[] split2 = playerPositionInput.Split(new char[] { ',' }, 2);
                        metaFile.PlayerLocation = new Vector2(float.Parse(split2[0]), float.Parse(split2[1]));
                        break;
                }

                count++;
            }
        }

    }
}
