using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataBaseManager.Business;
using DataBaseManager.Data;

namespace DataBaseManager.Presentation
{
    public partial class FormAddPermision : Form
    {
        
        public Permission Permission { get; set; }
        
        public FormAddPermision()
        {
            InitializeComponent();
            checkedListBox1.Items.AddRange(Controller.GetCategories());
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            List<Category> categories = new();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                categories.Add((Category) item);
            }

            Permission = new Permission()
            {
                Name = textBox1.Text,
                Admin = checkBox1.Checked,
                CanAddCategories = checkBox2.Checked,
                ModifiableCategories = categories
            };
            
            Close();
        }
    }
}