using System.Windows.Forms;
using DataBaseManager.Data;

namespace DataBaseManager.Presentation
{
    public partial class PropControl : UserControl
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int ParentPropertyId { get; set; }
        
        public PropertyType PropertyType { get; set; }
        
        public PropControl()
        {
            InitializeComponent();
        }
    }
}
