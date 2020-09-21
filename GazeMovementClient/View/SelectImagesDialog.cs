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
    public partial class SelectImagesDialog : Form
    {
        //База данных
        private IDatabase DB;

        public List<int> SelectedImages { get; private set; }

        public SelectImagesDialog(IDatabase database, List<int> selectedImages)
        {
            this.DB = database;

            InitializeComponent();

            SelectedImages = new List<int>(selectedImages);

            InitImagesBox();
        }

        private void InitImagesBox()
        {
            //Заполняем список
            this.ImagesBox.Items.Clear();

            List<string> names = DB.GetImageNames();

            foreach (string name in names)
            {
                this.ImagesBox.Items.Add(name);
            }

            //Выделям изображения
            foreach(int index in SelectedImages)
            {
                ImagesBox.SetSelected(index, true);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            SelectedImages.Clear();

            foreach(var index in this.ImagesBox.SelectedIndices)
            {
                SelectedImages.Add((int)index);
            }

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
