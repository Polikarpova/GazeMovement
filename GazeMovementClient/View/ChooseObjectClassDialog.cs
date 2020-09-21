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
    public partial class ChooseObjectClassDialog : Form
    {
        public string SelectedItemText { get; private set; }

        public ChooseObjectClassDialog(List<string> objectClassNames)
        {
            InitializeComponent();

            if (objectClassNames.Count != 0)
            {
                this.namesComboBox.Items.AddRange(objectClassNames.ToArray());
                this.namesComboBox.SelectedIndex = 0;
            }
            else
            {
                this.namesComboBox.Enabled = false;
            }
        }

        private void NewClassText_TextChanged(object sender, EventArgs e)
        {
            if (this.newClassText.Text.Length == 0)
            {
                this.addClassButton.Enabled = false;
                this.OkButton.Enabled = this.namesComboBox.SelectedIndex != -1;
            }
            else
            {
                this.addClassButton.Enabled = true;
                this.OkButton.Enabled = false;
            }
        }

        private void AddClassButton_Click(object sender, EventArgs e)
        {
            if (this.namesComboBox.Items.Contains(this.newClassText.Text))
            {
                MessageBox.Show("Такой класс уже существует. Введите другое имя.", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string newName = this.newClassText.Text;
                string tabuSymbols = " _\\/:*?“”«»<>|";

                foreach (char symbol in tabuSymbols)
                {
                    newName = newName.Replace(symbol, '-');
                }

                this.namesComboBox.Items.Add(newName);
                this.namesComboBox.SelectedIndex = this.namesComboBox.Items.Count - 1;
                OkButton_Click(this, new EventArgs());
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.SelectedItemText = this.namesComboBox.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void NamesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OkButton.Enabled = this.namesComboBox.SelectedIndex != -1;
        }
    }
}
