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



namespace ClientBLog.Notifications
{
    public class NotificationService : INotificationService
    {  
        public async Task NotificationServiceMessage()
        {
          using (ClientWebSocket client = new ClientWebSocket())
            {

                Uri serviceUri = new Uri("ws://localhost:5000/send");
                var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(120));

                try
                {
                    await client.ConnectAsync(serviceUri, cts.Token);
                   
                    Console.WriteLine("Conectado ao servidor WebSocket");

                    while (client.State == WebSocketState.Open)
                    {
                        
                        string message = Console.ReadLine();

                        if (!string.IsNullOrEmpty(message))
                        {
                            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await client.SendAsync(bytesToSend, WebSocketMessageType.Text, true, cts.Token);

                            var responseBuffer = new byte[1024];
                            var offset = 0;
                            var packet = 1024;

                            while (true)
                            {
                                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(responseBuffer, offset, packet);
                                WebSocketReceiveResult response = await client.ReceiveAsync(bytesReceived, cts.Token);

                                var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                                Console.WriteLine($"Received message: {responseMessage}");

                                if (response.EndOfMessage)
                                {
                                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Encerrando conexão", CancellationToken.None);

                                    break;
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task NotificationServiceMessage(string message)
        {
            using (ClientWebSocket client = new ClientWebSocket())
            {
                Uri serviceUri = new Uri("ws://localhost:5000/send");
                var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(120));

                try
                {
                    await client.ConnectAsync(serviceUri, cts.Token);

                    Console.WriteLine("Conectado ao servidor WebSocket");

                    if (client.State == WebSocketState.Open)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await client.SendAsync(bytesToSend, WebSocketMessageType.Text, true, cts.Token);

                            var responseBuffer = new byte[1024];
                            var offset = 0;
                            var packet = 1024;

                            while (true)
                            {
                                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(responseBuffer, offset, packet);
                                WebSocketReceiveResult response = await client.ReceiveAsync(bytesReceived, cts.Token);

                                var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                                Console.WriteLine($"Received message: {responseMessage}");

                                if (response.EndOfMessage)
                                {
                                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Encerrando conexão", CancellationToken.None);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }


}
