using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SvelteTodo.API.Data;
using SvelteTodo.API.EndPoints;
using SvelteTodo.API.Entities;
using SvelteTodo.API.Interfaces;
using SvelteTodo.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// DBCONTEXT
builder.Services.AddDbContext<TodoDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITodoService, TodoService>();

// Allow SvelteApp to use API
builder.Services.AddCors(options =>
{
    options.AddPolicy("SvelteApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Allow Svelte to use API
app.UseCors("SvelteApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// Map endpoints by using building extensions. Find in DIR: /Endpoints.
app.MapGroup("/api/tasks").MapTodoTaskEndpoints();

app.Run();