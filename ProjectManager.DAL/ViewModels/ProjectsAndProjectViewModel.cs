using ProjectManager.Core.Entities;
using ProjectManager.DAL.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjectManager.DAL.ViewModels
{
    public class ProjectsAndProjectViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Employee> Employees { get; set; }
        public Array Priorities { get; set; }

        [Required(ErrorMessage = "Название проекта обязательно для заполнения")]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Название компании-заказчика обязательно для заполнения")]
        [Display(Name = "Название компании-заказчика")]
        public string CustomerCompanyName { get; set; }
        [Required(ErrorMessage = "Название компании-исполнителя обязательно для заполнения")]
        [Display(Name = "Название компании-исполнителя")]
        public string ExecutorCompanyName { get; set; }
        [Display(Name = "Руководитель проекта")]
        public int ProjectManagerId { get; set; }
        [Display(Name = "Приоритет")]
        public int PriorityId { get; set; }
        [CheckDate(ErrorMessage = "Некорректо введена дата")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата начала")]
        public DateTime DateStart { get; set; }
        [Display(Name = "Дата завершения")]
        [CheckDate(ErrorMessage = "Некорректо введена дата")]
        [DataType(DataType.DateTime)]
        public DateTime DateEnd { get; set; }
    }
}
