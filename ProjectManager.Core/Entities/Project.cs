using ProjectManager.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Core.Entities
{
    public partial class Project : Entity
    {
        public Project()
        {
            EmployeeProjects = new HashSet<EmployeeProject>();
            Tasks = new HashSet<Task>();
        }

        public string Name { get; set; }
        public string CustomerCompanyName { get; set; }
        public string ExecutorCompanyName { get; set; }
        public int? ProjectManagerId { get; set; }
        public DateTime CreatedAd { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        [Required]
        public virtual int PriorityId
        {
            get => (int)this.Priority;
            set => Priority = (Priority)value;
        }
        [EnumDataType(typeof(Priority))]
        public Priority Priority { get; set; }
        public DateTime PriorityUpdate { get; set; }

        public virtual Employee ProjectManager { get; set; }
        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
