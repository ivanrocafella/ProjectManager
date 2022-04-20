using Microsoft.EntityFrameworkCore;
using ProjectManager.Core;
using ProjectManager.Core.Entities;
using ProjectManager.Core.Repositories;
using ProjectManager.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.Services
{
    public class EmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Employee GetEmployee(int id) => _unitOfWork.GetRepository<Employee>()
            .GetAll()
            .Include(ep => ep.EmployeeProjects)
            .ThenInclude(p => p.Project)
            .Include(ps => ps.Projects)
            .Include(ct => ct.CreatedTasks)
            .Include(et => et.ExecutedTasks)
            .FirstOrDefault(e => e.Id == id);

        public void AddEmployeeToProject(int employeeId, int projectId)
        {
            EmployeeProject employeeProject = new()
            {
                EmployeeId = employeeId,
                ProjectId = projectId,
                CreatedAd = DateTime.Now
            };
            _unitOfWork.GetRepository<EmployeeProject>().Add(employeeProject);
            _unitOfWork.Complete();
        }

        public async Task<List<Employee>> AllEmployees() => await _unitOfWork.GetRepository<Employee>()
           .GetAll()
           .ToListAsync();

        public EmployeesAndEmployeeViewModel GetEmployeesAndEmployeeViewModel()
        {
            EmployeesAndEmployeeViewModel employeesAndEmployee = new()
            {
                Employees = AllEmployees().Result.OrderByDescending(p => p.Id).ToList()
            };
            return employeesAndEmployee;
        }

        public EditEmployeeViewModel GetEditEmployeeViewModel(Employee employee)
        {
            EditEmployeeViewModel editEmployeeView = new()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                SurName = employee.SurName,
                PatronymicName = employee.PatronymicName,
                Email = employee.Email
            };
            return editEmployeeView;
        }

        public Employee AddEmployee(EmployeesAndEmployeeViewModel viewModel)
        {
            Employee employee = new()
            {
                FirstName = viewModel.FirstName,
                SurName = viewModel.SurName,
                Email = viewModel.Email
            };
            _unitOfWork.GetRepository<Employee>().Add(employee);
            _unitOfWork.Complete();
            return employee;
        }

        public void EditEmployee(EditEmployeeViewModel viewModel)
        {
            IRepository<Employee> repository = _unitOfWork.GetRepository<Employee>();
            Employee employeeForEdit = repository.GetById(viewModel.Id);
            employeeForEdit.FirstName = viewModel.FirstName;
            employeeForEdit.SurName = viewModel.SurName;
            employeeForEdit.PatronymicName = viewModel.PatronymicName;
            employeeForEdit.Email = viewModel.Email;
            repository.Update(employeeForEdit);
            _unitOfWork.Complete();
        }

        public void Remove(int id)
        {
            _unitOfWork.GetRepository<Employee>().Remove(id);
            _unitOfWork.Complete();
        }

        public DetailsEmployeeViewModel GetDetailsEmployeeViewModel(Employee employee)
        {
            List<Project> projects = employee.EmployeeProjects
                .Select(e => e.Project)
                .ToList();
            List<Project> freeProjects = _unitOfWork.GetRepository<Project>()
                .GetAll()
                .AsEnumerable()
                .Except(projects)
                .ToList();
            DetailsEmployeeViewModel detailsEmployeeView = new()
            {
                Employee = employee,
                PinnedProjects = projects,
                ManagedProjects = employee.Projects.ToList(),
                FreeProjects = freeProjects,
                CreatedTasks = employee.CreatedTasks.ToList()
            };
            return detailsEmployeeView;
        }

    }
}
