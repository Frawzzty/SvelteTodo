using Microsoft.EntityFrameworkCore;
using SvelteTodo.API.Data;
using SvelteTodo.API.Entities;
using SvelteTodo.API.Interfaces;

namespace SvelteTodo.API.Services
{
    public class TodoService(TodoDbContext context) : ITodoService
    {
        private readonly TodoDbContext _context = context;

        public async Task<TodoTask?> GetTodoTaskByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("THROW: GetTodoTaskAsync: id was null empty");

            TodoTask? task = null;

            try 
            {
                task = await _context.Set<TodoTask>().SingleOrDefaultAsync(task => task.Id == id);
            }
            catch 
            { 
                //throw new DbUpdateException($"THROW: Could not get TodoItem from DB. ID:{id}"); 
            }

            return task;
        }

        public async Task<IReadOnlyCollection<TodoTask>> GetAllTodoTasksAsync()
        {

            List<TodoTask> todoTasks = new();

            try
            {
                todoTasks = await _context.Set<TodoTask>().ToListAsync();
            }
            catch
            {
                //throw new DbUpdateException($"THROW: Could not get TodoItems from DB");
            }

            return todoTasks;
        }

        public async Task AddTodoTask(TodoTask newTodoTask)
        {
            if (newTodoTask == null) 
                throw new ArgumentNullException("Todo task was null");
            if(string.IsNullOrWhiteSpace(newTodoTask.Text))
                throw new ArgumentNullException("Todo task text is null or empty");
            if (newTodoTask.DoneByDate == default)
                throw new ArgumentNullException("Todo task date was default value");

            if (string.IsNullOrWhiteSpace(newTodoTask.Id))
                newTodoTask.Id = Guid.NewGuid().ToString();

            await _context.AddAsync(newTodoTask);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTodoTaskAsync(TodoTask todoTaskToUpdate)
        {
            if (todoTaskToUpdate == null) throw new ArgumentNullException("Todo task was null");

            _context.Update(todoTaskToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodoTaskAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            var todoTask = await GetTodoTaskByIdAsync(id);

            if (todoTask is not null)
            {
                _context.Remove(todoTask);
                _context.SaveChanges();
            }
        }
    }
}
