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
    }
}
