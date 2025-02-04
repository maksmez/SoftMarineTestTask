using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SoftMarine
{
    public class AllInspectionsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Inspection> _inspections;
        private Inspection _selectedInspection;
        private readonly INavigationService _navigationService;

        public ICommand OpenAddInspectionCommand { get; } //Команда для открытия окна добавления инспекии
        public ICommand DeleteInspectionCommand { get; } // Команда для удаления



        public AllInspectionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            OpenAddInspectionCommand = new RelayCommand(OpenAddInspectionWindow);
            DeleteInspectionCommand = new RelayCommand(DeleteInspection, CanDeleteInspection);
            Inspections = new ObservableCollection<Inspection>();
            LoadInspections(); // Загружаем данные при создании ViewModel
        }

        public Inspection SelectedInspection
        {
            get => _selectedInspection;
            set
            {
                _selectedInspection = value;
                OnPropertyChanged(nameof(SelectedInspection));

                // Обновляем доступность кнопки удаления
                (DeleteInspectionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private void DeleteInspection()
        {
            if (SelectedInspection == null) return;

            var result = MessageBox.Show($"Удалить инспекцию \"{SelectedInspection.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new SoftMarinDbContext())
                {
                    var inspectionToDelete = context.Inspections.Find(SelectedInspection.Id);
                    if (inspectionToDelete != null)
                    {
                        context.Inspections.Remove(inspectionToDelete);
                        context.SaveChanges();
                    }
                }

                // Обновляем список инспекций
                LoadInspections();
            }
        }

        private bool CanDeleteInspection()
        {
            return SelectedInspection != null;
        }


        private void OpenAddInspectionWindow()
        {
            _navigationService.OpenAddInspectionWindow(LoadInspections);
        }


        public ObservableCollection<Inspection> Inspections
        {
            get => _inspections;
            set
            {
                _inspections = value;
                OnPropertyChanged(nameof(Inspections));
            }
        }

        // Метод для загрузки инспекций из базы данных
        public void LoadInspections()
        {
            using (var context = new SoftMarinDbContext())
            {
                // Загружаем инспекции из базы данных
                var inspections = context.Inspections
                    .Include(x=>x.Remarks) // Если нужно загрузить связанные замечания
                    .ToList();

                // Очищаем коллекцию и добавляем загруженные данные
                Inspections.Clear();
                foreach (var inspection in inspections)
                {
                    Inspections.Add(inspection);
                }
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
