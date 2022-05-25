using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataBaseManager.Business;
using DataBaseManager.Data;

namespace DataBaseManager.Presentation
{
    public partial class FormAdd : Form
    {
        private List<PropControl> _intPropControls = new List<PropControl>();
        private List<PropControl> _stringPropControls = new List<PropControl>();
        private List<PropControl> _enumPropControls = new List<PropControl>();

        private Category _parent;
        public Category Category { get; set; }
        
        public FormAdd(Category parent)
        {
            InitializeComponent();
            _parent = parent;
        }
        

        private int _top = 1;
        private void button1_Click(object sender, System.EventArgs e)
        {
            PropControl propControl = new PropControl();
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Integer":
                    _intPropControls.Add(propControl);
                    propControl.textBox2.PlaceholderText = "Integer value";
                    break;
                case "String":
                    _stringPropControls.Add(propControl);
                    propControl.textBox2.PlaceholderText = "String value";
                    break;
                case "Enum":
                    _enumPropControls.Add(propControl);
                    propControl.textBox2.PlaceholderText = "Enum value";
                    break;
            }

            this.Controls.Add(propControl);
            propControl.Top = 40 * _top;
            propControl.Left = 12;
            panel1.Top += 40;
            comboBox1.SelectedItem = null;
            button1.Enabled = false;
            _top++;
            if (this.Height <= 40 * _top + 60)
                this.Height += 40;
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Category = new Category()
            {
                Parent = _parent,
                Name = textBox1.Text,
                IntProperties = _intPropControls.Select(x => 
                    new IntProperty(){Name = x.textBox1.Text, Value = Int32.Parse(x.textBox2.Text)}).ToArray(),
                StringProperties = _stringPropControls.Select(x => 
                    new StringProperty(){Name = x.textBox1.Text, Value = x.textBox2.Text}).ToArray(),
                EnumProperties = _enumPropControls.Select(x => 
                    new EnumProperty(){Name = x.textBox1.Text,Value = x.textBox2.Text}).ToArray()
            };
            Controller.AddCategory(Category);
            Close();
        }
    }
}