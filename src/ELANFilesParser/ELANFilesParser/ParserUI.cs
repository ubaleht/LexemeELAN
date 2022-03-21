using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ELANFilesParseLibrary;
using ELANToDatabase;

namespace ELANFilesParser
{
    public partial class ParserUI : Form
    {
        private EAFParser parser;

        public ParserUI()
        {
            InitializeComponent();
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
                openParseFileDialog.InitialDirectory = "c:\\";
                openParseFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openParseFileDialog.FilterIndex = 2;
                openParseFileDialog.RestoreDirectory = true;

                if (openParseFileDialog.ShowDialog() == DialogResult.OK)
                {
                    /*
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    */
                }
            //}

            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void parseButton_Click(object sender, EventArgs e)
        {
            //TODO - path
            parser = new EAFParser(@"D:\Proj\Under-ResourcedLanguages\SIF\SiberianIngrianFinnish\annotations\KKM-34-003.eaf");
            parser.Parse();
            
        }

        private void saveToDatabaseButton_Click(object sender, EventArgs e)
        {
            ELANToMSSQLServer elanToMSSQLServer = new ELANToMSSQLServer();
            elanToMSSQLServer.SaveELANFileToDatabase(parser.AnnotationsFragments);
        }
    }
}
