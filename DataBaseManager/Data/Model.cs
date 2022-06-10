using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Forms;

namespace DataBaseManager.Data
{

    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Admin { get; set; }
        public bool CanAddCategories { get; set; }
        public Category ModifiableCategory { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
    
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
    
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswdHash {get; set; }
        public Role Role { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("ParentId")]
        public int ParentId { get; set; }
        
        public Category Category { get; set; }
    }

    public class IntProperty: Property
    {
        public int Value { get; set; }
    }

    public class StringProperty: Property
    {
        public string Value { get; set; }
    }

    public class EnumFlag : Property
    {
        public EnumProperty EnumProperty { get; set; }
    }

    public class EnumProperty: Property
    {
        public IEnumerable<EnumFlag> Flags { get; set; }
        public string Value { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }
        public IEnumerable<IntProperty> IntProperties { get; set; }
        public IEnumerable<StringProperty> StringProperties { get; set; }
        public IEnumerable<EnumProperty> EnumProperties { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }

    public enum PropertyType
    {
        Int,
        String,
        Enum
    }
}