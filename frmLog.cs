using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotonController
{
    public partial class frmLog : Form
    {
        public frmLog()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Program.Form.SendGcode();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtResponse.Text = "";
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {

        }
    }
}
