using SoftMarine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SoftMarine
{
    public class AddInspectionViewModel : INotifyPropertyChanged
    {
        private string _name;
        private DateTime _date;
        private string _comment;
        private ObservableCollection<Remark> _remarks;
        private ObservableCollection<Inspector> _inspectors;
        private Inspector _selectedInspector;

        public event Action RequestClose; // Событие для закрытия окна AddInspection
        public event Action UpdateGrid; // Событие для обновления DataGrid в главном окне
        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        public AddInspectionViewModel()
        {
            Date = DateTime.Now; //Для установки текущей даты в DatePicker
            Remarks = new ObservableCollection<Remark>();
            SaveCommand = new RelayCommand(Save);
            CloseCommand = new RelayCommand(Close);
            Inspectors = InspectorService.GetInspectors();
            SelectedInspector = Inspectors.FirstOrDefault();


        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public ObservableCollection<Remark> Remarks
        {
            get => _remarks;
            set
            {
                _remarks = value;
                OnPropertyChanged(nameof(Remarks));
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

        public Inspector SelectedInspector
        {
            get => _selectedInspector;
            set
            {
                _selectedInspector = value;
                OnPropertyChanged(nameof(SelectedInspector));
            }
        }
        private void Save()
        {
            try
            {
                var inspection = new Inspection
                {
                    Name = Name,
                    Date = Date,
                    InspectorId = SelectedInspector.Id,
                    Comment = Comment,
                    Remarks = Remarks.ToList()
                };

                if (!Validator.ValidateInspection(inspection, out var errorMessages))
                {
                    MessageBox.Show($"Проверьте поля на ошибки:\n{string.Join("\n", errorMessages)}", "Ошибка в данных!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var context = new SoftMarinDbContext())
                {
                    context.Inspections.Add(inspection);
                    context.SaveChanges();
                }

                UpdateGrid?.Invoke();
                MessageBox.Show($"Инспекция создана!", "Содание инспекции", MessageBoxButton.OK, MessageBoxImage.Information);

                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Close()
        {
            RequestClose.Invoke();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        
    }
}
