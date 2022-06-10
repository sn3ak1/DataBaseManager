using System;
using System.Linq;
using System.Windows.Forms;
using DataBaseManager.Business;

namespace DataBaseManager.Presentation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var form = new FormLogin();
            form.FormClosed += (s, args) =>
            {
                this.Show();
                button2.Enabled = false;
                if (UserController.LoggedAs == null)
                    label2.Text = "guest";
                else
                {
                    label2.Text = UserController.LoggedAs.Name;
                    if (UserController.LoggedAs.Role.Permissions.Any(x => x.Admin))
                        button2.Enabled = true;
                }

            };
            this.Hide();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new FormAdminPanel();
            form.FormClosed += (s, args) => this.Show();
            this.Hide();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new FormMain();
            form.FormClosed += (s, args) => this.Show();
            this.Hide();
            form.Show();
        }
    }
}