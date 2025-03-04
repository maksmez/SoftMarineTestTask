using SoftMarine.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SoftMarine.ViewModel
{
    public class EditInspectionViewModel : INotifyPropertyChanged
    {
        private string _name;
        private DateTime _date;
        private string _comment;
        private Remark _selectedRemark;
        private Inspector _selectedInspector;

        private ObservableCollection<Remark> _remarks;
        private ObservableCollection<Inspector> _inspectors;


        public event Action UpdateGrid; // Событие для обновления DataGrid в главном окне
        public event Action RequestClose;
        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand DeleteRemarkCommand { get; }


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
        public Remark SelectedRemark
        {
            get => _selectedRemark;
            set
            {
                _selectedRemark = value;
                OnPropertyChanged(nameof(SelectedRemark));
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
        public EditInspectionViewModel(Inspection inspection)
        {
            Name = inspection.Name;
            Date = inspection.Date;
            Comment = inspection.Comment;
            Remarks = new ObservableCollection<Remark>(inspection.Remarks);
            Inspectors = InspectorService.GetInspectors();
            SelectedInspector = Inspectors.FirstOrDefault(i => i.Id == inspection.Inspector.Id);

            SaveCommand = new RelayCommand(() => Save(inspection));
            DeleteRemarkCommand = new RelayCommand(() => DeleteRemarkExecute(SelectedRemark));

            CloseCommand = new RelayCommand(Close);
        }
        private void DeleteRemarkExecute(Remark remark)
        {
          Remarks.Remove(remark);
        }

        private void Save(Inspection inspection)
        {
            var inspection_form = new Inspection
            {
                Name = Name,
                Date = Date,
                InspectorId = SelectedInspector.Id,
                Comment = Comment,
                Remarks = Remarks.ToList()
            };
            if (!Validator.ValidateInspection(inspection_form, out var errorMessages))
            {
                MessageBox.Show($"Проверьте поля на ошибки:\n{string.Join("\n", errorMessages)}", "Ошибка в данных!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new SoftMarinDbContext())
            {
                var inspectionToUpdate = context.Inspections.Include("Remarks").FirstOrDefault(i => i.Id == inspection.Id);
                if (inspectionToUpdate != null)
                {
                    inspectionToUpdate.Name = Name;
                    inspectionToUpdate.Date = Date;
                    inspectionToUpdate.InspectorId = SelectedInspector.Id;
                    inspectionToUpdate.Comment = Comment;

                    // Удаляем замечания, которых больше нет
                    var remarksToRemove = inspectionToUpdate.Remarks
                        .Where(r => !Remarks.Any(newR => newR.Id == r.Id))
                        .ToList();
                    context.Remarks.RemoveRange(remarksToRemove);

                    // Обновляем существующие замечания
                    foreach (var remark in Remarks.Where(r => r.Id != 0)) // Только те, что уже в БД
                    {
                        var existingRemark = inspectionToUpdate.Remarks.FirstOrDefault(r => r.Id == remark.Id);
                        if (existingRemark != null)
                        {
                            existingRemark.Text = remark.Text;
                            existingRemark.Date = remark.Date;
                            existingRemark.Comment = remark.Comment;
                        }
                    }

                    // Добавляем новые замечания в `context.Remarks`
                    var newRemarks = Remarks.Where(r => r.Id == 0)
                        .Select(r => new Remark
                        {
                            Text = r.Text,
                            Date = r.Date,
                            Comment = r.Comment,
                            InspectionId = inspectionToUpdate.Id
                        }).ToList();

                    if (newRemarks.Any())
                    {
                        context.Remarks.AddRange(newRemarks);
                    }

                    context.SaveChanges();
                }
            }

            MessageBox.Show($"Инспекция изменена!", "Изменение инспекции", MessageBoxButton.OK, MessageBoxImage.Information);

            RequestClose?.Invoke();
        }


        private void Close()
        {
            RequestClose?.Invoke();
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
