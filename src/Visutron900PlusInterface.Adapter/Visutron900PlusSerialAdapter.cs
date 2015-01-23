using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    public class Visutron900PlusSerialAdapter
    {
        public Acknowlege SendData(RefraktionData refraktionData)
        {
            return Acknowlege.Acknowledge;
        }

        public RefraktionData ReceiveData()
        {
            return new RefraktionData();
        }
    }
}
