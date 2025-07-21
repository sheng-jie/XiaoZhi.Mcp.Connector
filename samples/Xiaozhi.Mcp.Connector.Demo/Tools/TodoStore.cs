using System;
using System.Collections.Generic;
using System.Linq;

namespace Xiaozhi.Mcp.Connector.Demo.Tools;

/// <summary>
/// Provides storage and operations for TodoItems
/// </summary>
public class TodoStore
{
    private readonly List<TodoItem> _todos = new();
    private int _nextId = 1;

    /// <summary>
    /// Gets all todo items
    /// </summary>
    /// <returns>List of all todo items</returns>
    public List<TodoItem> GetAll()
    {
        return _todos.ToList();
    }

    /// <summary>
    /// Gets a specific todo item by ID
    /// </summary>
    /// <param name="id">ID of the todo item to retrieve</param>
    /// <returns>The todo item if found, otherwise null</returns>
    public TodoItem? GetById(int id)
    {
        return _todos.FirstOrDefault(t => t.Id == id);
    }

    /// <summary>
    /// Creates a new todo item
    /// </summary>
    /// <param name="title">Title of the todo item</param>
    /// <param name="description">Description of the todo item</param>
    /// <returns>The newly created todo item</returns>
    public TodoItem Create(string title, string description = "")
    {
        var todo = new TodoItem
        {
            Id = _nextId++,
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _todos.Add(todo);
        return todo;
    }

    /// <summary>
    /// Updates an existing todo item
    /// </summary>
    /// <param name="id">ID of the todo item to update</param>
    /// <param name="title">New title (optional)</param>
    /// <param name="description">New description (optional)</param>
    /// <param name="isCompleted">New completion status (optional)</param>
    /// <returns>The updated todo item if found, otherwise null</returns>
    public TodoItem? Update(int id, string? title = null, string? description = null, bool? isCompleted = null)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return null;
        }

        if (title != null)
        {
            todo.Title = title;
        }

        if (description != null)
        {
            todo.Description = description;
        }

        if (isCompleted.HasValue && todo.IsCompleted != isCompleted.Value)
        {
            todo.IsCompleted = isCompleted.Value;
            todo.CompletedAt = isCompleted.Value ? DateTime.UtcNow : null;
        }

        return todo;
    }

    /// <summary>
    /// Deletes a todo item by ID
    /// </summary>
    /// <param name="id">ID of the todo item to delete</param>
    /// <returns>True if the item was deleted, false if not found</returns>
    public bool Delete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return false;
        }

        return _todos.Remove(todo);
    }

    /// <summary>
    /// Gets all completed todo items
    /// </summary>
    /// <returns>List of completed todo items</returns>
    public List<TodoItem> GetCompleted()
    {
        return _todos.Where(t => t.IsCompleted).ToList();
    }

    /// <summary>
    /// Gets all incomplete todo items
    /// </summary>
    /// <returns>List of incomplete todo items</returns>
    public List<TodoItem> GetIncomplete()
    {
        return _todos.Where(t => !t.IsCompleted).ToList();
    }
}