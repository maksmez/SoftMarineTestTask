using SoftMarine.Model;
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
using System.Windows.Threading;

namespace SoftMarine
{
    public class AllInspectionsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Inspection> _inspections;
        private ObservableCollection<Inspector> _inspectors;
        private Inspection _selectedInspection;
        private Inspector _selectedInspector;
        private string _searchText;
        private DispatcherTimer _filterTimer; // Таймер для задержки поиска при вводе текста

        private readonly INavigationService _navigationService;

        public ICommand OpenAddInspectionCommand { get; } //Команда для открытия окна добавления инспекии
        public ICommand OpenEditInspectionCommand { get; } // Команда для открытия окна редактирования инспекции
        public ICommand OpenAllInspectorsCommand {  get; } //Команда для открытия окна с инспекторами
        public ICommand DeleteInspectionCommand { get; } // Команда для удаления
        public ICommand FilterByInspectorCommand {  get; } //Команда для фильтрации
        public ObservableCollection<Inspection> Inspections
        {
            get => _inspections;
            set
            {
                _inspections = value;
                OnPropertyChanged(nameof(Inspections));
            }
        }

        public ObservableCollection<Inspector> Inspectors
        {
            get => _inspectors;
            set
            {
                _inspectors = value;
                OnPropertyChanged(nameof(Inspectors));
            }
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
                // Обновляем доступность кнопки редактирования
                (OpenEditInspectionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        public Inspector SelectedInspector
        {
            get => _selectedInspector;
            set
            {
                _selectedInspector = value;
                //Выполняем фильтрацию при выборе инспектора
                GetInspections();
                OnPropertyChanged(nameof(SelectedInspector));
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                _filterTimer.Stop();
                _filterTimer.Start(); // Запускаем таймер при изменении текста
                OnPropertyChanged(nameof(SelectedInspector));
            }
        }
        public AllInspectionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            OpenAddInspectionCommand = new RelayCommand(OpenAddInspectionWindow);
            DeleteInspectionCommand = new RelayCommand(DeleteInspection, IsSelectInspection);
            OpenEditInspectionCommand = new RelayCommand(OpenEditInspectionWindow, IsSelectInspection);
            FilterByInspectorCommand = new RelayCommand(GetInspections);
            OpenAllInspectorsCommand = new RelayCommand(OpenAllInspectorsWindow);
           
            Inspections = new ObservableCollection<Inspection>();
            GetInspections(); // Загружаем данные при создании ViewModel
            GetInspectors();


            // Таймер для задержки поиска при вводе текста. Выполняем фильтрацию только после 300 мс
            _filterTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            _filterTimer.Tick += (s, e) =>
            {
                _filterTimer.Stop();
                GetInspections();
            };
        }
        public void GetInspectors()
        {
            Inspectors = InspectorService.GetInspectors();
            // Создаем фиктивный объект "Все инспекторы"
            var allInspectorsOption = new Inspector { Id = 0, Name = "Все инспекторы" };
            // Добавляем в начало списка
            Inspectors.Insert(0, allInspectorsOption);
            SelectedInspector = allInspectorsOption;
        }
        private void OpenAllInspectorsWindow()
        {
            _navigationService.OpenAllInspectorsWindow(GetInspectors);
        }
        
        public void GetInspections()
        {
            try
            {
                using (var context = new SoftMarinDbContext())
                {
                    IEnumerable<Inspection> FilteredInspections = context.Inspections
                     .Include(x => x.Remarks)
                     .Include(i => i.Inspector)
                     .AsEnumerable() // Фильтрация выполняется в памяти, если данных много, нужно переработать
                     .Where(i =>
                         (SelectedInspector == null || SelectedInspector.Id == 0 || 
                         (i.Inspector != null && i.Inspector.Id == SelectedInspector.Id)) &&
                         (string.IsNullOrEmpty(SearchText) || i.Name.Contains(SearchText))
                     );


                    // Очищаем коллекцию и добавляем загруженные данные
                    Inspections.Clear();
                    foreach (var inspection in FilteredInspections)
                    {
                        Inspections.Add(inspection);
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Произошла ошибка!\n{ex}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void OpenEditInspectionWindow()
        {
            _navigationService.OpenEditInspectionWindow(SelectedInspection, GetInspections);
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
                GetInspections();
            }
        }

        private bool IsSelectInspection()
        {
            return SelectedInspection != null;
        }

        private void OpenAddInspectionWindow()
        {
            _navigationService.OpenAddInspectionWindow(GetInspections);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
