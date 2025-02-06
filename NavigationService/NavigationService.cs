using SoftMarine.View;
using SoftMarine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftMarine
{
    public class NavigationService : INavigationService
    {
        public void OpenAddInspectionWindow(Action updateMainGrid)
        {
            var addInspectionVM = new AddInspectionViewModel();
            var addInspectionWindow = new AddInspection { DataContext = addInspectionVM };
            addInspectionWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            // Закрываем окно при завершении операции
            addInspectionVM.RequestClose += () =>
            {
                addInspectionWindow.Close();
                updateMainGrid?.Invoke(); // Обновляем список в главном окне
            };

            addInspectionWindow.ShowDialog();
        }

        public void OpenEditInspectionWindow(Inspection inspection,Action updateMainGrid)
        {
            var editInspectionVM = new EditInspectionViewModel(inspection);
            var editInspectionWindow = new EditInspection { DataContext = editInspectionVM };
            editInspectionWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            // Закрываем окно при завершении операции
            editInspectionVM.RequestClose += () =>
            {
                editInspectionWindow.Close();
                updateMainGrid?.Invoke(); // Обновляем список в главном окне
            };

            editInspectionWindow.ShowDialog();
        }
    }
}
