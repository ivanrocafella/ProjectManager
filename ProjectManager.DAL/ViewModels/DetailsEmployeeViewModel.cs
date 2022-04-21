using ProjectManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.ViewModels
{
    public class DetailsEmployeeViewModel
    {
        public Employee Employee { get; set; }
        public List<Project> PinnedProjects { get; set; }
        public List<Project> ManagedProjects { get; set; }
        public List<Project> FreeProjects { get; set; }
    }
}
