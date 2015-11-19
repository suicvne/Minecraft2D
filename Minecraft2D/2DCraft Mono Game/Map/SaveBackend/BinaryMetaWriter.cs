using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minecraft2D.Map.SaveBackend
{
    public class BinaryMetaWriter
    {
        private BinaryWriter writer;
        private Minecraft2DMeta metaFile;

        public BinaryMetaWriter(string path, Minecraft2DMeta meta)
        {
            if(File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error writing meta binary: " + ex.Message);
                }
            }
            writer = new BinaryWriter(File.Open(path, FileMode.CreateNew));
            metaFile = meta;
        }

        public void WriteMeta()
        {
            if(writer == null)
                throw new NullReferenceException("BinaryWriter is null!");

            writer.Write($"{(int)Math.Floor((float)metaFile.WorldSize.X / 32)}x{(int)Math.Floor((float)metaFile.WorldSize.Y / 32)}");
            writer.Write($"\"{metaFile.WorldName}\"");
            writer.Write($"{metaFile.PlayerLocation.X},{metaFile.PlayerLocation.Y}");

            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

    }
}
