using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationDummyServer.MVVM.Model
{
    public class SerialServer
    {
        private SerialPort _serialPort;
        private bool _isRunning;

        public SerialServer(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void Start()
        {
            try
            {
                _serialPort.Open();
                _isRunning = true;
                Console.WriteLine($"Serial server started on {_serialPort.PortName}. Waiting for data...");

                while (_isRunning)
                {
                    // Keep the server running
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting serial server: {ex.Message}");
                Stop();
            }
        }

        public void Stop()
        {
            _isRunning = false;
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            Console.WriteLine("Serial server stopped.");
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                string data = serialPort.ReadExisting();
                Console.WriteLine($"Data received: {data}");

                // Process the received data here

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling serial data: {ex.Message}");
            }
        }
    }
}
