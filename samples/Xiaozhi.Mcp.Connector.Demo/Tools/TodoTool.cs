using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Xiaozhi.Mcp.Connector.Demo.Tools;


[McpServerToolType]
/// <summary> 
/// A tool for managing todo items
/// </summary>
internal class TodoTool
{
    private readonly TodoStore _todoStore;

    public TodoTool(TodoStore todoStore)
    {
        _todoStore = todoStore;
    }

    /// <summary>
    /// Gets all todo items
    /// </summary>
    /// <returns>List of all todo items</returns>
    [McpServerTool(Name = "list_todos"), Description("Gets all todo items")]
    public List<TodoItem> GetAllTodos()
    {
        return _todoStore.GetAll();
    }

    /// <summary>
    /// Gets a specific todo item by ID
    /// </summary>
    /// <param name="id">ID of the todo item to retrieve</param>
    /// <returns>The todo item if found, otherwise null</returns>
    [McpServerTool(Name = "get_todo"), Description("Gets a specific todo item by ID")]
    public TodoItem? GetTodoById(int id)
    {
        return _todoStore.GetById(id);
    }

    /// <summary>
    /// Creates a new todo item
    /// </summary>
    /// <param name="title">Title of the todo item</param>
    /// <param name="description">Description of the todo item</param>
    /// <returns>The newly created todo item</returns>
    [McpServerTool(Name = "create_todo"), Description("Creates a new todo item")]
    public TodoItem CreateTodo(string title, string description = "")
    {
        return _todoStore.Create(title, description);
    }

    /// <summary>
    /// Updates an existing todo item
    /// </summary>
    /// <param name="id">ID of the todo item to update</param>
    /// <param name="title">New title (optional)</param>
    /// <param name="description">New description (optional)</param>
    /// <param name="isCompleted">New completion status (optional)</param>
    /// <returns>The updated todo item if found, otherwise null</returns>
    [McpServerTool(Name = "update_todo"), Description("Updates an existing todo item")]
    public TodoItem? UpdateTodo(int id, string? title = null, string? description = null, bool? isCompleted = null)
    {
        return _todoStore.Update(id, title, description, isCompleted);
    }

    /// <summary>
    /// Deletes a todo item by ID
    /// </summary>
    /// <param name="id">ID of the todo item to delete</param>
    /// <returns>True if the item was deleted, false if not found</returns>
    [McpServerTool(Name = "delete_todo"), Description("Deletes a todo item by ID")]
    public bool DeleteTodo(int id)
    {
        return _todoStore.Delete(id);
    }

    /// <summary>
    /// Marks a todo item as completed
    /// </summary>
    /// <param name="id">ID of the todo item to mark as completed</param>
    /// <returns>The updated todo item if found, otherwise null</returns>
    [McpServerTool(Name = "complete_todo"), Description("Marks a todo item as completed")]
    public TodoItem? CompleteTodo(int id)
    {
        return _todoStore.Update(id, isCompleted: true);
    }

    /// <summary>
    /// Gets all completed todo items
    /// </summary>
    /// <returns>List of completed todo items</returns>
    [McpServerTool(Name = "list_completed_todos"), Description("Gets all completed todo items")]
    public List<TodoItem> GetCompletedTodos()
    {
        return _todoStore.GetCompleted();
    }

    /// <summary>
    /// Gets all incomplete todo items
    /// </summary>
    /// <returns>List of incomplete todo items</returns>
    [McpServerTool(Name = "list_incomplete_todos"), Description("Gets all incomplete todo items")]
    public List<TodoItem> GetIncompleteTodos()
    {
        return _todoStore.GetIncomplete();
    }
}