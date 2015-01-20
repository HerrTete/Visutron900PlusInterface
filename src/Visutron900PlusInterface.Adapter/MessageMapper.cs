using System.Globalization;
using System.Reflection;
using System.Text;

using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    internal class MessageMapper
    {
        internal static byte[] Map(RefraktionDataIn inputData)
        {
            var inputPattern = GetInputPattern();
            var output = new byte[inputPattern.Length * 2];

            var outputPos = 0;
            var valuePosition = 0;

            for (int i = 0; i < inputPattern.Length;i++)
            {
                output[outputPos] = inputPattern[i];
                outputPos++;
                if (inputPattern[i] == 0x3A)
                {
                    var valuePart = GetValue(inputData, valuePosition);
                    if (valuePart != null)
                    {
                        for (int j = 0; j < valuePart.Length; j++)
                        {
                            output[outputPos] = valuePart[j];
                            outputPos++;
                        }
                    }
                    valuePosition++;
                }
            }
            return output;
        }

        internal static RefraktionDataOut Map(byte[] inputData)
        {
            return new RefraktionDataOut();
        }

        private static byte[] GetValue(RefraktionDataIn inputData, int index)
        {
            byte[] retVal = null;

            switch (index)
            {
                case 0:
                {
                    var outputString = GetDoubleAlsString(inputData.SphäreFernRechts, true);
                    retVal = Encoding.ASCII.GetBytes(outputString);
                }
                    break;
            }

            return retVal;
        }

        private static string GetDoubleAlsString(double inputValue, bool plusSign = false)
        {
            var stringRepresentation = inputValue.ToString(CultureInfo.GetCultureInfo("en-US")).PadLeft(7);

            if (plusSign && inputValue > 0)
            {
                stringRepresentation = string.Format(" +{0}", inputValue.ToString(CultureInfo.GetCultureInfo("en-US")).PadLeft(5));
            }

            return stringRepresentation;
        }

        private static byte[] GetInputPattern()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream("Visutron900PlusInterface.Adapter.MessagePattern.TelegrammVisutron900_In_Pattern");

            var resouceBytes = new byte[stream.Length];

            stream.Read(resouceBytes, 0, resouceBytes.Length);

            stream.Close();
            stream.Dispose();

            return resouceBytes;
        }
    }
}
