using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.ValidationAttributes
{
    public class CheckDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = Convert.ToDateTime(value);

            if (date >= DateTime.Now && date < DateTime.MaxValue)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);

        }
    }
}
