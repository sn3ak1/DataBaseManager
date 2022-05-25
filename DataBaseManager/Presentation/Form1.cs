using System;
using System.Windows.Forms;

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
            FormView formView = new FormView();
            formView.FormClosed += (s, args) => this.Show();
            this.Hide();
            formView.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormAdd formAdd = new FormAdd(null);
            formAdd.FormClosed += (s, args) => this.Show();
            this.Hide();
            formAdd.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.FormClosed += (s, args) => this.Show();
            this.Hide();
            form.Show();
        }
    }
}