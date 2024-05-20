using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TaskService.Controllers;
using TaskService.Models;
using Xunit;

namespace TaskService.Tests
{
    public class TaskControllerTests
    {
        private readonly Mock<TaskContext> _mockContext;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _mockContext = new Mock<TaskContext>(new DbContextOptions<TaskContext>());
            _controller = new TaskController(_mockContext.Object);
        }


        [Fact]
        public async System.Threading.Tasks.Task GetTask_ReturnsNotFoundResult_WhenTaskNotFound()
        {
            var taskId = 1;
            Models.Task task = null;
            _mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync(task);
            var result = await _controller.GetTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
      [Fact]
        public async System.Threading.Tasks.Task PutTask_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var taskId = 1;
            var task = new Models.Task { Id = 2 };
            
            var result = await _controller.PutTask(taskId, task);
            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async System.Threading.Tasks.Task PostTask_ReturnsCreatedAtActionResult()
        {
            var task = new Models.Task { Id = 1 };
            var result = await _controller.PostTask(task);
            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetTask", createdAtActionResult.ActionName);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ReturnsNotFound_WhenTaskNotFound()
        {
            var taskId = 1;
            Models.Task task = null;
            _mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync(task);
            var result = await _controller.DeleteTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ReturnsNoContent_WhenTaskDeletedSuccessfully()
        {
           
            var taskId = 1;
            var task = new Models.Task { Id = taskId };
            _mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync(task);
            var result = await _controller.DeleteTask(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task PutTask_ReturnsTask_WhenUpdatedSuccessfully()
        {
            var taskId = 1;
            var updatedTask = new Models.Task { Id = taskId, Title = "Updated Task" };
            var existingTask = new Models.Task { Id = taskId, Title = "Existing Task" };
            _mockContext.Setup(c => c.Tasks.FindAsync(taskId)).ReturnsAsync(existingTask);
            var result = await _controller.PutTask(taskId, updatedTask);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Task>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var task = Assert.IsAssignableFrom<Models.Task>(okObjectResult.Value);
            Assert.Equal(updatedTask.Title, task.Title);
        }
    }

}
