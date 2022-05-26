using System.Windows.Forms;

namespace DataBaseManager.Presentation
{
    public partial class PropControl : UserControl
    {
        public int PropertyId { get; set; }
        public int ParentPropertyId { get; set; }
        public PropControl()
        {
            InitializeComponent();
        }
    }
}
