using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationDummyServer.MVVM.ViewModel
{
    public class OptionViewModel : INotifyPropertyChanged
    {

        public OptionViewModel()
        {
            //// Do nothing
        }

        /// <summary>
        /// Inotify property items
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
