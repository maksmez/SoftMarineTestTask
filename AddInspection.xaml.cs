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

namespace SoftMarine
{
    /// <summary>
    /// Логика взаимодействия для AddInspection.xaml
    /// </summary>
    public partial class AddInspection : Window
    {
        public AddInspection()
        {
            InitializeComponent();

            if (DataContext is AddInspectionViewModel vm)
            {
                vm.RequestClose += () => this.Close();
            }
        }
    }
}
