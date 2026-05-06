using neoStockMasterv2.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot.WinForms;

namespace neoStockMasterv2.Forms
{
    public partial class ZReportScreen : Form
    {
        public static User LoggedInUser { get; set; }

        public ZReportScreen(string language)
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
