using ProjectManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Array Priorities { get; set; }

        [Required(ErrorMessage = "Название задачи обязательно для заполнения")]
        [Display(Name = "Название задачи")]
        public string Name { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Данные об авторе задачи обязательны для заполнения")]
        [Display(Name = "Автор задачи")]
        public int AutorId { get; set; }
        [Display(Name = "Исполнитель задачи")]
        public int ExecutorId { get; set; }
        [Display(Name = "Приоритет")]
        public int PriorityId { get; set; }
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
    }
}
