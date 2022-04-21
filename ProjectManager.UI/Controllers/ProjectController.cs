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

        //Action with page Index of Project
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

        //Action with page Details of Project
        public IActionResult Details(int id, string Name, 
                                     string StatusIdForFiltr, SortState SortOrder = SortState.NameAsc)
        {
            ViewBag.NameSort = SortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.PriorityId = SortOrder == SortState.PriorityIdAsc ? SortState.PriorityIdDesc : SortState.PriorityIdAsc;
            ViewBag.StatuId = SortOrder == SortState.StatusIdAsc ? SortState.StatusIdDesc : SortState.StatusIdAsc;

            Project project = _PService.GetProject(id);
            if (project == null)
                return NoContent();
            else
            {
                IQueryable<Task> tasks = _TService.QueryableTasksOfProjectAfterFilter(Name, StatusIdForFiltr, project.Id);
                DetailsProjectViewModel detailsProjectView = _PService.GetDetailsProjectViewModel(project,
                                                             _TService.QueryableTasksAfterSort(tasks, SortOrder));
                return View(detailsProjectView);
            }
        }
        
        //Action with adding Employee to Project
        [HttpPost]
        public IActionResult Details(int employeeId, int projectId)
        {
            _EService.AddEmployeeToProject(employeeId, projectId);
            return RedirectToAction("Details", "Project", new { id = projectId });
        }

        //Action with adding new Project to database
        [HttpPost]
        public JsonResult Add(ProjectsAndProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Project project = _PService.AddProject(viewModel);
                if (project.ProjectManagerId != null)
                    _EService.AddEmployeeToProject((int)project.ProjectManagerId, project.Id);
                project.EmployeeProjects.Clear();
                return Json(new { success = true, projectJS = project});
            }
            return Json(new { succes = false });
        }

        //Action with removing Project from database
        [HttpPost]
        public IActionResult Remove(int id)
        {
            _PService.Remove(id);
            return Ok();
        }

        //Action with removing executing Employee from Project
        [HttpPost]
        public IActionResult RemoveFromProject(int projectId, int employeeId)
        {
            List<Task> tasks = _TService.GetTasksByProjectIdEmployeeId(projectId, employeeId);
            foreach (var item in tasks)
                _TService.RemoveFromTask(item);
            _PService.RemoveFromProject(projectId, employeeId);
            return Ok();
        }

        //Action with removing leading Employee from Project
        [HttpPost]
        public IActionResult RemoveProjectManagerFromProject(int id)
        {
            _PService.RemoveProjectManagerFromProject(id);
            return Ok();
        }

        //Action with page Edit of Project
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

        //Action with editing Project
        [HttpPost]
        public IActionResult Edit(EditProjectViewModel viewModel)
        {
            _PService.EditProject(viewModel);
            return RedirectToAction("Details", new { id = viewModel.Id });
        }

    }
}
