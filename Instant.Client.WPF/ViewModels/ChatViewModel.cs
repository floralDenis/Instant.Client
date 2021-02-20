using Instant.Client.Core.Models.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Instant.Client.WPF.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public int ChatId { get; set; }
        public string Title { get; set; }
        public ChatTypes ChatType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
