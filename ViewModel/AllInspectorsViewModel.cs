using SoftMarine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace SoftMarine.ViewModel
{
    public class AllInspectorsViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _number;
        private Inspector _selectedInspector;
        private ObservableCollection<Inspector> _inspectors;
        public event Action UpdateCombobox; // Событие для обновления ComboBox с инспекторами в главном окне
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }


        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                IsModified = true;

            }
        }

        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
                IsModified = true;

            }
        }

        public Inspector SelectedInspector
        {
            get => _selectedInspector;
            set
            {
                _selectedInspector = value;
                OnPropertyChanged(nameof(SelectedInspector));
                IsModified = true;

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

        public AllInspectorsViewModel()
        {
            Inspectors = InspectorService.GetInspectors();
            SaveCommand = new RelayCommand(Save);
            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new RelayCommand(Edit);

        }

        public bool IsModified { get; private set; } = false; // Флаг изменений

        private void Edit()
        {
            if (!IsModified)
                return;
            var result = MessageBox.Show($"Сохранить изменения для инспектора \"{SelectedInspector.Name}\"?", "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (SelectedInspector == null || result == MessageBoxResult.No)
            {
                Inspectors = InspectorService.GetInspectors();
                return;
            }

            try
            {
                using (var context = new SoftMarinDbContext())
                {
                    var inspectorDB = context.Inspectors.Find(SelectedInspector.Id);
                    if (inspectorDB != null)
                    {
                        inspectorDB.Name = SelectedInspector.Name;
                        inspectorDB.Number = SelectedInspector.Number;
                        context.SaveChanges();
                    }
                }
                UpdateCombobox.Invoke();
                MessageBox.Show("Изменения сохранены!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Delete()
        {
            try
            {
                if (SelectedInspector == null) return;

                var result = MessageBox.Show($"Удалить инспектора? \"{SelectedInspector.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new SoftMarinDbContext())
                    {
                        var inspectorDB = context.Inspectors.Find(SelectedInspector.Id);
                        if (inspectorDB != null)
                        {
                            context.Inspectors.Remove(inspectorDB);
                            context.SaveChanges();
                        }
                    }
                    // Обновляем список инспекций
                    Inspectors = InspectorService.GetInspectors();
                    UpdateCombobox.Invoke();
                }
            }
            catch (DbUpdateException) 
            {
                MessageBox.Show($"У этого инспектора есть инспекции!\nУдаление невозможно!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Save()
        {
            try
            {
                var inspector = new Inspector()
                {
                    Name = Name,
                    Number = Number,
                };

                if (!Validator.ValidateInspector(inspector, out var errorMessages))
                {
                    MessageBox.Show($"Проверьте поля на ошибки:\n{string.Join("\n", errorMessages)}", "Ошибка в данных!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var context = new SoftMarinDbContext())
                {
                    context.Inspectors.Add(inspector);
                    context.SaveChanges();
                }

                MessageBox.Show("Изменения сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                // Обновляем список инспекций
                Inspectors = InspectorService.GetInspectors();
                Name = "";
                Number = "";
                UpdateCombobox.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
