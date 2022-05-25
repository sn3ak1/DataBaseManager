using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBaseManager.Business;
using DataBaseManager.Data;

namespace DataBaseManager.Presentation
{
    public partial class Form2 : Form
    {
        private ToolStripMenuItem _addChildCategory;
        private ToolStripMenuItem _addSiblingCategory;
        private ToolStripMenuItem _removeCategory;
        
        public Form2()
        {
            InitializeComponent();
            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.DrawNode += treeView_DrawNode; ;
            treeView1.MouseDown += treeView_MouseDown; ;
            treeView1.ItemHeight = 70;
            
            _addChildCategory = new ToolStripMenuItem("Add child", null, null, "AddC");
            _addSiblingCategory = new ToolStripMenuItem("Add sibling", null, null, "AddS");
            _removeCategory = new ToolStripMenuItem("Remove", null, null, "Remove");
            _addChildCategory.Click += addChildCategory_Click;
            _addSiblingCategory.Click += addSiblingCategory_Click;
            _removeCategory.Click += removeCategory_Click;
            contextMenuStrip1.Items.Add(_addChildCategory);
            contextMenuStrip1.Items.Add(_addSiblingCategory);
            contextMenuStrip1.Items.Add(_removeCategory);
            
            var viewData = new Controller();
            var rootCategory = viewData.GetRootCategory();
            treeView1.Nodes.Add(new CategoryTreeNode(rootCategory));
            
            AddChildrenToTree(rootCategory, (CategoryTreeNode) treeView1.Nodes[0]);
            
            treeView1.ExpandAll();
            
        }
        
        private static void AddChildrenToTree(Category category, CategoryTreeNode node)
        {
            node.Nodes.AddRange(category.Children.Select(x => new CategoryTreeNode(x)).ToArray());
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                AddChildrenToTree(category.Children.ToArray()[i], (CategoryTreeNode) node.Nodes[i]);
            }
        }
        

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var category = ((CategoryTreeNode) treeView1.SelectedNode).Category;
            foreach (ToolStripItem item in contextMenuStrip1.Items)
            {
                item.Enabled = category!=null;
            }
            if(category==null) return;
            if (category.Children!=null && category.Children.Any())
                _removeCategory.Enabled = false;
            if (category.Id != 1) return;
            _addSiblingCategory.Enabled = false;
            _removeCategory.Enabled = false;
        }

        private void removeCategory_Click(object sender, EventArgs e)
        {
            var node = ((CategoryTreeNode) treeView1.SelectedNode);
            Controller.RemoveCategory(node.Category);
            node.Remove();
            treeView1.Refresh();
        }
        
        private void addChildCategory_Click(object sender, EventArgs e)
        {
            var node = ((CategoryTreeNode) treeView1.SelectedNode);
            AddCategory(node, node);
        }
        
        private void addSiblingCategory_Click(object sender, EventArgs e)
        {
            var node = ((CategoryTreeNode) treeView1.SelectedNode);
            AddCategory(node, (CategoryTreeNode) node.Parent);
        }

        private void AddCategory(CategoryTreeNode node, CategoryTreeNode parent)
        {
            var formAdd = new FormAdd(parent.Category);
            formAdd.Closed += (s, args) =>
            {
                var added = ((FormAdd) s)?.Category;
                if (added == null) return;
                parent.Nodes.Add(new CategoryTreeNode(added));
                treeView1.Refresh();
            };
            formAdd.Show();
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            // Draw the background and node text for a selected node.
            e.DrawDefault = false;
            Rectangle eNodeBounds = NodeBounds(e.Node);
            if (eNodeBounds.X==0 && eNodeBounds.Y==0) return;
 
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Node.Bounds); // clear any remains from system
 
            // Draw the background of the selected node. The NodeBounds method makes the highlight rectangle large enough for the text.
            e.Graphics.FillRectangle(((e.State & TreeNodeStates.Selected) != 0) ? SystemBrushes.Highlight : SystemBrushes.HighlightText
                                      , eNodeBounds);
 
            // Draw the node text using  the node font. If the node font has not been set, use the TreeView font.
            e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont??e.Node.TreeView?.Font??((TreeView)sender).Font,
                                  ((e.State & TreeNodeStates.Selected) != 0) ? SystemBrushes.Window : SystemBrushes.WindowText, eNodeBounds);
 
            // If the node has focus, draw the focus rectangle.
            if ((e.State & TreeNodeStates.Focused) != 0) {
                using (Pen focusPen = new Pen(Color.Black)) {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    e.Graphics.DrawRectangle(focusPen, new Rectangle(eNodeBounds.Location, Rectangle.Inflate(eNodeBounds, -1, -1).Size));
                }
            }
        }
 
        // Selects a node that is clicked on its label text.
        private void treeView_MouseDown(object sender, MouseEventArgs e) {
            TreeNode clickedNode = ((TreeView)sender).GetNodeAt(e.X, e.Y);
            if (NodeBounds(clickedNode).Contains(e.X, e.Y)) ((TreeView)sender).SelectedNode = clickedNode;
        }
 
        // Returns the bounds of the specified node label, Possibly multi line.
        private Rectangle NodeBounds(TreeNode node) {
            if (node?.TreeView!=null  && node?.Text!=null && (0<node.Bounds.Location.X || 0<node.Bounds.Location.Y)) {
                // Retrieve a Graphics object from the TreeView handle and use it to calculate the display size of the text.
                using (Graphics g = node.TreeView.CreateGraphics()) {
                    SizeF textSize = g.MeasureString(node.Text, node.NodeFont ?? node.TreeView.Font);
                    return Rectangle.Ceiling(new RectangleF(PointF.Add(node.Bounds.Location
                                                           , new SizeF(0, (node.TreeView.ItemHeight-textSize.Height)/2))
                                           , textSize)); //Centre Y
                }
            } else return node?.Bounds??new Rectangle(); //FallBack to the normal node bounds, and to empty 0,0.
        }
        
    }

}
