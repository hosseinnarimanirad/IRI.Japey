using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainProject.Forms
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = @"D:\Data\Aja\SHP_Map\City_Poly.dbf";

            OpenFileDialog dialog = new OpenFileDialog() { Filter = "dBase IV|*.dbf" };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            filePath = dialog.FileName;

            System.Data.DataTable table = IRI.Ket.ShapefileFormat.Dbf.DbfFile.Read(filePath,  "myTable");

            this.dataGridView1.DataSource = table;
        }
    }
}
