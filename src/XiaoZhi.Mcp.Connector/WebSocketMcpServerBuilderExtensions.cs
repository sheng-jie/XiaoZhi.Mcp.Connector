using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Websocket.Client;

namespace XiaoZhi.Mcp.Connector;

public static class WebSocketMcpServerBuilderExtensions
{
    public static IMcpServerBuilder WithWebSocketServerTransport(this IMcpServerBuilder builder, string websocketUrl)
    {
        Throw.IfNull(builder);

        builder.Services.Configure<XiaoZhiWebSocketOptions>(options =>
        {
            options.WebSocketUrl = websocketUrl;
        });

        return builder.WithWebSocketServerTransport();
    }
    public static IMcpServerBuilder WithWebSocketServerTransport(this IMcpServerBuilder builder)
    {
        Throw.IfNull(builder);

        AddSingleSessionServerDependencies(builder.Services);
        AddWebSocketClient(builder.Services);

        builder.Services.AddSingleton<ITransport>(sp =>
        {
            var loggerFactory = sp.GetService<ILoggerFactory>();
            var client = sp.GetRequiredService<WebsocketClient>();
            return new WebSocketServerTransport(client, loggerFactory);
        });

        return builder;
    }

    public static void AddWebSocketClient(IServiceCollection services)
    {
        services.AddSingleton<WebsocketClient>(sp =>
        {
            var websocketOptions = sp.GetRequiredService<IOptions<XiaoZhiWebSocketOptions>>();

            Throw.IfNullOrWhiteSpace(websocketOptions.Value.WebSocketUrl, nameof(websocketOptions.Value.WebSocketUrl));

            var loggerFactory = sp.GetService<ILoggerFactory>();

            var client = new WebsocketClient(
                url: new Uri(websocketOptions.Value.WebSocketUrl)
                //clientFactory: factory
                );

            client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            client.IsReconnectionEnabled = true;

            client.ReconnectionHappened.Subscribe(info =>
            {
                loggerFactory?.CreateLogger<WebsocketClient>()
                    .LogInformation("Reconnection happened: {Info}", info);
            });

            client.DisconnectionHappened.Subscribe(info =>
            {
                loggerFactory?.CreateLogger<WebsocketClient>()
                    .LogInformation("Disconnection happened: {Info}", info);
            });

            return client;
        });
    }

    private static void AddSingleSessionServerDependencies(IServiceCollection services)
    {
        services.AddHostedService<SingleSessionMcpServerHostedService>();

        services.TryAddSingleton(services =>
        {
            ITransport serverTransport = services.GetRequiredService<ITransport>();
            IOptions<McpServerOptions> options = services.GetRequiredService<IOptions<McpServerOptions>>();
            ILoggerFactory? loggerFactory = services.GetService<ILoggerFactory>();
            return McpServerFactory.Create(serverTransport, options.Value, loggerFactory, services);
        });
    }
}

