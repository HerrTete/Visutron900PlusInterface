namespace Visutron900PlusInterface.Adapter.DTOs
{
    public class RefraktionDataIn
    {
        [Index(5)]
        [Index(6)]
        [Index(7)]
        [Index(14)]
        [Index(15)]
        [Index(16)]
        [Index(20)]
        [Index(21)]
        [Index(22)]
        public double DoubleDefault { get; private set; }

        [Index(18)]
        public int IntDefault { get; private set; }

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

        [Index(17)]
        public double PupillendistanzLinks { get; set; }

        [Index(4)]
        public PrismaHorizontal GesamtprismaHorizontal { get; set; }

        [Index(13)]
        public PrismaVertikal GesamtprismaVertikal { get; set; }


        [Index(19)]
        public double Pupillendistanz { get; set; }


        [Index(23)]
        public string Patientenname { get; set; }

        [Index(24)]
        public string PatientenID { get; set; }
    }
}