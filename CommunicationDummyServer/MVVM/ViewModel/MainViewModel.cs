using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        public string IpPortPlaceholder = "IP:PORT";
        public string ExpectedResponsePlaceholder = "Insert the expected responses to each of the incoming commands separated by new line";
        private ConnectionManager connectionManager;

        public MainViewModel()
        {
            ButtonText = "Run Server";
            _isRunning = false;

            connectionManager = new ConnectionManager();
            ConnectionTypes = new List<ConnectionType>((ConnectionType[])Enum.GetValues(typeof(ConnectionType)));
            IpPortText = IpPortPlaceholder;
            ExpectedResponseText = ExpectedResponsePlaceholder;
            IpPortGotFocusCommand = new RelayCommand(OnIpPortGotFocus);
            IpPortLostFocusCommand = new RelayCommand(OnIpPortLostFocus);
            ExpectedResponseGotFocusCommand = new RelayCommand(OnExpectedResponseGotFocus);
            ExpectedResponseLostFocusCommand = new RelayCommand(OnExpectedResponseLostFocus);

            // Initialize commands with empty actions
            RunServerCommand = new RelayCommand(RunServer);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
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
            if (IpPortText == IpPortPlaceholder)
            {
                IpPortText = string.Empty;
            }
        }

        private void OnIpPortLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(IpPortText))
            {
                IpPortText = IpPortPlaceholder;
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
            if(!_isRunning)
            {
                connectionManager.StartConnection(ConnectionType.Tcp, "5000", 152000, 6000);
                IpPortText = connectionManager.GetCurrentIP() + ":5000";
                ButtonText = "Stop Server";
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
                connectionManager.StopConnection(SelectedConnectionType);
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
