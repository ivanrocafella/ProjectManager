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
        private readonly TaskService _TService;

        public TaskController(ProjectService pService, TaskService tService)
        {
            _PService = pService;
            _TService = tService;
        }

        //Action with adding Task to database
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

        //Action with page Details of Task
        public IActionResult Details(int id)
        {
            Task task = _TService.GetTask(id);
            if (task == null)
                return NoContent();
            else
                return View(task);
        }

        //Action with page Edit of Task
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
                if (task.ExecutorId != null)
                    editTaskView.Executors.Remove(task.Executor);
                return View(editTaskView);
            }
        }

        //Action with editing of Task
        [HttpPost]
        public IActionResult Edit(EditTaskViewModel viewModel)
        {
            _TService.EditTask(viewModel);
            return RedirectToAction("Details", new { id = viewModel.Id });
        }

        //Action with removing Task from database
        [HttpPost]
        public IActionResult Remove(int id)
        {
            _TService.Remove(id);
            return Ok();
        }
    }
}
