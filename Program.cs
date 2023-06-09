var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    Task.LoadAll();
    // make a string that contains an html for the user to enter the name and the description of a new Task then be able to clock submit to be redirected to the /add route
    string html = @$"Add a new Task
                    <form action=""/add"" method=""get"">
                        <label for=""name"">Name:</label><br>
                        <input type=""text"" id=""name"" name=""name""><br>
                        <label for=""description"">Description:</label><br>
                        <input type=""text"" id=""description"" name=""description""><br><br>
                        <input type=""submit"" value=""Submit"">
                    </form>";
    if (Task.Tasks == null || Task.Tasks.Count == 0)
    {
        return Results.Content($"{html} <br> There are no tasks to show", "text/html");
    }
    String tasks = Task.Tasks.OrderBy(t => t.IsDone).ThenByDescending(t => t.CreatedDate).Select(t =>
    {
        return $"<a href=\"/mark/{t.Id}\">{t}</a>";
    }).Aggregate((a, b) => $"<br> {a} <br><br> {b}");

    return Results.Content($"{html} <br> <pre>{tasks}</pre", "text/html");
});

app.MapGet("/add", (string name, string description) =>
{
    Task.LoadAll();
    Task task = new(name, description);
    Task.SaveAll();
    String html = @$"Task added successfully <br> <pre>{task}</pre>
                    <br> <a href=""/"">Go back</a>";
    return Results.Content(html, "text/html");
});

app.MapGet("/mark/{id}", (int id) =>
{
    Task.LoadAll();
    Task? task = Task.GetTaskById(id);
    if (task == null)
    {
        return Results.Content($"Task with id {id} not found", "text/html");
    }
    task.SwitchIsDone();
    Task.SaveAll();
    return Results.Redirect("/");
});

app.Run();
