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
    public partial class ProcessedDataDialog : Form
    {
        private IDatabase DB;

        public ProcessedDataDialog(IDatabase database, List<ObjectClassInfo> commonPatterns)
        {
            InitializeComponent();

            DB = database;
            
            foreach (ObjectClassInfo info in commonPatterns)
            {
                Label panelTitle = new Label
                {
                    Text = info.ObjectClassName,
                    Anchor = AnchorStyles.Bottom,
                    Size = new Size(178, 13),
                    Location = new Point(12, 12)
                };

                Label panelLabel = new Label
                {
                    Text = info.CommonPattern.Length == 0 ? "Общий паттерн не найден" : info.CommonPattern,
                    Anchor = AnchorStyles.Bottom,
                    Size = new Size(178, 13),
                    Location = new Point(12, 32)
                };

                Label panelGridSizeLabel = new Label
                {
                    Text = info.CommonPattern.Length == 0 ? "" : $"Сетка {info.GridSize.X} строк(-и) на" + Environment.NewLine + $"{info.GridSize.Y} столбцов(-а)",
                    Anchor = AnchorStyles.Bottom,
                    Size = new Size(178, 33),
                    Location = new Point(12, 52)
                };

                Panel panel = new Panel
                {
                    Size = new Size(190, 80)
                };
                panel.Controls.Add(panelTitle);
                panel.Controls.Add(panelLabel);
                panel.Controls.Add(panelGridSizeLabel);

                this.flowLayoutPanel1.Controls.Add(panel);
            }
        }
    }
}
