using SoftMarine.Model;
using SoftMarine.ViewModel;
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

namespace SoftMarine.View
{
    /// <summary>
    /// Логика взаимодействия для AllInspectors.xaml
    /// </summary>
    public partial class AllInspectors : Window
    {
        public AllInspectors()
        {
            InitializeComponent();
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //Срабатывает если пользователь отредактировал строку
            if (DataContext is AllInspectorsViewModel viewModel && viewModel.SelectedInspector != null)
            {
                viewModel.EditCommand.Execute(null);
            }
        }
    }
}
