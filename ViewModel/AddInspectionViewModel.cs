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
        private string _inspector;
        private string _comment;
        private ObservableCollection<Remark> _remarks;
        public event Action RequestClose; // Событие для закрытия окна AddInspection
        public event Action UpdateGrid; // Событие для обновления DataGrid в главном окне
        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        
        //Список инспекторов
        public ObservableCollection<string> Inspectors { get; set; } = new ObservableCollection<string>
        {
            "Контрабандистович А.У (88005553535)",
            "Пограничников Г.Г (123987456)",
            "Оружников П.П (0987654321)"
        };
        public AddInspectionViewModel()
        {
            Inspector = Inspectors[0];
            Date = DateTime.Now; //Для установки текущей даты в DatePicker
            Remarks = new ObservableCollection<Remark>();
            SaveCommand = new RelayCommand(Save);
            CloseCommand = new RelayCommand(Close);
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
        public string Inspector
        {
            get => _inspector;
            set
            {
                _inspector = value;
                OnPropertyChanged(nameof(Inspector));
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



        public bool ValidateInspection(out Inspection inspection, out List<string> errorMessages)
        {
            var listErrors = new List<string>();
            bool isValid = true;

            inspection = new Inspection
            {
                Name = Name,
                Date = Date,
                Inspector = Inspector,
                Comment = Comment,
                Remarks = Remarks.ToList()
            };

            var validationContextInspection = new ValidationContext(inspection);
            var validationResultsInspection = new List<ValidationResult>();

            // Проверяем валидность объекта
            Validator.TryValidateObject(inspection, validationContextInspection, validationResultsInspection);

            // Собираем сообщения об ошибках
            listErrors.AddRange(validationResultsInspection.Select(x => x.ErrorMessage).ToList());

            for (int i = 0; i < Remarks.Count; i++)
            {
                var validationResultsRemarks = new List<ValidationResult>();
                var validationContextRemarks = new ValidationContext(Remarks[i]);
                Validator.TryValidateObject(Remarks[i], validationContextRemarks, validationResultsRemarks);
                listErrors.AddRange(validationResultsRemarks.Select(x => $"Ошибка в {i + 1} замечании: " + x.ErrorMessage).ToList());
            }

            if (listErrors.Count > 0)
                isValid = false;

            errorMessages = listErrors;
            return isValid;
        }


        private void Save()
        {
            try
            {
                // Валидируем данные перед созданием объекта
                if (!ValidateInspection(out var inspection, out var errorMessages))
                {
                    MessageBox.Show($"Проверьте поля на ошибки:\n{string.Join("\n", errorMessages)}", "Ошибка в данных!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var context = new SoftMarinDbContext())
                {
                    context.Inspections.Add(inspection);
                    context.SaveChanges();
                }
                UpdateGrid?.Invoke(); // Сообщаем главному окну обновить DataGrid
                MessageBox.Show("Инспекция успешно сохранена!","Отлично!", MessageBoxButton.OK, MessageBoxImage.Information);
                RequestClose.Invoke();
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
