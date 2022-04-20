using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.Entities;
using ProjectManager.Core.Entities.Enums;
using ProjectManager.DAL.Services;
using ProjectManager.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = ProjectManager.Core.Entities.Task;

namespace ProjectManager.UI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _PService;
        private readonly EmployeeService _EService;
        private readonly TaskService _TService;

        public ProjectController(ProjectService pService, EmployeeService eService, TaskService tService)
        {
            _PService = pService;
            _EService = eService;
            _TService = tService;
        }

        public IActionResult Index(DateTime DateStartFrom, DateTime DateStartBefore,
                                   string Name, string PriorityIdForFiltr, SortState SortOrder = SortState.DateStartDesc)
        {
            ViewBag.NameSort = SortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.DateStart = SortOrder == SortState.DateStartAsc ? SortState.DateStartDesc : SortState.DateStartAsc;
            ViewBag.PriorityId = SortOrder == SortState.PriorityIdAsc ? SortState.PriorityIdDesc : SortState.PriorityIdAsc;
            IQueryable<Project> projects = _PService.QueryableProjcetsAfterFilter(DateStartFrom, DateStartBefore,
                                                                                  Name, PriorityIdForFiltr);
            ProjectsAndProjectViewModel projectsAndProject =
                _PService.GetProjectsAndProjectViewModel(_PService.QueryableProjectsAfterSort(projects, SortOrder));
            projectsAndProject.Employees = _EService.AllEmployees().Result;

            return View(projectsAndProject);
        }

        public IActionResult Details(int id)
        {
            Project project = _PService.GetProject(id);
            if (project == null)
                return NoContent();
            else
            {
                DetailsProjectViewModel detailsProjectView = _PService.GetDetailsProjectViewModel(project);
                return View(detailsProjectView);
            }

        }

        [HttpPost]
        public IActionResult Details(int employeeId, int projectId)
        {
            _EService.AddEmployeeToProject(employeeId, projectId);
            return RedirectToAction("Details", "Project", new { id = projectId });
        }

        [HttpPost]
        public JsonResult Add(ProjectsAndProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Project project = _PService.AddProject(viewModel);
                if (project.ProjectManagerId != null)
                    _EService.AddEmployeeToProject((int)project.ProjectManagerId, project.Id);
                return Json(new { success = true, nameProjectJS = project.Name, idProjectJS = project.Id });
            }
            return Json(new { succes = false });
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            _PService.Remove(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveFromProject(int projectId, int employeeId)
        {
            List<Task> tasks = _TService.GetTasksByProjectIdEmployeeId(projectId, employeeId);
            foreach (var item in tasks)
                _TService.RemoveFromTask(item);
            _PService.RemoveFromProject(projectId, employeeId);
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveProjectManagerFromProject(int id)
        {
            _PService.RemoveProjectManagerFromProject(id);
            return Ok();
        }
        public IActionResult Edit(int id)
        {
            Project project = _PService.GetProject(id);
            if (project == null)
                return NoContent();
            else
            {
                EditProjectViewModel editProjectView = _PService.GetEditProjectViewModel(project);
                return View(editProjectView);
            }
        }

        [HttpPost]
        public IActionResult Edit(EditProjectViewModel viewModel)
        {
            _PService.EditProject(viewModel);
            return RedirectToAction("Details", new { id = viewModel.Id });
        }


        //Validation

        //   [AcceptVerbs("GET", "POST")]
        //   public bool CheckDate(DateTime dateStart) => dateStart >= DateTime.Now && dateStart < DateTime.MaxValue || dateStart == DateTime.MinValue;
    }
}
