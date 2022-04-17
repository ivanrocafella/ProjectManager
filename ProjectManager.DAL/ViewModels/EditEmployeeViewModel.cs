using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Имя сотрудника обязательно для заполнения")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Фамилия сотрудника обязательно для заполнения")]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }
        [Display(Name = "Отчество")]
        public string PatronymicName { get; set; }
        [Required(ErrorMessage = "Почта обязательна для заполнения")]
        [Display(Name = "Электронная почта")]
        [EmailAddress(ErrorMessage = "Некорректная почта")]
        public string Email { get; set; }
    }
}
