using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataBaseManager.Business;
using DataBaseManager.Data;

namespace DataBaseManager.Presentation
{
    public partial class FormAdminPanel : Form
    {
        public FormAdminPanel()
        {
            InitializeComponent();

            var roles = UserController.GetRoles();
            
            comboBox2.Items.AddRange(roles.ToArray());
            foreach (var role in roles)
            {
                comboBox1.Items.AddRange(role.Users.ToArray());
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            UserController.DeleteUser((User)comboBox1.SelectedItem);
            comboBox1.Items.Remove(comboBox1.SelectedItem);
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedItem = ((User) comboBox1.SelectedItem).Role;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var role = (Role) comboBox2.SelectedItem;
            if(((User) comboBox1.SelectedItem)!=null)
            {
                ((User) comboBox1.SelectedItem).Role = role;
                ((Role) comboBox2.SelectedItem).Users.Add(((User) comboBox1.SelectedItem));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserController.EditUser((User)comboBox1.SelectedItem);
        }
    }
}