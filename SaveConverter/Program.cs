using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft2D.Saves
{
    class SaveConverter
    {
        [STAThread]
        static void Main(string[] args)
        {
            mainLine();
        }

        static void mainLine()
        {
            Console.Clear();
            Console.WriteLine("Minecraft 2D Save Test");
            Console.WriteLine("\nSelect Operation: ");
            Console.WriteLine("1. Read Binary Save");
            Console.WriteLine("2. Read Plain Text Save");

            int input = Convert.ToInt32(Console.ReadLine());
            if (input > 2)
                if (input < 0)
                    mainLine();
            if (input == 2)
                ReadPlainTextSave();
            else if (input == 1)
                ReadBinarySave();
        }

        static void ReadBinarySave()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select binary save to open";
            ofd.Filter = "Minecraft 2D Binary Saves (*.mc2dbin)|*.mc2dbin|All Files (*.*)|*.*";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                BinarySaveReader bsr = new BinarySaveReader(ofd.FileName);
                bsr.ReadMap();
                if(bsr.ReadTiles != null)
                {
                    int phase2Res = Phase2();
                    if (phase2Res == 1)
                    {
                        WriteBinarySave(bsr.ReadTiles);
                    }
                    else if (phase2Res == 2)
                    {
                        WritePlainTextSave();
                    }
                }
            }
        }

        static int Phase2()
        {
            Console.Clear();
            Console.WriteLine("What now?\n");
            Console.WriteLine("1. Write to binary save");
            Console.WriteLine("2. Write to plain text save");

            int input = Convert.ToInt32(Console.ReadLine());
            if (input > 2)
                if (input < 0)
                    Phase2();

            return input;
        }

        static void ReadPlainTextSave()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select plain text save to open";
            ofd.Filter = "Minecraft 2D Plain Text Saves (*.mc2dwld, *.wld)|*.mc2dwld;*.wld|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PlainTextSaveReader bsr = new PlainTextSaveReader(ofd.FileName);
                bsr.ReadSave();
                if (bsr.TileMap != null)
                {
                    int phase2Res = Phase2();
                    if (phase2Res == 1)
                    {
                        WriteBinarySave(bsr.TileMap);
                    }
                    else if (phase2Res == 2)
                    {
                        WritePlainTextSave();
                    }
                }
            }
        }

        static void WritePlainTextSave()
        {
            mainLine();
        }

        static void WriteBinarySave(Tile[,] tileMap)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save binary save";
            sfd.Filter = "Minecraft 2D Binary Saves (*.mc2dbin)|*.mc2dbin";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                BinarySaveWriter bsw = new BinarySaveWriter(sfd.FileName, tileMap);
                bsw.WriteSave();
            }

            mainLine();
        }
    }
}
