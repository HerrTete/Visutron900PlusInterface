using System;

namespace Visutron900PlusInterface.Adapter.DTOs
{
    public class RefraktionDataOut : RefraktionDataIn
    {
        public double AkkommodationsbreiteRechts { get; set; }
        public double AkkommodationsbreiteLinks { get; set; }
        public double Visus_S_C_Rechts { get; set; }
        public double Visus_C_C_Rechts { get; set; }
        public double Visus_S_C_Links { get; set; }
        public double Visus_C_C_Links { get; set; }

        public double HornhautScheitelAbstand { get; set; }
        public double Fusionsbreite { get; set; }
        public double Visus_S_C { get; set; }
        public double Visus_C_C { get; set; }

        public DateTime RefraktionsZeitpunkt { get; set; }
    }
}