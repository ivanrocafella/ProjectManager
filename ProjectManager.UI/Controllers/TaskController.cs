using Microsoft.AspNetCore.Mvc;
using ProjectManager.DAL.Services;
using ProjectManager.DAL.ViewModels;
using ProjectManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = ProjectManager.Core.Entities.Task;

namespace ProjectManager.UI.Controllers
{
    public class TaskController : Controller
    {

        private readonly ProjectService _PService;
        private readonly EmployeeService _EService;
        private readonly TaskService _TService;

        public TaskController(ProjectService pService, EmployeeService eService, TaskService tService)
        {
            _PService = pService;
            _EService = eService;
            _TService = tService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(DetailsProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Task task = _TService.AddTask(viewModel);
                return Json(new { success = true, taskJS = task });
            }
            return Json(new { succes = false });
        }
    }
}
