using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    public class Visutron900PlusSerialAdapter
    {
        public Acknowlege SendData(RefraktionDataIn refraktionDataIn)
        {
            return Acknowlege.Acknowledge;
        }

        public RefraktionDataIn ReceiveData()
        {
            return new RefraktionDataIn();
        }
    }
}
