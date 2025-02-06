using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftMarine
{
    public class Inspection
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название инспекции обязательно для заполнения.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата должна быть указана.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Инспектор должен быть указан.")]
        public string Inspector { get; set; }
        public int Count { get; set; }
        public string Comment { get; set; }

        // Навигационное свойство для связи с Remark
        public virtual ICollection<Remark> Remarks { get; set; } = new List<Remark>();
    }
}
