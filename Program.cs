var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    Task.LoadAll();
    return Task.Tasks.Select(task => task.ToString()).Aggregate((a, b) => $"{a}\n{b}") +
            @"
            
            Add a new task by going to /add?name=NAME&description=DESCRIPTION
            For example: /add?name=Test&description=This+is+a+test+task
            
            Mark a task as done by going to /mark/ID
            For example: /mark/1
            
            Delete a task by going to /delete/ID
            For example: /delete/1";
});

app.MapGet("/add", (string name, string description) =>
{
    Task.LoadAll();
    Task task = new(name, description);
    Task.SaveAll();
    return Results.Redirect("/");
});

app.MapGet("/mark/{id}", (int id) =>
{
    Task.LoadAll();
    Task? task = Task.GetTaskById(id);
    if (task == null)
    {
        return Results.Redirect("/");
    }
    task.SwitchIsDone();
    Task.SaveAll();
    return Results.Redirect("/");
});

app.MapGet("/delete/{id}", (int id) =>
{
    Task.LoadAll();
    Task? task = Task.GetTaskById(id);
    if (task == null)
    {
        return Results.Redirect("/");
    }
    task.Delete();
    Task.SaveAll();
    return Results.Redirect("/");
});

app.Run();
