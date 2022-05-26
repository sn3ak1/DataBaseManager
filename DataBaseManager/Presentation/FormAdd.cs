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
        private readonly List<PropControl> _intPropControls = new();
        private readonly List<PropControl> _stringPropControls = new();
        private readonly List<PropControl> _enumPropControls = new();
        
        private readonly List<PropControl> _propertiesToRemove = new();

        private Category _parent;
        public Category Category { get; set; }
        
        private int _top = 1;

        public FormAdd(Category parent, Category edited = null)
        {
            InitializeComponent();
            _parent = parent;
            Category = edited;
            if (edited != null)
                textBox1.Text = edited.Name;

            if(parent!=null&&edited==null)
            {
                foreach (var property in parent.IntProperties)
                {
                    CreateAndAddPropControl(property, _intPropControls, "Integer",true);
                }

                foreach (var property in parent.StringProperties)
                {
                    CreateAndAddPropControl(property, _stringPropControls, "String",true);
                }

                foreach (var property in parent.EnumProperties)
                {
                    CreateAndAddPropControl(property, _enumPropControls, "Enum",true);
                }
            }

            if (edited == null) return;
            foreach (var property in edited.IntProperties)
            {
                CreateAndAddPropControl(property, _intPropControls, "Integer");
            }
            
            foreach (var property in edited.StringProperties)
            {
                CreateAndAddPropControl(property, _stringPropControls, "String");
            }
            
            foreach (var property in edited.EnumProperties)
            {
                CreateAndAddPropControl(property, _enumPropControls, "Enum");
            }
        }

        private void CreateAndAddPropControl(Property property, ICollection<PropControl> propControls, 
            string placeholder, bool fromParent=false)
        {
            var propControl = new PropControl()
            {
                PropertyId = fromParent? 0: property.Id,
                ParentPropertyId = fromParent? property.Id: property.ParentId
            };
            propControls.Add(propControl);
            propControl.textBox1.Text = property.Name;
            propControl.textBox2.PlaceholderText = placeholder+" value";
            // propControl.textBox1.ReadOnly = fromParent || property.ParentId!=0;
            // propControl.button1.Enabled = !(fromParent || property.ParentId != 0);
            if(!fromParent)
            {
                switch (placeholder)
                {
                    case "Integer":
                        propControl.PropertyType = PropertyType.Int;
                        propControl.textBox2.Text =
                            Category.IntProperties.First(x => x.Id == property.Id).Value.ToString();
                        break;
                    case "String":
                        propControl.PropertyType = PropertyType.String;
                        propControl.textBox2.Text =
                            Category.StringProperties.First(x => x.Id == property.Id).Value.ToString();
                        break;
                    case "Enum":
                        propControl.PropertyType = PropertyType.Enum;
                        propControl.textBox2.Text =
                            Category.EnumProperties.First(x => x.Id == property.Id).Value.ToString();
                        break;
                    default:
                        propControl.textBox2.Text = null;
                        break;
                }
            }
            AddPropControl(propControl);
        }

        private void button_remove_property_clicked(object sender, System.EventArgs e)
        {
            var propControl = (PropControl) ((Button) sender).Parent;
            Controls.Remove(propControl);
            _top--;
            panel1.Top -= 40;
            Height -= 40;
            foreach (var control in _intPropControls.Concat(_stringPropControls).Concat(_enumPropControls))
            {
                if (control.Id <= propControl.Id) continue;
                control.Id--;
                control.Top -= 40;
            }
            _propertiesToRemove.Add(propControl);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            PropControl propControl = new PropControl();
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Integer":
                    propControl.PropertyType = PropertyType.Int;
                    _intPropControls.Add(propControl);
                    propControl.textBox2.PlaceholderText = "Integer value";
                    break;
                case "String":
                    propControl.PropertyType = PropertyType.String;
                    _stringPropControls.Add(propControl);
                    propControl.textBox2.PlaceholderText = "String value";
                    break;
                case "Enum":
                    propControl.PropertyType = PropertyType.Enum;
                    _enumPropControls.Add(propControl);
                    propControl.textBox2.PlaceholderText = "Enum value";
                    break;
            }

            AddPropControl(propControl);
        }

        private void AddPropControl(PropControl propControl)
        {
            Controls.Add(propControl);
            propControl.Id = _top;
            propControl.button1.Click += button_remove_property_clicked;
            propControl.Top = 40 * _top + 10;
            propControl.Left = 12;
            panel1.Top += 40;
            comboBox1.SelectedItem = null;
            button1.Enabled = false;
            _top++;
            if (Height <= 40 * _top + 60)
                Height += 40;
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            foreach (var propControl in _propertiesToRemove)
            {
                switch (propControl.PropertyType)
                {
                    case PropertyType.Int:
                        _intPropControls.Remove(propControl);
                        break;
                    case PropertyType.String:
                        _stringPropControls.Remove(propControl);
                        break;
                    case PropertyType.Enum:
                        _enumPropControls.Remove(propControl);
                        break;
                }
                if(propControl.PropertyId!=0)
                    Controller.RemoveProperty(propControl.PropertyId, propControl.PropertyType, Category);
            }
            
            var id = Category?.Id ?? 0;
            Category ??= new Category()
            {
                Id = id,
                Parent = _parent,
                Name = textBox1.Text
            }; 
            Category.Name = textBox1.Text;
            Category.IntProperties = _intPropControls.Select(x => new IntProperty
                {Id = x.PropertyId,Name = x.textBox1.Text, Value = int.Parse(x.textBox2.Text), ParentId = x.ParentPropertyId})
                .ToArray();
            Category.StringProperties = _stringPropControls.Select(x => new StringProperty
                {Id = x.PropertyId,Name = x.textBox1.Text, Value = x.textBox2.Text, ParentId = x.ParentPropertyId})
                .ToArray();
            Category.EnumProperties = _enumPropControls.Select(x => new EnumProperty
                {Id = x.PropertyId,Name = x.textBox1.Text, Value = x.textBox2.Text, ParentId = x.ParentPropertyId})
                .ToArray();
            if(id!=0)
                Controller.EditCategory(Category);
            else
                Controller.AddCategory(Category);
            Close();
        }
    }
}