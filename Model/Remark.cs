using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftMarine
{
    public class Remark
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Текст замечания должен быть указан.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Дата замечания должна быть указана.")]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Comment { get; set; }

        // Внешний ключ для связи с Inspection
        public int InspectionId { get; set; }
        // Навигационное свойство для связи с Inspection
        public virtual Inspection Inspection { get; set; }
    }
}
