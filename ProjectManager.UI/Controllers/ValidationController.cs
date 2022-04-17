using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.UI.Controllers
{
    public class ValidationController : Controller
    {
        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckDate(DateTime dateStart) => Json(dateStart >= DateTime.Now 
            && dateStart < DateTime.MaxValue || dateStart == DateTime.MinValue);
    }
}
