using System.Collections.Generic;

using Visutron900PlusInterface.UserInterface.WpfTools;

namespace Visutron900PlusInterface.UserInterface.ViewModel
{
    public class SerialPortSettingsControlViewModel : BaseViewModel
    {
        public bool IsEnabled { get; set; }

        public string Port { get; set; }
        public int BaudRate { get; set; }
        public string Parity { get; set; }
        public int Databit { get; set; }
        public string Stopbit { get; set; }

        public List<string> Ports { get; set; }
        public List<int> BaudRates { get; set; }
        public List<string> Parities { get; set; }
        public List<int> Databits { get; set; }
        public List<string> Stopbits { get; set; }
    }
}
