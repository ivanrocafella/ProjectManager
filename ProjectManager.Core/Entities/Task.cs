using ProjectManager.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Core.Entities
{
    public class Task : Entity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public int AutorId { get; set; }
        [InverseProperty("CreatedTasks")]
        public Employee Autor { get; set; }
        public int? ExecutorId { get; set; }
        [InverseProperty("ExecutedTasks")]
        public Employee Executor { get; set; }
        [Required]
        public virtual int PriorityId
        {
            get => (int)this.Priority;
            set => Priority = (Priority)value;
        }
        [EnumDataType(typeof(Priority))]
        public Priority Priority { get; set; }
        [Required]
        public virtual int StatusId
        {
            get => (int)this.Status;
            set => Status = (Status)value;
        }
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
    