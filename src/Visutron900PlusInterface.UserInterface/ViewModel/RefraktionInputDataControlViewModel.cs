using System.Windows.Input;
using Visutron900PlusInterface.Adapter.DTOs;
using Visutron900PlusInterface.UserInterface.WpfTools;

namespace Visutron900PlusInterface.UserInterface.ViewModel
{
    public class RefraktionInputDataControlViewModel : BaseViewModel
    {
        public double SphäreFernRechts { get; set; }
        public double SphäreNahRechts { get; set; }
        public double ZylinderRechts { get; set; }
        public int AchseRechts { get; set; }
        public double PrismaRechts { get; set; }
        public PrismaHorizontal GesamtprismaHorizontal { get; set; }
        public double PupillendistanzRechts { get; set; }
        public double SphäreFernLinks { get; set; }
        public double SphäreNahLinks { get; set; }
        public double ZylinderLinks { get; set; }
        public int AchseLinks { get; set; }
        public double PrismaLinks { get; set; }
        public PrismaVertikal GesamtprismaVertikal { get; set; }
        public double PupillendistanzLinks { get; set; }
        public double Pupillendistanz { get; set; }
        public string Patientenname { get; set; }
        public string PatientenID { get; set; }

        public ICommand SendCommand { get; set; }
    }
}