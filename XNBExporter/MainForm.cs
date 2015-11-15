using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace XNBExporter
{
    public partial class MainForm : Form
    {
        FolderBrowserDialog InputFileDialog = new FolderBrowserDialog();
        FolderBrowserDialog OutputFileDialog = new FolderBrowserDialog();

        public MainForm()
        {
            InitializeComponent();
            Font = SystemFonts.MessageBoxFont;
            CheckForIllegalCrossThreadCalls = false;

            InputFileDialog.Description = "Select folder with .xnb images to import";
            OutputFileDialog.Description = "Select folder to export to\nExported files will use the same file name\nas their original";
        }

        private void outputDirectory_TextChanged(object sender, EventArgs e)=>decompileButton.Enabled = Directory.Exists(outputDirectory.Text);

        private void inputDirectory_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void FillFileList(string dir)
        {
            foreach (var file in Directory.GetFiles(dir))
            {
                if (file.EndsWith(".xnb"))
                {
                    ListViewItem lvi = new ListViewItem(Path.GetFileName(file));
                    lvi.Checked = true;
                    filesListView.Items.Add(lvi);
                }
            }
        }

        private void inputDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == (int)Keys.Enter)
            {
                if (Directory.Exists(inputDirectory.Text))
                {
                    FillFileList(inputDirectory.Text);
                    if (outputDirectory.Text.Trim() == "")
                        outputDirectory.Text = inputDirectory.Text;
                }
            }
        }

        private void filesListView_DoubleClick(object sender, EventArgs e)
        {
            if(filesListView.SelectedItems.Count > 0)
            {
                filesListView.BeginUpdate();
                filesListView.SelectedItems[0].Checked = true;
                filesListView.EndUpdate();

                PreviewGame pg = new PreviewGame(Path.Combine(inputDirectory.Text, filesListView.SelectedItems[0].Text));
                Thread runThread = new Thread(pg.Run);
                
                    pg.ErrorOccurred += (ex) =>
                    {
                        MessageBox.Show("Error occurred while trying to preview " + filesListView.SelectedItems[0].Text + "\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        filesListView.SelectedItems[0].Checked = false;
                        runThread.Abort();
                        pg.Dispose();

                    };
                runThread.Start();
            }
        }

        private void ControlState(bool state)
        {
            decompileButton.Enabled = state;
            filesListView.Enabled = state;
            button1.Enabled = state;
            button2.Enabled = state;
            outputDirectory.Enabled = state;
            inputDirectory.Enabled = state;
        }
        
        private string[] BuildFilesList()
        {
            List<string> filesAsList = new List<string>();
            foreach(ListViewItem lvi in filesListView.Items)
            {
                if (lvi.Checked)
                    filesAsList.Add(Path.Combine(inputDirectory.Text, lvi.Text));
            }

            return filesAsList.ToArray<string>();
        }

        private void decompileButton_Click(object sender, EventArgs e)
        {
            if(filesListView.Items.Count == 0)
            {
                MessageBox.Show("There are no items to export!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ControlState(false);
            Update();

            ExportGame eg = new ExportGame(BuildFilesList(), outputDirectory.Text);
            eg.Run();

            ControlState(true);
            Update();

            MessageBox.Show("xnb's decompiled to " + outputDirectory.Text + " successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(InputFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputDirectory.Text = InputFileDialog.SelectedPath;
                if (Directory.Exists(inputDirectory.Text))
                {
                    FillFileList(inputDirectory.Text);
                    if(outputDirectory.Text.Trim() == "")
                        outputDirectory.Text = inputDirectory.Text;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(OutputFileDialog.ShowDialog() == DialogResult.OK)
            {
                outputDirectory.Text = OutputFileDialog.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("XNB Exporter by Mike Santiago\n\nUsing MonoGame to provide xnb reading technology.", "About XNB Exporter", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }
    }
}
