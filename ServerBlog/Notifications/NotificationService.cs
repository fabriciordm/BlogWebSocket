using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.WebSockets;



namespace ServerBlog.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly List<IWebSocketConnection> _sockets = new List<IWebSocketConnection>();

        public NotificationService()
        {

            NotificationServiceMessage("message");
            //var server = new WebSocketServer("ws://0.0.0.0:8181");


            //server.Start(socket =>
            //{
            //    socket.OnOpen = () => _sockets.Add(socket);
            //    socket.OnClose = () => _sockets.Remove(socket);
            //    socket.OnMessage = message => Console.WriteLine("Received: " + message);

            //});
        }
        public async Task NotificationServiceMessage(string mensagem)
        {

            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:8181/");
            httpListener.Start();
            Console.WriteLine("Servidor WebSocket iniciado.");

            while (true)
            {
                HttpListenerContext context = await httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    WebSocket webSocket = wsContext.WebSocket;

                    await HandleWebSocketConnection(webSocket, mensagem);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }


        }

        private static async Task HandleWebSocketConnection(WebSocket webSocket, string mensagem)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Mensagem recebida: {message}");

                // Enviar uma resposta de volta ao cliente
                var responseMessage = mensagem;
                var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public void NotifyAll(string message)
        {

            foreach (var socket in _sockets)
            {
                socket.Send(message);
            }
        }

        async Task  INotificationService.NotificationServiceMessage(string message)
        {
            NotificationServiceMessage(message);
        }
    }


    //public class NotificationService : INotificationService
    //{
    //    private readonly List<WebSocket> _sockets = new List<WebSocket>();
    //    private readonly HttpListener _httpListener;

    //    public NotificationService()
    //    {
    //        _httpListener = new HttpListener();
    //        _httpListener.Prefixes.Add("http://localhost:8181/");
    //        _httpListener.Start();
    //        Console.WriteLine("Servidor WebSocket iniciado.");
    //        Task.Run(AcceptWebSocketConnections);
    //    }

    //    private async Task AcceptWebSocketConnections()
    //    {
    //        while (true)
    //        {
    //            HttpListenerContext context = await _httpListener.GetContextAsync();
    //            if (context.Request.IsWebSocketRequest)
    //            {
    //                HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
    //                WebSocket webSocket = wsContext.WebSocket;
    //                _sockets.Add(webSocket);
    //                _ = Task.Run(() => HandleWebSocketConnection(webSocket));
    //            }
    //            else
    //            {
    //                context.Response.StatusCode = 400;
    //                context.Response.Close();
    //            }
    //        }
    //    }

    //    private async Task HandleWebSocketConnection(WebSocket webSocket)
    //    {
    //        var buffer = new byte[1024 * 4];
    //        while (webSocket.State == WebSocketState.Open)
    //        {
    //            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    //            if (result.MessageType == WebSocketMessageType.Text)
    //            {
    //                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
    //                Console.WriteLine("Received: " + message);
    //            }
    //            else if (result.MessageType == WebSocketMessageType.Close)
    //            {
    //                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
    //                _sockets.Remove(webSocket);
    //            }
    //        }
    //    }

    //    public async Task NotificationServiceMessage(string message)
    //    {
    //        var buffer = Encoding.UTF8.GetBytes(message);
    //        var tasks = new List<Task>();

    //        foreach (var socket in _sockets)
    //        {
    //            if (socket.State == WebSocketState.Open)
    //            {
    //                tasks.Add(socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None));
    //            }
    //        }

    //        await Task.WhenAll(tasks);
    //    }
    //}




}
