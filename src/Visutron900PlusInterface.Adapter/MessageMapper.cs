using System.Text;
using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    internal class MessageMapper
    {
        private static readonly Encoding encoding;

        static MessageMapper()
        {
            encoding = Encoding.ASCII;
        }

        internal static byte[] Map(RefraktionData inputData)
        {
            var messageGenerator = new MessageGenerator();
            var valueReader = new ValueReader(encoding);
            var values = valueReader.ReadValues(inputData);
            return messageGenerator.GeneratoreMessage(values);
        }

        internal static RefraktionData Map(byte[] inputData)
        {
            var valueReader = new ValueReader(encoding);
            var refraktionDataGenerator = new RefraktionDataGenerator();
            var values = valueReader.ReadValues(inputData);
            return refraktionDataGenerator.GenerateRefraktionData(values);
        }
    }
}
