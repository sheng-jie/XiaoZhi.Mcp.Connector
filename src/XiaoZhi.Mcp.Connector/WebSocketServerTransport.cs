using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using System.Text.Json;
using System.Threading.Channels;
using Websocket.Client;

namespace XiaoZhi.Mcp.Connector;

internal class WebSocketServerTransport : ITransport
{
    private readonly WebsocketClient websocketClient;
    private readonly ILogger<WebSocketServerTransport>? logger;

    public WebSocketServerTransport(WebsocketClient websocketClient, ILoggerFactory? loggerFactory)
    {
        this.websocketClient = websocketClient;
        this.logger = loggerFactory?.CreateLogger<WebSocketServerTransport>();
        // Start the WebSocket client connection
        websocketClient.Start();
    }

    public string? SessionId => throw new NotImplementedException();

    public ChannelReader<JsonRpcMessage> MessageReader
    {
        get
        {
            var channel = Channel.CreateUnbounded<JsonRpcMessage>();
            websocketClient.MessageReceived.Subscribe(msg =>
            {
                if (msg.MessageType != System.Net.WebSockets.WebSocketMessageType.Text)
                {
                    logger?.LogInformation("Received non-text message of type {MessageType}, ignoring.", msg.MessageType);
                    // Log or handle non-text messages if necessary
                    return;
                }

                if (string.IsNullOrEmpty(msg.Text))
                {
                    logger?.LogWarning("Received empty or null text message, ignoring.");
                    return;
                }

                try
                {
                    // Parse msg.Text to JsonRpcMessage
                    var jsonRpcMessage = JsonSerializer.Deserialize<JsonRpcMessage>(msg.Text);
                    if (jsonRpcMessage != null)
                    {
                        channel.Writer.TryWrite(jsonRpcMessage);
                    }
                    else
                    {
                        logger?.LogWarning("Deserialized JsonRpcMessage is null, ignoring.");
                    }
                }
                catch (JsonException ex)
                {
                    logger?.LogError(ex, "Failed to deserialize JsonRpcMessage from received text.");
                }
            });
            return channel.Reader;
        }
    }

    public ValueTask DisposeAsync()
    {
        this.websocketClient.Dispose();
        logger?.LogInformation("WebSocket client disposed.");
        return ValueTask.CompletedTask;
    }

    public Task SendMessageAsync(JsonRpcMessage message, CancellationToken cancellationToken = default)
    {
        this.websocketClient.Send(JsonSerializer.Serialize(message));

        return Task.CompletedTask;
    }
}
