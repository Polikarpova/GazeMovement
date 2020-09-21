using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GazeMovementClient.View
{
    public partial class NewImageNameDialog : Form
    {
        public string FileName { get; set; }
        private string TabuSymbols = " _\\/:*?<>|";

        public NewImageNameDialog(string currentName)
        {
            InitializeComponent();
            FileName = currentName.Replace('_','-');
            this.nameTextBox.Text = FileName;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            FileName = this.nameTextBox.Text;
            foreach (char symbol in TabuSymbols)
            {
                FileName = FileName.Replace(symbol, '-');
            }

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            OkButton.Enabled = nameTextBox.Text.Length != 0;
        }
    }
}
