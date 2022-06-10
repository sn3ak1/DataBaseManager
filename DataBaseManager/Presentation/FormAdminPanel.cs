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
            
            // comboBox1.Items.AddRange(UserController.GetUsers());
            comboBox2.Items.AddRange(roles.ToArray());
            foreach (var role in roles)
            {
                comboBox1.Items.AddRange(role.Users.ToArray());
            }
            // listBox1.Items.AddRange(UserController.GetPermissions());
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            UserController.DeleteUser((User)comboBox1.SelectedItem);
            comboBox1.Items.Remove(comboBox1.SelectedItem);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string name = Prompt.ShowDialog("Role name:", "Add role");
            var role = new Role() {Name = name};
            comboBox2.Items.Add(role);
            UserController.AddRole(role);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (var user in ((Role)comboBox2.SelectedItem).Users)
            {
                UserController.DeleteUser(user);
            }
            UserController.DeleteRole((Role)comboBox2.SelectedItem);
            comboBox2.Items.Remove(comboBox2.SelectedItem);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Permission permission = null;
            var formAdd = new FormAddPermision();
            formAdd.Closed += (s, args) =>
            {
                var permission = ((FormAddPermision) s)?.Permission;
                if(permission==null) return;
                listBox1.Items.Add(permission);
                ((Role) comboBox2.SelectedItem).Permissions.Add(permission);
                UserController.EditRole((Role) comboBox2.SelectedItem);
            };
            formAdd.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Role) comboBox2.SelectedItem).Permissions.Remove((Permission)listBox1.SelectedItem);
            UserController.DeletePermission((Permission)listBox1.SelectedItem);
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedItem = ((User) comboBox1.SelectedItem).Role;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var role = (Role) comboBox2.SelectedItem;
            if(((User) comboBox1.SelectedItem)!=null)
                ((User) comboBox1.SelectedItem).Role = role;
            listBox1.Items.Clear();
            if (role.Permissions == null) return;
            foreach (var permission in role.Permissions)
            {
                listBox1.Items.Add(permission);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserController.EditUser((User)comboBox1.SelectedItem);
        }
    }
}