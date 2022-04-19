using ProjectManager.Core;
using ProjectManager.Core.Entities;
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
            _unitOfWork.GetRepository<Core.Entities.Task>().Add(task);
            _unitOfWork.Complete();
            return task;
        }
    }
}
