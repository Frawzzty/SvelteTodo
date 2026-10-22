using Microsoft.AspNetCore.Http.HttpResults;
using SvelteTodo.API.Entities;
using SvelteTodo.API.Interfaces;
using SvelteTodo.API.Services;

namespace SvelteTodo.API.EndPoints
{
    // Create Extension for Webapplication. Use in Program.cs instead of writing the endpoints there.
    public static class TodoTaskEndPoints
    {
        /* Original webapplicaion extension 
         public static WebApplication MapTodoTaskEndpoints(this WebApplication app)
         {
            app.MapGet("/api/tasks", async (ITodoService todoService) =>
            {
                return await todoService.GetAllTodoTasksAsync();
            });

            app.MapGet("/api/tasks/{id}", async (ITodoService todoService, string id) =>
            {
                var todoTask = await todoService.GetTodoTaskByIdAsync(id);
                return todoTask is not null ? Results.Ok(todoTask) : Results.NotFound();
            });

            return app;
         }
         */

        public static RouteGroupBuilder MapTodoTaskEndpoints(this RouteGroupBuilder group)
        {
            // Read by Id
            group.MapGet("/", async (ITodoService todoService) =>
            {
                return await todoService.GetAllTodoTasksAsync();
            });

            // Read all
            group.MapGet("/{id}", async (ITodoService todoService, string id) =>
            {
                var todoTask = await todoService.GetTodoTaskByIdAsync(id);
                return todoTask is not null ? Results.Ok(todoTask) : Results.NotFound();
            });

            // Create
            group.MapPost("/", async (ITodoService todoService, TodoTask newTodoTask) =>
            {
                if(newTodoTask is null)
                    return Results.BadRequest();

                await todoService.AddTodoTask(newTodoTask);

                return Results.Created($"/api/tasks/{newTodoTask.Id}", newTodoTask);
            });

            // Update
            group.MapPut("/{id}", async (ITodoService todoService, string id, TodoTask updateTodoTask) =>
            {
                if (updateTodoTask is null)
                    return Results.BadRequest();

                var todoTask = await todoService.GetTodoTaskByIdAsync(id);

                if (todoTask is null)
                    return Results.NotFound();

                todoTask.Text = updateTodoTask.Text;
                todoTask.IsDone = updateTodoTask.IsDone;
                todoTask.DoneByDate = updateTodoTask.DoneByDate;

                await todoService.UpdateTodoTaskAsync(todoTask);

                return Results.Ok();
            });

            // Delete by Id
            group.MapDelete("/{id}", async (ITodoService todoService, string id) =>
            {
                if (string.IsNullOrEmpty(id)) return Results.BadRequest();

                await todoService.DeleteTodoTaskAsync(id);

                return Results.NoContent();
            });

            return group;
        }
    }
}
