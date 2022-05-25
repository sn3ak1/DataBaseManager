using System.Text;
using System.Windows.Forms;
using DataBaseManager.Business;
using DataBaseManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataBaseManager.Presentation
{
    public partial class FormView : Form
    {
        public FormView()
        {
            InitializeComponent();
            textBox1.Text = Controller.PrintData();
        }
        
        
    }
}