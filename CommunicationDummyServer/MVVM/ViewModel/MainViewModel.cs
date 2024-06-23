using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using CommunicationDummyServer.Core;
using CommunicationDummyServer.MVVM.Model;

namespace CommunicationDummyServer.MVVM.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isRunning;
        private string _ipPortText;
        private string _expectedResponseText;
        public string IpPortPlaceholderTcp = "0.0.0.0:5000";
        public string IpPortPlaceholderSerial = "COM1:152000";
        public string ExpectedResponsePlaceholder = "Insert the expected responses to each of the incoming commands separated by new line";
        private ConnectionManager connectionManager;

        private string ip;
        private int port;
        private string comPort;
        private int baudRate;

        private List<string> commands = new List<string>();

        public MainViewModel()
        {
            ButtonText = "Run Server";
            _isRunning = false;

            connectionManager = new ConnectionManager();
            ConnectionTypes = new List<ConnectionType>((ConnectionType[])Enum.GetValues(typeof(ConnectionType)));
            IpPortText = IpPortPlaceholderTcp;
            ExpectedResponseText = ExpectedResponsePlaceholder;
            IpPortGotFocusCommand = new RelayCommand(OnIpPortGotFocus);
            IpPortLostFocusCommand = new RelayCommand(OnIpPortLostFocus);
            ExpectedResponseGotFocusCommand = new RelayCommand(OnExpectedResponseGotFocus);
            ExpectedResponseLostFocusCommand = new RelayCommand(OnExpectedResponseLostFocus);

            // Initialize commands with empty actions
            RunServerCommand = new RelayCommand(RunServer);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        private void ExtractIpPort(out string ip, out int port)
        {
            string[] ipPort = IpPortText.Split(':');
            ip = ipPort[0];
            port = int.Parse(ipPort[1]);
        }

        private void ExtractComPort(out string comPort, out int baudRate)
        {
            string[] comPortBaudRate = IpPortText.Split(':');
            comPort = comPortBaudRate[0];
            baudRate = int.Parse(comPortBaudRate[1]);
        }

        public string IpPortText
        {
            get => _ipPortText;
            set
            {
                _ipPortText = value;
                OnPropertyChanged();
            }
        }

        public string ExpectedResponseText
        {
            get => _expectedResponseText;
            set
            {
                _expectedResponseText = value;
                OnPropertyChanged();
            }
        }

        public ICommand IpPortGotFocusCommand { get; }
        public ICommand IpPortLostFocusCommand { get; }
        public ICommand ExpectedResponseGotFocusCommand { get; }
        public ICommand ExpectedResponseLostFocusCommand { get; }
        public ICommand RunServerCommand { get; }
        public ICommand SaveSettingsCommand { get; }

        private void OnIpPortGotFocus(object obj)
        {
            if (IpPortText == IpPortPlaceholderTcp)
            {
                IpPortText = string.Empty;
            }
        }

        private void OnIpPortLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(IpPortText))
            {
                IpPortText = IpPortPlaceholderTcp;
            }
        }

        private void OnExpectedResponseGotFocus(object obj)
        {
            if (ExpectedResponseText == ExpectedResponsePlaceholder)
            {
                ExpectedResponseText = string.Empty;
            }
        }

        private void OnExpectedResponseLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(ExpectedResponseText))
            {
                ExpectedResponseText = ExpectedResponsePlaceholder;
            }
        }

        private void RunServer(object obj)
        {
            ////TODO: REFACTOR LATER
            commands = ExpectedResponseText.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            if(!_isRunning && SelectedConnectionType == ConnectionType.Tcp)
            {
                ExtractIpPort(out ip, out port);
            }
            else if(!_isRunning && SelectedConnectionType == ConnectionType.Serial)
            {
                ExtractComPort(out comPort, out baudRate);
            }

            if(!_isRunning)
            {
                connectionManager.StartConnection(SelectedConnectionType, comPort, baudRate, port, commands);

                if(SelectedConnectionType == ConnectionType.Tcp)
                    IpPortText = connectionManager.GetCurrentIP() + ":" + port.ToString();
                else
                    IpPortText = comPort + ":" + baudRate.ToString();

                ButtonText = "Stop Server";
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
                connectionManager.StopConnection(SelectedConnectionType);
                ButtonText = "Run Server";
            }
        }

        private void SaveSettings(object obj)
        {
            // Empty action for SaveSettingsCommand
        }

        private ConnectionType _selectedConnectionType;

        public List<ConnectionType> ConnectionTypes { get; }

        public ConnectionType SelectedConnectionType
        {
            get { return _selectedConnectionType; }
            set
            {
                _selectedConnectionType = value;
                if(_selectedConnectionType == ConnectionType.Tcp)
                {
                    IpPortText = IpPortPlaceholderTcp;
                }
                else
                {
                    IpPortText = IpPortPlaceholderSerial;
                }
                OnPropertyChanged();
            }
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        /**
         * TODO:
         * - Disable the Drop down menu, command box and IP/Port text box when the server is running
         * - For Serial COM check if any COM ports are available. If not, ask 
         * the user to install HHD Virtual Serial Port or any other software
         * **/

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
