using SoftMarine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftMarine
{
    public class InspectorService
    {
        public static ObservableCollection<Inspector> GetInspectors()
        {
            using (var context = new SoftMarinDbContext())
            {
                return new ObservableCollection<Inspector>(context.Inspectors.Include(x=>x.Inspections).ToList());
            }
        }
    }

}
