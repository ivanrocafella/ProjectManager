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

        public IActionResult Details(int id)
        {
            Task task = _TService.GetTask(id);
            if (task == null)
                return NoContent();
            else
                return View(task);
        }

        public IActionResult Edit(int id)
        {
            Task task = _TService.GetTask(id);
            if (task == null)
                return NoContent();
            else
            {
                EditTaskViewModel editTaskView = _TService.GetEditTaskViewModel(task);
                Project project = _PService.GetProject(task.ProjectId);
                editTaskView.Executors = project.EmployeeProjects
                                                .Select(e => e.Employee)
                                                .ToList();
                return View(editTaskView);
            }
        }

        [HttpPost]
        public IActionResult Edit(EditTaskViewModel viewModel)
        {
            _TService.EditTask(viewModel);
            return RedirectToAction("Details", new { id = viewModel.Id });
        }
    }
}
