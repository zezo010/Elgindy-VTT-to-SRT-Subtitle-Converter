using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is an open source tool for converting vtt subtitle files to srt one.
// Created by Mahmoud Elgindy at 2019

namespace Elgindy_VTT_to_SRT_Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string dfltpath;
        string savpath;
        FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
        string srcfilepath;
        string destfilepath;
        string destfilename;
        int i;
        private void Form1_Load(object sender, EventArgs e)
        {
            dfltpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Elgindy VTT to SRT Converter");
            checkpath(dfltpath);
            textBox1.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Elgindy VTT to SRT Converter");
            savpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Elgindy VTT to SRT Converter");
        }
        private void checkpath(string dfltpatth)
        {
            try
            {
                if (!Directory.Exists(dfltpatth))
                {
                    Directory.CreateDirectory(dfltpatth);
                }
            }
            catch { }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Add Files
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "WebVTT files (*.vtt)|*.vtt",
                //RestoreDirectory = true,
                Multiselect = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in openFileDialog1.FileNames)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { Path.GetFileNameWithoutExtension(str), Path.GetFullPath(str), "" }));
                } 
            }
            groupBox3.Text = "Files: " + listView1.Items.Count.ToString() + " files";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // Add Folder
            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var files = Directory.EnumerateFiles(folderBrowserDialog1.SelectedPath, "*.vtt", SearchOption.AllDirectories);
                foreach(string filename in files)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { Path.GetFileNameWithoutExtension(filename), Path.GetFullPath(filename), "" }));
                }
            }
            groupBox3.Text = "Files: " + listView1.Items.Count.ToString() + " files";
        }

        private void AddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // add files from menu
            button1.PerformClick();
        }

        private void AddFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // add folder from menu
            button2.PerformClick();
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // remove files from listview1 from menu
            button3.PerformClick();
        }

        private void OpenFileContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open selected file location from menu
            if (listView1.SelectedItems.Count != 0)
            {
                foreach (ListViewItem opitem in listView1.SelectedItems)
                {
                    Process.Start(Path.GetDirectoryName(opitem.SubItems[1].Text));
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            // remove selected files from listview1
            if (listView1.SelectedItems.Count != 0)
            {
                foreach (ListViewItem remitem in listView1.SelectedItems)
                {
                    remitem.Remove();
                }
            }
            groupBox3.Text = "Files: " + listView1.Items.Count.ToString() + " files";
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            // Clear listview1
            listView1.Items.Clear();
            groupBox3.Text = "Files: " + listView1.Items.Count.ToString() + " files";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            // About
            Form2 abt = new Form2();
            abt.ShowDialog();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            // Add a target folder path for saving converted files
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                savpath = folderBrowserDialog1.SelectedPath;
                textBox1.Text = savpath;
            }

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            // Converting process
            try
            {
                listView1.Select();
                checkpath(dfltpath);
                if (listView1.Items.Count == 0)
                {
                    MessageBox.Show("You should add some files!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    foreach (ListViewItem iteem in listView1.Items)
                    {
                        i = iteem.Index;
                        listView1.Items[i].Selected = true;
                        listView1.EnsureVisible(i);
                        iteem.SubItems[2].Text = "Converting....";
                        destfilename = iteem.SubItems[0].Text;
                        srcfilepath = iteem.SubItems[1].Text;
                        //MessageBox.Show(destfilename);
                        //MessageBox.Show(srcfilepath);
                        if (radioButton1.Checked)
                        {
                            destfilepath = Path.GetDirectoryName(srcfilepath);
                        }
                        else if (radioButton2.Checked)
                        {
                            destfilepath = savpath;
                        }
                        //MessageBox.Show(destfilepath);
                        ConvertToSrt(srcfilepath);
                        if (checkBox1.Checked == true)
                        {
                            File.Delete(srcfilepath);
                        }
                        iteem.SubItems[2].Text = "Successfully";
                    }
                    MessageBox.Show("Successfully finished !!", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {

            }
        }
        void ConvertToSrt(string filePathh)
        {
            // Converting Function
            try
            {
                using (StreamReader stream = new StreamReader(filePathh))
                {
                    //MessageBox.Show(filePathh);
                    StringBuilder output = new StringBuilder();
                    int lineNumber = 1;
                    while (!stream.EndOfStream)
                    {
                        string line = stream.ReadLine();
                        if (IsTimecode(line))
                        {
                            output.AppendLine(lineNumber.ToString());
                            lineNumber++;
                            line = line.Replace('.', ',');
                            line = DeleteCueSettings(line);
                            output.AppendLine(line);
                            bool foundCaption = false;
                            while (true)
                            {
                                if (stream.EndOfStream)
                                {
                                    if (foundCaption)
                                        break;
                                    else
                                        throw new Exception("Corrupted file: Found timecode without caption");
                                }
                                line = stream.ReadLine();
                                if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
                                {
                                    output.AppendLine();
                                    break;
                                }
                                foundCaption = true;
                                output.AppendLine(line);
                            }
                        }
                    }
                    string fileName = destfilename + ".srt";
                    //MessageBox.Show(fileName);
                    using (StreamWriter outputFile = new StreamWriter(destfilepath + '\\' + fileName))
                        outputFile.Write(output);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsTimecode(string line)
        {
            // Check time line
            return line.Contains("-->");
        }

        string DeleteCueSettings(string line)
        {
            StringBuilder output = new StringBuilder();
            foreach (char ch in line)
            {
                char chLower = Char.ToLower(ch);
                if (chLower >= 'a' && chLower <= 'z')
                {
                    break;
                }
                output.Append(ch);
            }
            return output.ToString();
        }
        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Open selected file location using Double click
            if (listView1.SelectedItems.Count != 0)
            {
                foreach (ListViewItem opitem in listView1.SelectedItems)
                {
                    Process.Start(Path.GetDirectoryName(opitem.SubItems[1].Text));
                }
            }
        }
    }
}
