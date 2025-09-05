using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyProject.Application.Services;
using MyProject.Domain.Enums;
using MyProject.Domain.Interfaces;
using NUnit.Framework;
using Task = MyProject.Domain.Entities.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Application.Test.Services;

[TestFixture]
public sealed class TaskServiceTests
{
    private Mock<ITaskRepository> _mockTaskRepository;
    private Mock<ILogger<TaskService>> _mockLogger;
    private TaskService _taskService;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _mockLogger = new Mock<ILogger<TaskService>>();
        _taskService = new TaskService(_mockTaskRepository.Object, _mockLogger.Object);
        _userId = Guid.NewGuid();
    }

    [Test]
    public async System.Threading.Tasks.Task CreateTaskAsync_ShouldReturnSuccess_WhenTaskIsCreated()
    {
        // Arrange
        var title = "Test Task";
        var status = "Pending";
        var priority = "Medium";

        _mockTaskRepository.Setup(r => r.AddAsync(It.IsAny<Task>())).Returns(System.Threading.Tasks.Task.CompletedTask);
        _mockTaskRepository.Setup(r => r.SaveChangesAsync()).Returns(System.Threading.Tasks.Task.FromResult(1));

        // Act
        var result = await _taskService.CreateTaskAsync(_userId, title, null, null, status, priority);

        // Assert
        result.Success.Should().BeTrue();
        result.Code.Should().Be(201);
        result.Message.Should().Be("Task created successfully.");
        _mockTaskRepository
            .Verify(r => r.AddAsync(
                It.Is<Task>(t => t.Title == title && t.UserId == _userId)), Times.Once);
        _mockTaskRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async System.Threading.Tasks.Task GetTasksAsync_ShouldReturnFilteredTasks_WhenFiltersAreApplied()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var statusFilter = "Completed";
        var priorityFilter = "High";
        var dueDateFilter = DateTime.UtcNow.Date;

        var tasks = new List<Task>
        {
            new()
            {
                Id = Guid.NewGuid(), Title = "Filtered Task", UserId = _userId, Status = TaskStatus.Completed,
                Priority = TaskPriority.High, DueDate = dueDateFilter, CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _mockTaskRepository
            .Setup(r => r.GetAllTasksByUserIdAsync(
                _userId, pageNumber, pageSize, It.IsAny<DateTime?>(), It.IsAny<TaskStatus>(), It.IsAny<TaskPriority?>()))
            .ReturnsAsync(tasks);
        _mockTaskRepository
            .Setup(r => r.GetTotalCountByUserIdAsync(
                _userId, It.IsAny<TaskStatus?>(), It.IsAny<DateTime?>(), It.IsAny<TaskPriority?>()))
            .ReturnsAsync(tasks.Count);

        // Act
        var result = await _taskService.GetTasksAsync(
            _userId, pageNumber, pageSize, dueDateFilter, statusFilter, priorityFilter);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Tasks.Should().HaveCount(1);
        result.Data.TotalCount.Should().Be(1);
        
        _mockTaskRepository
            .Verify(r => r.GetAllTasksByUserIdAsync(
                _userId, pageNumber, pageSize, dueDateFilter, TaskStatus.Completed, TaskPriority.High), Times.Once);
        _mockTaskRepository
            .Verify(r => r.GetTotalCountByUserIdAsync(
                _userId, TaskStatus.Completed, dueDateFilter, TaskPriority.High), Times.Once);
    }

    [Test]
    public async System.Threading.Tasks.Task GetTasksAsync_ShouldReturnNotFound_WhenNoTasksMatchFilters()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var statusFilter = "Pending";

        _mockTaskRepository
            .Setup(r => r.GetAllTasksByUserIdAsync(
                _userId, pageNumber, pageSize, null, TaskStatus.Pending, null))
            .ReturnsAsync(new List<Task>());
        _mockTaskRepository
            .Setup(r => r.GetTotalCountByUserIdAsync(
                _userId, TaskStatus.Pending, null, null))
            .ReturnsAsync(0);

        // Act
        var result = await _taskService.GetTasksAsync(_userId, pageNumber, pageSize, null, statusFilter, null);

        // Assert
        result.Success.Should().BeFalse();
        result.Code.Should().Be(404);
        result.Message.Should().Be("No tasks found.");
    }

    [Test]
    public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExistsAndBelongsToUser()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new Task
        {
            Id = taskId, Title = "Test Task", UserId = _userId, Status = TaskStatus.Pending,
            Priority = TaskPriority.Low, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };
        _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

        // Act
        var result = await _taskService.GetTaskByIdAsync(taskId, _userId);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(taskId);
    }

    [Test]
    public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((Task)null!);

        // Act
        var result = await _taskService.GetTaskByIdAsync(taskId, _userId);

        // Assert
        result.Success.Should().BeFalse();
        result.Code.Should().Be(404);
        result.Message.Should().Be("Task not found.");
    }

    [Test]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldReturnSuccess_WhenAllFieldsAreUpdated()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new Task
        {
            Id = taskId, Title = "Old Title", Description = "Old Desc", DueDate = DateTime.UtcNow.AddDays(-1),
            Status = TaskStatus.Pending, Priority = TaskPriority.Low, UserId = _userId
        };
        
        _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
        _mockTaskRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var newTitle = "New Title";
        var newDesc = "New Desc";
        var newDueDate = DateTime.UtcNow.AddDays(5);
        var newStatus = "InProgress";
        var newPriority = "High";

        // Act
        var result = await _taskService.UpdateTaskAsync(
            taskId, _userId, newTitle, newDesc, newDueDate, newStatus, newPriority);

        // Assert
        result.Success.Should().BeTrue();
        result.Code.Should().Be(200);
        
        task.Title.Should().Be(newTitle);
        task.Description.Should().Be(newDesc);
        task.DueDate.Should().Be(newDueDate);
        task.Status.Should().Be(TaskStatus.InProgress);
        task.Priority.Should().Be(TaskPriority.High);
        
        _mockTaskRepository.Verify(r => r.Update(task), Times.Once);
        _mockTaskRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldNotUpdateFields_WhenInputsAreNullOrWhitespace()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var originalTitle = "Original Title";
        var originalDesc = "Original Desc";
        var task = new Task
        {
            Id = taskId, Title = originalTitle, Description = originalDesc, Status = TaskStatus.Pending,
            Priority = TaskPriority.Low, UserId = _userId
        };
        
        _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
        _mockTaskRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _taskService.UpdateTaskAsync(taskId, _userId, " ", null, null, "", null);

        // Assert
        result.Success.Should().BeTrue();
        
        task.Title.Should().Be(originalTitle);
        task.Description.Should().Be(originalDesc);
        
        _mockTaskRepository.Verify(r => r.Update(task), Times.Once);
        _mockTaskRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldReturnNotFound_WhenTaskDoesNotBelongToUser()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var task = new Task { Id = taskId, Title = "Test Task", UserId = otherUserId };
        
        _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

        // Act
        var result = await _taskService.UpdateTaskAsync(taskId, _userId, "New Title", null, null, null, null);

        // Assert
        result.Success.Should().BeFalse();
        result.Code.Should().Be(404);
        result.Message.Should().Be("Task not found.");
        
        _mockTaskRepository.Verify(r => r.Update(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((Task)null!);

        // Act
        var result = await _taskService.UpdateTaskAsync(taskId, _userId, "New Title", null, null, null, null);

        // Assert
        result.Success.Should().BeFalse();
        result.Code.Should().Be(404);
        result.Message.Should().Be("Task not found.");
    }

    [Test]
    public async System.Threading.Tasks.Task DeleteTaskAsync_ShouldReturnSuccess_WhenTaskIsDeleted()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new Task { Id = taskId, Title = "Test Task", UserId = _userId };
        
        _mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
        _mockTaskRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _taskService.DeleteTaskAsync(taskId, _userId);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Task deleted successfully.");
        
        _mockTaskRepository.Verify(r => r.Remove(task), Times.Once);
        _mockTaskRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}