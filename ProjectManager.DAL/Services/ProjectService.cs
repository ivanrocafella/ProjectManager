using Microsoft.EntityFrameworkCore;
using ProjectManager.Core;
using ProjectManager.Core.Entities;
using ProjectManager.Core.Entities.Enums;
using ProjectManager.Core.Repositories;
using ProjectManager.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.Services
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Project> AllProjects() => _unitOfWork.GetRepository<Project>()
            .GetAll()
            .Include(e => e.ProjectManager);

        public ProjectsAndProjectViewModel GetProjectsAndProjectViewModel(IQueryable<Project> projects)
        {
            ProjectsAndProjectViewModel projectsAndProjectView = new()
            {
                Projects = projects.ToList(),
                Priorities = Enum.GetValues(typeof(Priority))
            };
            return projectsAndProjectView;
        }

        public Project GetProject(int id) => _unitOfWork.GetRepository<Project>()
            .GetAll()
            .Include(ep => ep.EmployeeProjects)
            .ThenInclude(e => e.Employee)
            .Include(e => e.ProjectManager)
            .Include(t => t.Tasks)
            .FirstOrDefault(e => e.Id == id);

        public DetailsProjectViewModel GetDetailsProjectViewModel(Project project)
        {
            List<Employee> executors = project.EmployeeProjects
                .Select(e => e.Employee)
                .ToList();
            List<Employee> freeEmployess = _unitOfWork.GetRepository<Employee>()
                .GetAll()
                .AsEnumerable()
                .Except(executors)
                .ToList();
            List<Employee> ExecutorsAndProjectManager = new();
            ExecutorsAndProjectManager.AddRange(executors);
            if (project.ProjectManager != null)
                ExecutorsAndProjectManager.Add(project.ProjectManager);
            DetailsProjectViewModel detailsProjectView = new()
            {
                Project = project,
                Executors = executors,
                FreeEmployees = freeEmployess,
                ExecutorsAndProjectManager = ExecutorsAndProjectManager,
                Priorities = Enum.GetValues(typeof(Priority))
            };
            return detailsProjectView;
        }

        public EditProjectViewModel GetEditProjectViewModel(Project project)
        {
            EditProjectViewModel editProjectView = new()
            {
                Id = project.Id,
                Name = project.Name,
                CustomerCompanyName = project.CustomerCompanyName,
                ExecutorCompanyName = project.ExecutorCompanyName,
                PriorityId = project.PriorityId,
                DateStart = project.DateStart,
                DateEnd = project.DateEnd,
                Priorities = Enum.GetValues(typeof(Priority))
            };
            return editProjectView;
        }

        public Project AddProject(ProjectsAndProjectViewModel viewModel)
        {
            Project project = new()
            {
                Name = viewModel.Name,
                CustomerCompanyName = viewModel.CustomerCompanyName,
                ExecutorCompanyName = viewModel.ExecutorCompanyName,
                PriorityId = viewModel.PriorityId,
                CreatedAd = DateTime.Now,
                PriorityUpdate = DateTime.Now,
                DateStart = viewModel.DateStart,
                DateEnd = viewModel.DateEnd
            };
            if (viewModel.ProjectManagerId != 0)
                project.ProjectManagerId = viewModel.ProjectManagerId;
            _unitOfWork.GetRepository<Project>().Add(project);
            _unitOfWork.Complete();
            return project;
        }

        public void EditProject(EditProjectViewModel viewModel)
        {
            IRepository<Project> repository = _unitOfWork.GetRepository<Project>();
            Project projectForEdit = repository.GetById(viewModel.Id);
            projectForEdit.Name = viewModel.Name;
            projectForEdit.CustomerCompanyName = viewModel.CustomerCompanyName;
            projectForEdit.ExecutorCompanyName = viewModel.ExecutorCompanyName;
            if (projectForEdit.PriorityId != viewModel.PriorityId)
                projectForEdit.PriorityUpdate = DateTime.Now;
            projectForEdit.PriorityId = viewModel.PriorityId;
            projectForEdit.DateStart = viewModel.DateStart;
            projectForEdit.DateEnd = viewModel.DateEnd;
            repository.Update(projectForEdit);
            _unitOfWork.Complete();
        }

        public void Remove(int id)
        {
            _unitOfWork.GetRepository<Project>().Remove(id);
            _unitOfWork.Complete();
        }

        public void RemoveFromProject(int projectId, int employeeId)
        {
            IRepository<EmployeeProject> repository = _unitOfWork.GetRepository<EmployeeProject>();
            EmployeeProject employeeProject = repository
                .GetAll()
                .FirstOrDefault(e => e.ProjectId == projectId && e.EmployeeId == employeeId);
            repository.RemoveByEntity(employeeProject);
            _unitOfWork.Complete();
        }

        public void RemoveProjectManagerFromProject(int id)
        {
            IRepository<Project> repository = _unitOfWork.GetRepository<Project>();
            Project project = repository.GetById(id);
            project.ProjectManagerId = null;
            repository.Update(project);
            _unitOfWork.Complete();
        }

        public IQueryable<Project> QueryableProjcetsAfterFilter(DateTime DateStartFrom, DateTime DateStartBefore,
                                                                string Name, string PriorityIdForFiltr)
        {
            IQueryable<Project> projects = AllProjects().OrderByDescending(e => e.DateStart);
            if (!string.IsNullOrEmpty(Name))
                projects = projects.Where(p => p.Name.Contains(Name));

            if (DateStartFrom != DateTime.MinValue && DateStartBefore != DateTime.MinValue)
                projects = projects.Where(e => e.DateStart >= DateStartFrom && e.DateStart <= DateStartBefore);
            else if (DateStartFrom != DateTime.MinValue)
                projects = projects.Where(e => e.DateStart >= DateStartFrom);
            else if (DateStartBefore != DateTime.MinValue)
                projects = projects.Where(e => e.DateStart <= DateStartBefore);

            if (!string.IsNullOrEmpty(PriorityIdForFiltr) && PriorityIdForFiltr != "All")
                projects = projects.Where(e => e.PriorityId == Int32.Parse(PriorityIdForFiltr));

            return projects;
        }

        public IQueryable<Project> QueryableProjectsAfterSort(IQueryable<Project> projects, SortState SortOrder)
        {
            switch (SortOrder)
            {
                case SortState.NameDesc:
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case SortState.DateStartAsc:
                    projects = projects.OrderBy(s => s.DateStart);
                    break;
                case SortState.DateStartDesc:
                    projects = projects.OrderByDescending(s => s.DateStart);
                    break;
                case SortState.PriorityIdAsc:
                    projects = projects.OrderBy(s => s.PriorityId);
                    break;
                case SortState.PriorityIdDesc:
                    projects = projects.OrderByDescending(s => s.PriorityId);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }
            return projects;
        }
    }
}
