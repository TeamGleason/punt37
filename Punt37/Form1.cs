using System;
using System.Windows.Forms;

namespace Punt37
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            this.Resize += Main_Resize;
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ListenForPunt.Listen();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Show();
            Activate();

            Reboot.ForceReboot();
        }
    }
}
