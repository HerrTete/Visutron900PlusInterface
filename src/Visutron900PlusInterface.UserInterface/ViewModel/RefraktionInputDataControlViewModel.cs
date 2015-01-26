using System.Windows.Input;

using Visutron900PlusInterface.UserInterface.WpfTools;

namespace Visutron900PlusInterface.UserInterface.ViewModel
{
    public class RefraktionInputDataControlViewModel : BaseViewModel
    {
        public double SphäreFernRechts { get; set; }

        public ICommand SendCommand { get; set; }
    }
}