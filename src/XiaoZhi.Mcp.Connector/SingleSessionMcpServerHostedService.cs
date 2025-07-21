using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;


internal sealed class SingleSessionMcpServerHostedService(IMcpServer session, IHostApplicationLifetime? lifetime = null) : BackgroundService
{
    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await session.RunAsync(stoppingToken).ConfigureAwait(false);
        }
        finally
        {
            lifetime?.StopApplication();
        }
    }
}