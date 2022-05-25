using System;
using System.Linq;
using System.Windows.Forms;
using DataBaseManager.Business;
using DataBaseManager.Data;

namespace DataBaseManager.Presentation
{
    public sealed class CategoryTreeNode: TreeNode
    {
        public Category Category { get; }

        public CategoryTreeNode(Category category): base(Controller.getCategoryString(category))
        {
            Category = category;
        }
    }
}