using System.IO.Ports;

namespace Visutron900PlusInterface.Contracts
{
    public class SerialConnectionSettings
    {
        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public Parity Parity { get; set; }

        public int DataBits { get; set; }

        public StopBits StopBits { get; set; }
    }
}