using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.Entities;
using ProjectManager.DAL.Services;
using ProjectManager.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.UI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ProjectService _PService;
        private readonly EmployeeService _EService;

        public EmployeeController(ProjectService pService, EmployeeService eService)
        {
            _PService = pService;
            _EService = eService;
        }
        public IActionResult Index()
        {
            return View(_EService.GetEmployeesAndEmployeeViewModel());
        }

        [HttpPost]
        public JsonResult Add(EmployeesAndEmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _EService.AddEmployee(viewModel);
                return Json(new { success = true, employeeJS = employee });
            }
            return Json(new { succes = false });
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            _EService.Remove(id);
            return Ok();
        }

        public IActionResult Details(int id)
        {
            Employee employee = _EService.GetEmployee(id);
            if (employee == null)
                return NoContent();
            else
            {
                DetailsEmployeeViewModel detailsEmployeeView = _EService.GetDetailsEmployeeViewModel(employee);
                return View(detailsEmployeeView);
            }

        }

        [HttpPost]
        public IActionResult Details(int employeeId, int projectId)
        {
            Employee employee = _EService.GetEmployee(employeeId);
            Project project = _PService.GetProject(projectId);
            if (employee != null)
                _EService.AddEmployeeToProject(employee, project);
            return RedirectToAction("Details", "Employee", new { id = employeeId });
        }

        public IActionResult Edit(int id)
        {
            Employee employee = _EService.GetEmployee(id);
            if (employee == null)
                return NoContent();
            else
            {
                EditEmployeeViewModel editEmployeeView = _EService.GetEditEmployeeViewModel(employee);
                return View(editEmployeeView);
            }
        }

        [HttpPost]
        public IActionResult Edit(EditEmployeeViewModel viewModel)
        {
            _EService.EditEmployee(viewModel);
            return RedirectToAction("Details", new { id = viewModel.Id });
        }
    }
}
