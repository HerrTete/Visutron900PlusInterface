using System.Text;

using Visutron900PlusInterface.Messages.DTOs;

namespace Visutron900PlusInterface.Messages
{
    public class MessageMapper
    {
        private readonly Encoding _encoding;

        public MessageMapper(Encoding encoding)
        {
            _encoding = encoding;
        }

        public byte[] Map(RefraktionData inputData)
        {
            var messageGenerator = new MessageGenerator();
            var valueReader = new ValueReader(_encoding);
            var values = valueReader.ReadValues(inputData);
            return messageGenerator.GeneratoreMessage(values);
        }

        public RefraktionData Map(byte[] inputData)
        {
            var valueReader = new ValueReader(_encoding);
            var refraktionDataGenerator = new RefraktionDataGenerator();
            var values = valueReader.ReadValues(inputData);
            return refraktionDataGenerator.GenerateRefraktionData(values);
        }
    }
}
