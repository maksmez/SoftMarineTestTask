using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftMarine.Model
{
    public class Inspector
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя инспектора обязательно для заполнения.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Номер инспектора обязательно для заполнения.")]
        public string Number { get; set; }

        // Навигационное свойство - список инспекций
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}
