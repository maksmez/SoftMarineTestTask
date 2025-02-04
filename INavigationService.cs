using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftMarine
{
    public interface INavigationService
    {
        void OpenAddInspectionWindow(Action updateMainGrid);
    }
}
