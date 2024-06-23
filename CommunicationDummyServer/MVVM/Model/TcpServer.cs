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
        private CancellationTokenSource _cancellationTokenSource;
        private TcpClient _client;


        public TcpServer(int port)
        {
            GetcurrentIP = IPAddress.Any.ToString();
            IPAddress ipAddress = System.Net.IPAddress.Parse(GetcurrentIP);
            _listener = new TcpListener(ipAddress, port);
        }

        public async Task Start()
        {
            _listener.Start();

            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _client = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(_client, _cancellationTokenSource.Token));
            }
        }

        private async void HandleClient(TcpClient client, CancellationToken token)
        {

            using (client)
            {
                var buffer = new byte[1024];
                var stream = client.GetStream();

                while (!token.IsCancellationRequested)
                {
                    if (!stream.DataAvailable)
                    {
                        await Task.Delay(100, token);
                        continue;
                    }

                    try
                    {
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        if (bytesRead == 0)
                        {
                            break;
                        }

                        var message = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        // Echo the message back to the client
                        await stream.WriteAsync(buffer, 0, bytesRead, token);
                    }
                    catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _cancellationTokenSource.Cancel();
            _listener.Stop();
        }

        private string _getcurrentIP;

        public string GetcurrentIP
        {
            get { return _getcurrentIP; }
            set { _getcurrentIP = value; }
        }
    }

}
