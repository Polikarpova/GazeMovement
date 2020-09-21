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
    public partial class ObjectClassesDialog : Form
    {
        //База данных
        private IDatabase DB;

        public ObjectClassesDialog(IDatabase database)
        {
            InitializeComponent();

            this.DB = database;

            //заполняем таблицу информацией
            InitListView();
        }

        private void InitListView()
        {
            List<ObjectClassInfo> data = DB.GetObjectClassesInfo();

            foreach (ObjectClassInfo info in data)
            {
                string commonPattern = info.CommonPattern.Length == 0 ? "Общий паттерн не найден" : info.CommonPattern;
                string gridSize = info.GridSize.X == 0 ? "" : $"{info.GridSize.X} строк(-и) на {info.GridSize.Y} столбцов(-а)";
                objectClassesListView.Items.Add(new ListViewItem(new[] {info.ObjectClassName, commonPattern, gridSize}));
            }
        }
    }
}
