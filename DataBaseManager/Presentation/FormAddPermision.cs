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
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            checkedListBox1.Items.AddRange(Controller.GetCategoriesBasic());
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Permission = new Permission()
            {
                Name = textBox1.Text,
                Admin = checkBox1.Checked,
                CanAddCategories = checkBox2.Checked,
                ModifiableCategory = (Category) checkedListBox1.SelectedItem
            };
            
            Close();
        }
        
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
            for (int ix = 0; ix < checkedListBox1.Items.Count; ++ix)
                if (ix != e.Index) checkedListBox1.SetItemChecked(ix, false);
        }
    }
}