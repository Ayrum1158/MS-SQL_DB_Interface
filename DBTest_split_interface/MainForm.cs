using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBTest_split_interface
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DB_interface DBI = new DB_interface();


            object[] vals = new object[2] { 6, "engine6" };

            DBI.InsertInto("Detal", new string[] { "id", "Name" }, vals);

            var temp1 = DBI.SelectAllFrom("Detal");

            var temp2 = DBI.ClearTable("Detal");
        }
    }
}
