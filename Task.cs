using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Task
{
    #region Static Attributes
    public static List<Task> Tasks { get; set; } = new List<Task>();
    private const string FILE_NAME = "tasks.json";
    #endregion

    #region Fields
    private readonly int id;
    private string name;
    // default get and custom set
    private string description;
    private bool isDone;
    private readonly DateTime createdDate;
    #endregion

    #region Attributes
    public int Id { get => id; }
    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Name cannot be null or empty");
            }
            name = value;
        }
    }
    public string Description
    {
        get => description;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Description cannot be null or empty");
            }
            description = value;
        }
    }
    public bool IsDone { get => isDone; set => isDone = value; }
    public DateTime CreatedDate { get => createdDate; }
    #endregion

    #region Constructors
    [JsonConstructorAttribute]
    public Task(int id, string name, string description, bool isDone, DateTime createdDate)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.isDone = isDone;
        this.createdDate = createdDate;

        Tasks.Add(this);
    }

    public Task(string name, string description) : this(Tasks.Count + 1, name, description, false, DateTime.Now)
    {
    }
    #endregion

    #region Methods

    public void SwitchIsDone()
    {
        isDone = !isDone;
    }

    public void Delete()
    {
        Tasks.Remove(this);
    }

    public static Task? GetTaskById(int id)
    {
        foreach (Task task in Tasks)
        {
            if (task.Id == id)
            {
                return task;
            }
        }
        return null;
    }

    public override string ToString()
    {
        return $"|{id,-5}|{name,-20}|{description,-50}|{(isDone ? "Completed" : "Not Completed" ),-15}|{createdDate,-20}";
    }
    #endregion

    #region Static Methods
    public static void SaveAll()
    {
        string json = JsonSerializer.Serialize(Tasks);
        File.WriteAllText(FILE_NAME, json);
    }

    public static void LoadAll()
    {
        String json;
        try
        {
            json = File.ReadAllText(FILE_NAME);
        }
        catch (FileNotFoundException e)
        {
            return;
        }

        Tasks = JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
    }
    #endregion
}