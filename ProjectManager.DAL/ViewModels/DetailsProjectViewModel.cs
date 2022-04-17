using ProjectManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.ViewModels
{
    public class DetailsProjectViewModel
    {
        public Project Project { get; set; }
        public List<Employee> Executors { get; set; }
        public List<Employee> FreeEmployees { get; set; }
    }
}
