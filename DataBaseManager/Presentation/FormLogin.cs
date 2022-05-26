using System.Windows.Forms;
using DataBaseManager.Business;

namespace DataBaseManager.Presentation
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 14;
            
            button1.Click += (o, i) =>
            {
                if (UserController.Login(textBox1.Text, textBox2.Text))
                {
                    MessageBox.Show("Login successful");
                    this.Close();
                }
                else
                    MessageBox.Show("Wrong credentials");
            };
            button2.Click += (o, i) =>
            {
                MessageBox.Show(UserController.AddUser(textBox1.Text, textBox2.Text)
                    ? "Registration successful"
                    : "Login is taken!");
            };
        }
        
    }
}