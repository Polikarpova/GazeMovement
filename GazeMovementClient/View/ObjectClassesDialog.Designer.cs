namespace GazeMovementClient.View
{
    partial class ObjectClassesDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objectClassesListView = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.сommonPatternColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gridSizeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // objectClassesListView
            // 
            this.objectClassesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.сommonPatternColumn,
            this.gridSizeColumn});
            this.objectClassesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectClassesListView.Location = new System.Drawing.Point(0, 0);
            this.objectClassesListView.Name = "objectClassesListView";
            this.objectClassesListView.Size = new System.Drawing.Size(552, 254);
            this.objectClassesListView.TabIndex = 0;
            this.objectClassesListView.UseCompatibleStateImageBehavior = false;
            this.objectClassesListView.View = System.Windows.Forms.View.Details;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Имя класса";
            this.nameColumn.Width = 150;
            // 
            // сommonPatternColumn
            // 
            this.сommonPatternColumn.Text = "Общий паттерн";
            this.сommonPatternColumn.Width = 217;
            // 
            // gridSizeColumn
            // 
            this.gridSizeColumn.Text = "Размер сетки";
            this.gridSizeColumn.Width = 120;
            // 
            // ObjectClassesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 254);
            this.Controls.Add(this.objectClassesListView);
            this.Name = "ObjectClassesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Классы объектов";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView objectClassesListView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader сommonPatternColumn;
        private System.Windows.Forms.ColumnHeader gridSizeColumn;
    }
}