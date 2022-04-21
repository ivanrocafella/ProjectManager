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
using Task = ProjectManager.Core.Entities.Task;

namespace ProjectManager.DAL.Services
{
    public class TaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Task> AllTasks() => _unitOfWork.GetRepository<Task>().GetAll();

        public List<Task> GetTasksByProjectIdEmployeeId(int projectId, int employeeId) =>
            _unitOfWork.GetRepository<Task>()
            .Find(e => e.ProjectId == projectId && e.ExecutorId == employeeId)
            .ToList();

        public Task AddTask(DetailsProjectViewModel viewModel)
        {
            Task task = new()
            {
                Name = viewModel.Name,
                Comment = viewModel.Comment,
                AutorId = viewModel.AutorId,
                PriorityId = viewModel.PriorityId,
                ProjectId = viewModel.Project.Id
            };
            if (viewModel.ExecutorId != 0)
                task.ExecutorId = viewModel.ExecutorId;
            _unitOfWork.GetRepository<Task>().Add(task);
            _unitOfWork.Complete();
            return task;
        }

        public Task GetTask(int id) => _unitOfWork.GetRepository<Task>()
        .GetAll()
        .Include(a => a.Autor)
        .Include(e => e.Executor)
        .Include(t => t.Project)
        .FirstOrDefault(e => e.Id == id);

        public EditTaskViewModel GetEditTaskViewModel(Task task)
        {
            EditTaskViewModel editTaskView = new()
            {
                Id = task.Id,
                Name = task.Name,
                ExecutorId = task.ExecutorId,
                Executor = task.Executor,
                PriorityId = task.PriorityId,
                StatusId = task.StatusId,
                Comment = task.Comment, 
                Priorities = Enum.GetValues(typeof(Priority)),
                Statuses = Enum.GetValues(typeof(Status)),
            };
            return editTaskView;
        }

        public void EditTask(EditTaskViewModel viewModel)
        {
            IRepository<Task> repository = _unitOfWork.GetRepository<Task>();
            Task taskForEdit = repository.GetById(viewModel.Id);
            taskForEdit.Name = viewModel.Name;
            if (viewModel.ExecutorId > 0)
                taskForEdit.ExecutorId = viewModel.ExecutorId;
            else if (viewModel.ExecutorId == 0 && taskForEdit.ExecutorId != null)
                taskForEdit.ExecutorId = null;           
            taskForEdit.PriorityId = viewModel.PriorityId;
            taskForEdit.StatusId = viewModel.StatusId;
            taskForEdit.Comment = viewModel.Comment;
            repository.Update(taskForEdit);
            _unitOfWork.Complete();
        }

        public void RemoveFromTask(Task task)
        {
            IRepository<Task> repository = _unitOfWork.GetRepository<Task>();
            task.ExecutorId = null;
            repository.Update(task);
            _unitOfWork.Complete();
        }

        public void Remove(int id)
        {
            _unitOfWork.GetRepository<Task>().Remove(id);
            _unitOfWork.Complete();
        }

        public IQueryable<Task> QueryableTasksOfProjectAfterFilter(string Name, string StatusIdForFiltr, int projectId)
        {
            IQueryable<Task> tasks = AllTasks().Where(e => e.ProjectId == projectId).OrderBy(e => e.Name);
            if (!string.IsNullOrEmpty(Name))
                tasks = tasks.Where(p => p.Name.Contains(Name));

            if (!string.IsNullOrEmpty(StatusIdForFiltr) && StatusIdForFiltr != "All")
                tasks = tasks.Where(e => e.StatusId == Int32.Parse(StatusIdForFiltr));

            return tasks;
        }

        public IQueryable<Task> QueryableTasksAfterSort(IQueryable<Task> tasks, SortState SortOrder)
        {
            switch (SortOrder)
            {
                case SortState.NameDesc:
                    tasks = tasks.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriorityIdAsc:
                    tasks = tasks.OrderBy(s => s.PriorityId);
                    break;
                case SortState.PriorityIdDesc:
                    tasks = tasks.OrderByDescending(s => s.PriorityId);
                    break;
                case SortState.StatusIdAsc:
                    tasks = tasks.OrderBy(s => s.StatusId);
                    break;
                case SortState.StatusIdDesc:
                    tasks = tasks.OrderByDescending(s => s.StatusId);
                    break;
                default:
                    tasks = tasks.OrderBy(s => s.Name);
                    break;
            }
            return tasks;
        }
    }
}
