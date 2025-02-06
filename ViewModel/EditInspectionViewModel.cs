﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace SoftMarine.ViewModel
{
    public class EditInspectionViewModel : INotifyPropertyChanged
    {
        private string _name;
        private DateTime _date;
        private string _inspector;
        private string _comment;
        private ObservableCollection<Remark> _remarks;
        public event Action UpdateGrid; // Событие для обновления DataGrid в главном окне
        public event Action RequestClose;

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }


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

        //Список инспекторов
        public ObservableCollection<string> Inspectors { get; set; } = new ObservableCollection<string>
        {
            "Контрабандистович А.У (88005553535)",
            "Пограничников Г.Г (123987456)",
            "Оружников П.П (0987654321)"
        };

        public EditInspectionViewModel(Inspection inspection)
        {
            Name = inspection.Name;
            Date = inspection.Date;
            Inspector = inspection.Inspector;
            Comment = inspection.Comment;
            Remarks = new ObservableCollection<Remark>(inspection.Remarks);
            SaveCommand = new RelayCommand(() => Save(inspection));
            CloseCommand = new RelayCommand(Close);
        }


        private void Save(Inspection inspection)
        {
            using (var context = new SoftMarinDbContext())
            {
                var inspectionToUpdate = context.Inspections.Find(inspection.Id);
                if (inspectionToUpdate != null)
                {
                    inspectionToUpdate.Name = Name;
                    inspectionToUpdate.Date = Date;
                    inspectionToUpdate.Inspector = Inspector;
                    inspectionToUpdate.Comment = Comment;
                    // Удаляем старые замечания из базы
                    var remarksToRemove = inspectionToUpdate.Remarks
                        .Where(r => !Remarks.Any(newR => newR.Id == r.Id))
                        .ToList();
                    context.Remarks.RemoveRange(remarksToRemove);

                    // Обновляем существующие и добавляем новые замечания
                    foreach (var remark in Remarks)
                    {
                        var existingRemark = inspectionToUpdate.Remarks.FirstOrDefault(r => r.Id == remark.Id);
                        if (existingRemark != null)
                        {
                            existingRemark.Text = remark.Text;
                            existingRemark.Date = remark.Date;
                            existingRemark.Comment = remark.Comment;
                        }
                        else
                        {
                            // Добавляем новое замечание, связанное с текущей инспекцией
                            inspectionToUpdate.Remarks.Add(new Remark
                            {
                                Text = remark.Text,
                                Date = remark.Date,
                                Comment = remark.Comment,
                                InspectionId = inspectionToUpdate.Id // Указываем внешний ключ
                            });
                        }
                    }


                    context.SaveChanges();
                }
            }

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
