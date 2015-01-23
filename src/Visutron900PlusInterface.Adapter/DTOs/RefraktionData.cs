using System;

namespace Visutron900PlusInterface.Adapter.DTOs
{
    public class RefraktionData
    {
        [Index(0, ForcePlusSign = true)]
        public double SphäreFernRechts { get; set; }

        [Index(1, ForcePlusSign = true)]
        public double SphäreNahRechts { get; set; }

        [Index(2)]
        public double ZylinderRechts { get; set; }

        [Index(3)]
        public int AchseRechts { get; set; }

        [Index(4)]
        public double PrismaRechts { get; set; }

        [Index(4)]
        public PrismaHorizontal GesamtprismaHorizontal { get; set; }

        [Index(5)]
        public double AkkommodationsbreiteRechts { get; set; }
        
        [Index(6)]
        public double Visus_S_C_Rechts { get; set; }
        
        [Index(7)]
        public double Visus_C_C_Rechts { get; set; }

        [Index(8)]
        public double PupillendistanzRechts { get; set; }

        [Index(9, ForcePlusSign = true)]
        public double SphäreFernLinks { get; set; }

        [Index(10, ForcePlusSign = true)]
        public double SphäreNahLinks { get; set; }

        [Index(11)]
        public double ZylinderLinks { get; set; }

        [Index(12)]
        public int AchseLinks { get; set; }

        [Index(13)]
        public double PrismaLinks { get; set; }

        [Index(13)]
        public PrismaVertikal GesamtprismaVertikal { get; set; }

        [Index(14)]
        public double AkkommodationsbreiteLinks { get; set; }
        
        [Index(15)]
        public double Visus_S_C_Links { get; set; }
        
        [Index(16)]
        public double Visus_C_C_Links { get; set; }

        [Index(17)]
        public double PupillendistanzLinks { get; set; }

        [Index(18)]
        public int HornhautScheitelAbstand { get; set; }

        [Index(19)]
        public double Pupillendistanz { get; set; }
        
        [Index(20)]
        public double Fusionsbreite { get; set; }

        [Index(21)]
        public double Visus_S_C { get; set; }
        
        [Index(22)]
        public double Visus_C_C { get; set; }
        
        [Index(23)]
        public string Patientenname { get; set; }

        [Index(24)]
        public string PatientenID { get; set; }
        
        [Index(25)]
        [Index(26)]
        public DateTime RefraktionsZeitpunkt { get; set; }
    }
}