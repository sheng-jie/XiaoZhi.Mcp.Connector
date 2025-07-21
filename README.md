# 小智 MCP 接入点连接器

本项目的目的是为了将使用 [MCP官方C# SDK](https://github.com/modelcontextprotocol/csharp-sdk) 开发的MCP Server 直接对接小智AI MCP 接入点而开发。其核心思想是自定义`ITransport` 来直接连接到小智AI的WebSocket接入点，实现MCP协议的无缝集成。

## 项目特性

-  **WebSocket传输**: 基于WebSocket协议实现与小智AI MCP接入点的连接
-  **自动重连**: 内置断线重连机制，确保连接稳定性
-  **日志支持**: 完整的日志记录，便于调试和监控
-  **依赖注入**: 完全支持.NET依赖注入容器
- **单会话模式**: 专为单一会话场景优化的服务架构

## 快速开始

### 安装

```bash
dotnet add package XiaoZhi.Mcp.Connector
```

### 基本用法

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XiaoZhi.Mcp.Connector;

var builder = Host.CreateApplicationBuilder(args);

// 配置小智MCP连接器
var xiaoZhiMcpAccessPoint = "wss://api.xiaozhi.me/mcp/?token=exxxxxxxxxx"; // 替换为实际的小智AI MCP接入点地址

builder.Services.AddMcpServer()
    .WithWebSocketServerTransport(xiaoZhiMcpAccessPoint)
    .WithTools<TodoTool>(); // 添加自定义工具

var host = builder.Build();
await host.RunAsync();
```

### 配置选项

```csharp
builder.Services.Configure<XiaoZhiWebSocketOptions>(options =>
{
    options.WebSocketUrl = "wss://api.xiaozhi.me/mcp/?token=exxxxxxxxxx"; // 替换为实际的小智AI MCP接入点地址
});

builder.Services.AddMcpServer()
    .WithWebSocketServerTransport()
    .WithTools<TodoTool>(); // 添加自定义工具
```

## 项目结构

```
src/XiaoZhi.Mcp.Connector/
├── WebSocketMcpServerBuilderExtensions.cs  # 扩展方法和服务注册
├── XiaoZhiWebSocketOptions.cs           # 配置选项
├── WebSocketServerTransport.cs            # WebSocket传输实现
├── SingleSessionMcpServerHostedService.cs # 单会话托管服务
└── Throw.cs                              # 参数验证工具
```

## 核心组件

### WebSocketServerTransport

自定义的`ITransport`实现，负责：
- WebSocket连接管理
- 消息收发处理
- 连接状态监控
- 异常处理

### WebSocketMcpServerBuilderExtensions

提供流畅的API来配置MCP服务器：

```csharp
public static IMcpServerBuilder WithWebSocketServerTransport(this IMcpServerBuilder builder, string webSocketUrl)
```

### SingleSessionMcpServerHostedService

后台服务，负责：
- MCP服务器生命周期管理
- 单一会话处理
- 服务启动和停止

## 依赖项

- ModelContextProtocol >= 0.3.0-preview.3
- Websocket.Client >= 5.2.0
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.Logging

## 示例项目

查看 `samples/Xiaozhi.Mcp.Connector.Demo/` 目录中的完整示例

## 配置说明

### WebSocket配置

- **WebSocketUrl**: 小智AI MCP接入点的WebSocket地址
- **ReconnectTimeout**: 重连超时时间（默认30秒）
- **IsReconnectionEnabled**: 是否启用自动重连（默认启用）

### 日志配置

项目使用Microsoft.Extensions.Logging进行日志记录，支持：
- 连接状态变化日志
- 重连事件日志
- 断开连接日志
- 错误和异常日志

## 开发和贡献

### 构建项目

```bash
dotnet build
```

### 运行示例

```bash
cd samples/Xiaozhi.Mcp.Connector.Demo
dotnet run
```
