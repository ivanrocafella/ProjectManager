using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Core.Entities
{
    public partial class Employee : Entity
    {
        public Employee()
        {
            EmployeeProjects = new HashSet<EmployeeProject>();
            Projects = new HashSet<Project>();
            CreatedTasks = new HashSet<Task>();
            ExecutedTasks = new HashSet<Task>();
        }

        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string PatronymicName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> CreatedTasks { get; set; }
        public virtual ICollection<Task> ExecutedTasks { get; set; }
    }
}
