# С�� MCP �����������

����Ŀ��Ŀ����Ϊ�˽�ʹ�� [MCP�ٷ�C# SDK](https://github.com/modelcontextprotocol/csharp-sdk) ������MCP Server ֱ�ӶԽ�С��AI MCP �����������������˼�����Զ���`ITransport` ��ֱ�����ӵ�С��AI��WebSocket����㣬ʵ��MCPЭ����޷켯�ɡ�

## ��Ŀ����

-  **WebSocket����**: ����WebSocketЭ��ʵ����С��AI MCP����������
-  **�Զ�����**: ���ö����������ƣ�ȷ�������ȶ���
-  **��־֧��**: ��������־��¼�����ڵ��Ժͼ��
-  **����ע��**: ��ȫ֧��.NET����ע������
- **���Ựģʽ**: רΪ��һ�Ự�����Ż��ķ���ܹ�

## ���ٿ�ʼ

### ��װ

```bash
dotnet add package XiaoZhi.Mcp.Connector
```

### �����÷�

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XiaoZhi.Mcp.Connector;

var builder = Host.CreateApplicationBuilder(args);

// ����С��MCP������
var xiaoZhiMcpAccessPoint = "wss://api.xiaozhi.me/mcp/?token=exxxxxxxxxx"; // �滻Ϊʵ�ʵ�С��AI MCP������ַ

builder.Services.AddMcpServer()
    .WithWebSocketServerTransport(xiaoZhiMcpAccessPoint)
    .WithTools<TodoTool>(); // ����Զ��幤��

var host = builder.Build();
await host.RunAsync();
```

### ����ѡ��

```csharp
builder.Services.Configure<XiaoZhiWebSocketOptions>(options =>
{
    options.WebSocketUrl = "wss://api.xiaozhi.me/mcp/?token=exxxxxxxxxx"; // �滻Ϊʵ�ʵ�С��AI MCP������ַ
});

builder.Services.AddMcpServer()
    .WithWebSocketServerTransport()
    .WithTools<TodoTool>(); // ����Զ��幤��
```

## ��Ŀ�ṹ

```
src/XiaoZhi.Mcp.Connector/
������ WebSocketMcpServerBuilderExtensions.cs  # ��չ�����ͷ���ע��
������ XiaoZhiWebSocketOptions.cs           # ����ѡ��
������ WebSocketServerTransport.cs            # WebSocket����ʵ��
������ SingleSessionMcpServerHostedService.cs # ���Ự�йܷ���
������ Throw.cs                              # ������֤����
```

## �������

### WebSocketServerTransport

�Զ����`ITransport`ʵ�֣�����
- WebSocket���ӹ���
- ��Ϣ�շ�����
- ����״̬���
- �쳣����

### WebSocketMcpServerBuilderExtensions

�ṩ������API������MCP��������

```csharp
public static IMcpServerBuilder WithWebSocketServerTransport(this IMcpServerBuilder builder, string webSocketUrl)
```

### SingleSessionMcpServerHostedService

��̨���񣬸���
- MCP�������������ڹ���
- ��һ�Ự����
- ����������ֹͣ

## ������

- ModelContextProtocol >= 0.3.0-preview.3
- Websocket.Client >= 5.2.0
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.Logging

## ʾ����Ŀ

�鿴 `samples/Xiaozhi.Mcp.Connector.Demo/` Ŀ¼�е�����ʾ��

## ����˵��

### WebSocket����

- **WebSocketUrl**: С��AI MCP������WebSocket��ַ
- **ReconnectTimeout**: ������ʱʱ�䣨Ĭ��30�룩
- **IsReconnectionEnabled**: �Ƿ������Զ�������Ĭ�����ã�

### ��־����

��Ŀʹ��Microsoft.Extensions.Logging������־��¼��֧�֣�
- ����״̬�仯��־
- �����¼���־
- �Ͽ�������־
- ������쳣��־

## �����͹���

### ������Ŀ

```bash
dotnet build
```

### ����ʾ��

```bash
cd samples/Xiaozhi.Mcp.Connector.Demo
dotnet run
```
