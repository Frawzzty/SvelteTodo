using Microsoft.EntityFrameworkCore;
using SvelteTodo.API.Entities;

namespace SvelteTodo.API.Data
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
    {
        public DbSet<TodoTask> TodoTasks { get; set; }
        
    }
}
