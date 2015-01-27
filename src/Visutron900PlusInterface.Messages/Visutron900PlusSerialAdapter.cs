using Visutron900PlusInterface.Messages.DTOs;

namespace Visutron900PlusInterface.Messages
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
