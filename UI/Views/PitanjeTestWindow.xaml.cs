using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.ViewModel;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for PitanjeTestWindow.xaml
    /// </summary>
    public partial class PitanjeTestWindow : Window
    {
        public PitanjeTestWindow(bool izmena)
        {
            InitializeComponent();
            this.DataContext = new PitanjeTestWindowViewModel(izmena);
        }
    }
}
