using SvelteTodo.API.Entities;

namespace SvelteTodo.API.Interfaces
{
    public interface ITodoService
    {
        public Task<TodoTask?> GetTodoTaskByIdAsync(string id);
        public Task<IReadOnlyCollection<TodoTask>> GetAllTodoTasksAsync();
        public Task AddTodoTask(TodoTask todoTask);
        public Task UpdateTodoTaskAsync(TodoTask todoTask);
        public Task DeleteTodoTaskAsync(string id);
    }
}
