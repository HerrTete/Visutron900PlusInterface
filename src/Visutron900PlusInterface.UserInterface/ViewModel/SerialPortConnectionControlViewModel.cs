using Visutron900PlusInterface.UserInterface.WpfTools;

namespace Visutron900PlusInterface.UserInterface.ViewModel
{
    public class SerialPortConnectionControlViewModel : BaseViewModel
    {
        public BaseCommand OpenCommand { get; set; }
        public BaseCommand CloseCommand { get; set; }
        public BaseCommand CreateCommand { get; set; }
    }
}