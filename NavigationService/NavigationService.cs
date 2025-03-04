using SoftMarine.View;
using SoftMarine.ViewModel;
using System;

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

        public void OpenEditInspectionWindow(Inspection inspection, Action updateMainGrid)
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

        public void OpenAllInspectorsWindow(Action updateInspectors)
        {
            var openInspectorsVM = new AllInspectorsViewModel();
            var openInspectorsWindow = new AllInspectors { DataContext = openInspectorsVM };
            openInspectorsWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            
            // Подписываемся на событие изменение инспекторов
            openInspectorsVM.UpdateCombobox += () =>
            {
                updateInspectors?.Invoke(); // Обновляем список инспекторов в главном окне
            };

            openInspectorsWindow.ShowDialog();
        }
    }
}
