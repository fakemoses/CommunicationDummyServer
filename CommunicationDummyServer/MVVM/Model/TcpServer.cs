using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationDummyServer.MVVM.Model
{
    public class TcpServer
    {
        private TcpListener _listener;
        private bool _isRunning;

        public TcpServer(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task Start()
        {
            _listener.Start();
            _isRunning = true;
            ////Console.WriteLine($"Telnet server started on port {_listener.LocalEndpoint}");

            while (_isRunning)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                HandleClient(client);
            }
        }

        private async void HandleClient(TcpClient client)
        {
            ////Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            try
            {
                //// INCLUDE A LOGIN OPTION HERE LATER
                await writer.WriteLineAsync("Welcome to the Telnet server!");
                //await writer.WriteLineAsync("Type 'exit' to disconnect.");

                string request;
                do
                {
                    request = await reader.ReadLineAsync();
                    ////Console.WriteLine($"Received from {client.Client.RemoteEndPoint}: {request}");

                    // Example command handling
                    if (request.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        await writer.WriteLineAsync("Goodbye!");
                        break;
                    }
                    else
                    {
                        await writer.WriteLineAsync($"You said: {request}");
                    }
                } while (request != null);
            }
            catch (Exception ex)
            {
                ////Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                reader.Close();
                writer.Close();
                client.Close();
                ////Console.WriteLine($"Client disconnected: {client.Client.RemoteEndPoint}");
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            ////Console.WriteLine("Telnet server stopped.");
        }
    }

}
