using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Serilog;
using Xiaozhi.Mcp.Connector.Demo.Tools;
using XiaoZhi.Mcp.Connector;

Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Verbose() // Capture all log levels
           .WriteTo.Console(standardErrorFromLevel: Serilog.Events.LogEventLevel.Verbose)
           .CreateLogger();

try
{
    Log.Information("Starting server...");

    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddSerilog();

    // Register TodoStore as a singleton service
    builder.Services.AddSingleton<TodoStore>();

    builder.Services.Configure<McpServerOptions>(options =>
    {
        options.ServerInfo = new Implementation
        {
            Name = "TodoMcpServer",
            Version = "0.1.0",
            Title = "Todo MCP Server"
        };
        options.ServerInstructions = "A mcp server for managing todo items";
    });

    var xiaoZhiMcpAccessPoint = "wss://api.xiaozhi.me/mcp/?token=xxxx";

    builder.Services.AddMcpServer()
        .WithWebSocketServerTransport(xiaoZhiMcpAccessPoint)
        .WithTools<TodoTool>();

    var app = builder.Build();

    await app.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}