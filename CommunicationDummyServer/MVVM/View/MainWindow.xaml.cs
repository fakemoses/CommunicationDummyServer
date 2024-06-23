using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunicationDummyServer.MVVM.ViewModel;

namespace CommunicationDummyServer.MVVM.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var viewModel = DataContext as MainViewModel;
            if (textBox != null && viewModel != null)
            {
                if (textBox.Name == "IpPortTextBox" && textBox.Text == viewModel.IpPortPlaceholderTcp)
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                }
                else if (textBox.Name == "ExpectedResponseTextBox" && textBox.Text == viewModel.ExpectedResponsePlaceholder)
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var viewModel = DataContext as MainViewModel;
            if (textBox != null && viewModel != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox.Name == "IpPortTextBox")
                    {
                        textBox.Text = viewModel.IpPortPlaceholderTcp;
                        textBox.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                    else if (textBox.Name == "ExpectedResponseTextBox")
                    {
                        textBox.Text = viewModel.ExpectedResponsePlaceholder;
                        textBox.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                }
            }
        }
    }
}
