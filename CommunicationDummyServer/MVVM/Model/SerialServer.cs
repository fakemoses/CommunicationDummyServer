using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationDummyServer.MVVM.Model
{
    public class SerialServer
    {
        private SerialPort _serialPort;
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        public SerialServer(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _serialPort.Open();
                _isRunning = true;

                Task.Run(() => RunServer(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                Stop();
            }
        }

        private void RunServer(CancellationToken token)
        {
            try
            {
                while (_isRunning && !token.IsCancellationRequested)
                {
                    // Keep the server running
                    Thread.Sleep(100); // Sleep to prevent high CPU usage
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _cancellationTokenSource.Cancel();

            if (_serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                string data = serialPort.ReadExisting();
                // Process the received data here
                // Echo back the received data
                serialPort.Write(data);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
