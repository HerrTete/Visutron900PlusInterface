using System;
using Visutron900PlusInterface.Adapter.DTOs;
using Visutron900PlusInterface.UserInterface.WpfTools;

namespace Visutron900PlusInterface.UserInterface.ViewModel
{
    public class RefraktionResultDataControlViewModel : BaseViewModel
    {
        public double Sph�reFernRechts { get; set; }
        public double Sph�reNahRechts { get; set; }
        public double ZylinderRechts { get; set; }
        public int AchseRechts { get; set; }
        public double PrismaRechts { get; set; }
        public PrismaHorizontal GesamtprismaHorizontal { get; set; }
        public double AkkommodationsbreiteRechts { get; set; }
        public double Visus_S_C_Rechts { get; set; }
        public double Visus_C_C_Rechts { get; set; }
        public double PupillendistanzRechts { get; set; }
        public double Sph�reFernLinks { get; set; }
        public double Sph�reNahLinks { get; set; }
        public double ZylinderLinks { get; set; }
        public int AchseLinks { get; set; }
        public double PrismaLinks { get; set; }
        public PrismaVertikal GesamtprismaVertikal { get; set; }
        public double AkkommodationsbreiteLinks { get; set; }
        public double Visus_S_C_Links { get; set; }
        public double Visus_C_C_Links { get; set; }
        public double PupillendistanzLinks { get; set; }
        public int HornhautScheitelAbstand { get; set; }
        public double Pupillendistanz { get; set; }
        public double Fusionsbreite { get; set; }
        public double Visus_S_C { get; set; }
        public double Visus_C_C { get; set; }
        public string Patientenname { get; set; }
        public string PatientenID { get; set; }
        public DateTime RefraktionsZeitpunkt { get; set; }
    }
}