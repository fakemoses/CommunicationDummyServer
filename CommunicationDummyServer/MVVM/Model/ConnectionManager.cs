using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationDummyServer.MVVM.Model
{
    enum ConnectionStatus
    {
        Disconnected,
        Connected
    }

    /// <summary>
    /// Type of connections/protocols that can be used
    /// </summary>
    public enum ConnectionType
    {
        Tcp,
        Serial
    }

    class ConnectionManager
    {
        private TcpServer tcpServer;
        private SerialServer serialServer;

        public ConnectionManager()
        {
            // Initialize the connection manager
        }

        public async void StartConnection(ConnectionType connectionType, string portName, int baudRate, int port)
        {
            switch (connectionType)
            {
                case ConnectionType.Tcp:
                    tcpServer = new TcpServer(port);
                    await tcpServer.Start();
                    break;
                case ConnectionType.Serial:
                    serialServer = new SerialServer(portName, baudRate);
                    serialServer.Start();
                    break;
                default:
                    break;
            }
        }

        public async void StopConnection(ConnectionType connectionType)
        {
            switch (connectionType)
            {
                case ConnectionType.Tcp:
                    tcpServer.Stop();
                    break;
                case ConnectionType.Serial:
                    serialServer.Stop();
                    break;
                default:
                    break;
            }
        }
    }
}
