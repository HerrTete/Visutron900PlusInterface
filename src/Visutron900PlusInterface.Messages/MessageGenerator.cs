using System.Collections.Generic;
using System.Reflection;

namespace Visutron900PlusInterface.Messages
{
    internal class MessageGenerator
    {
        internal byte[] GeneratoreMessage(List<byte[]> values)
        {
            var template = GetInputPattern();
            var returnBuffer = new List<byte>();

            var valuePosition = 0;

            foreach (var templateByte in template)
            {
                returnBuffer.Add(templateByte);

                if (templateByte == ':')
                {
                    returnBuffer.AddRange(values[valuePosition]);
                    valuePosition++;
                }
            }

            return returnBuffer.ToArray();
        }

        private byte[] GetInputPattern()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream("Visutron900PlusInterface.Messages.MessagePattern.TelegrammVisutron900_In_Pattern");

            var resouceBytes = new byte[stream.Length];

            stream.Read(resouceBytes, 0, resouceBytes.Length);

            stream.Close();
            stream.Dispose();

            return resouceBytes;
        }

    }
}