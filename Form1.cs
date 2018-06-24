using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reporting.Model;

namespace Reporting
{
    public partial class Form1 : Form
    {
        private BindingList<ReportView> _bindingList = new BindingList<ReportView>();

        public Form1()
        {
            InitializeComponent();
            GetData();
        }

        private void GetData()
        {
            
        }    
    }
}
