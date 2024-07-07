
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
       
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        var wsOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120)
        };

        app.UseWebSockets(wsOptions);

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/send")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        await Send(context, webSocket);
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                await next();
            }
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
    }

    private async Task Send(HttpContext context, WebSocket websocket)
    {
        var buffer = new byte[1024 * 24];
        WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
        if (result != null)
        {
            while (!result.CloseStatus.HasValue)
            {
                string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"notificação do blog:{msg}".ToString().ToUpper());
                await websocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"status de notificação enviado: {DateTime.UtcNow:f}".ToString().ToUpper())),
                    result.MessageType, result.EndOfMessage, System.Threading.CancellationToken.None);
                result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
              
            }
            await websocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, System.Threading.CancellationToken.None);

        }
    }
}




