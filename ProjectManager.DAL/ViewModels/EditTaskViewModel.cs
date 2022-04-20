using ProjectManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.ViewModels
{
    public class EditTaskViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название задачи обязательно для заполнения")]
        [Display(Name = "Название задачи")]
        public string Name { get; set; }
        [Display(Name = "Исполнитель задачи")]
        public int? ExecutorId { get; set; }
        [Display(Name = "Приоритет")]
        public int PriorityId { get; set; }
        [Display(Name = "Статус")]
        public int StatusId { get; set; }
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        public Array Priorities { get; set; }
        public Array Statuses { get; set; }
        public List<Employee> Executors { get; set; }
        public Employee Executor { get; set; }
    }
}

