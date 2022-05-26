using System;
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
                label2.Text = UserController.LoggedAs.Name==null ? "guest" : UserController.LoggedAs.Name;
            };
            this.Hide();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new FormAdd(null);
            form.FormClosed += (s, args) => this.Show();
            this.Hide();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new Form2();
            form.FormClosed += (s, args) => this.Show();
            this.Hide();
            form.Show();
        }
    }
}