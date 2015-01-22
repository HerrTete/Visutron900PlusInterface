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

            for (int i = 0; i < inputPattern.Length; i++)
            {
                output[outputPos] = inputPattern[i];
                outputPos++;
                if (inputPattern[i] == 0x3A)
                {
                    var valuePart = GetValue(inputData, valuePosition);
                    AppendValues(ref output, ref outputPos, valuePart);
                    valuePosition++;
                }
            }
            return output;
        }

        private static void AppendValues(ref byte[] output, ref int outputPos, byte[] valuePart)
        {
            if (valuePart != null)
            {
                for (int j = 0; j < valuePart.Length; j++)
                {
                    output[outputPos] = valuePart[j];
                    outputPos++;
                }
            }
        }

        internal static RefraktionDataOut Map(byte[] inputData)
        {
            return new RefraktionDataOut();
        }

        private static byte[] GetValue(RefraktionDataIn inputData, int index)
        {
            string outputString = null;

            switch (index)
            {
                case 0:
                    {
                        outputString = GetDoubleAlsString(inputData.SphäreFernRechts, true);
                        break;
                    }
                case 1:
                    {
                        outputString = GetDoubleAlsString(inputData.SphäreNahRechts, true);
                        break;
                    }
                case 2:
                    {
                        outputString = GetDoubleAlsString(inputData.ZylinderRechts);
                        break;
                    }
                case 3:
                    {
                        outputString = GetIntAlsString(inputData.AchseRechts);
                        break;
                    }
                case 4:
                    {
                        outputString = GetDoubleAlsString(inputData.PrismaRechts);
                        outputString += " " + inputData.GesamtprismaHorizontal;
                        break;
                    }
                case 5:
                case 6:
                case 7:
                    {
                        outputString = GetDoubleAlsString(0.0);
                        break;
                    }
                case 8:
                    {
                        outputString = GetDoubleAlsString(inputData.PupillendistanzRechts);
                        break;
                    }
                case 9:
                    {
                        outputString = GetDoubleAlsString(inputData.SphäreFernLinks, true);
                        break;
                    }
                case 10:
                    {
                        outputString = GetDoubleAlsString(inputData.SphäreNahLinks, true);
                        break;
                    }
                case 11:
                    {
                        outputString = GetDoubleAlsString(inputData.ZylinderLinks);
                        break;
                    }
                case 12:
                    {
                        outputString = GetIntAlsString(inputData.AchseLinks);
                        break;
                    }
                case 13:
                    {
                        outputString = GetDoubleAlsString(inputData.PrismaLinks);
                        outputString += " " + inputData.GesamtprismaVertikal;
                        break;
                    }
                case 14:
                case 15:
                case 16:
                    {
                        outputString = GetDoubleAlsString(0.0);
                        break;
                    }
                case 17:
                    {
                        outputString = GetDoubleAlsString(inputData.PupillendistanzLinks);
                        break;
                    }
                case 18:
                    {
                        outputString = GetIntAlsString(0);
                        break;
                    }
                case 19:
                    {
                        outputString = GetDoubleAlsString(inputData.Pupillendistanz);
                        break;
                    }
                case 20:
                case 21:
                case 22:
                    {
                        outputString = GetDoubleAlsString(0.0);
                        break;
                    }
                case 23:
                    {
                        outputString = inputData.Patientenname;
                        break;
                    }
                case 24:
                    {
                        outputString = inputData.PatientenID;
                        break;
                    }
            }

            return outputString == null ? null : Encoding.ASCII.GetBytes(outputString);
        }

        private static string GetDoubleAlsString(double inputValue, bool plusSign = false)
        {
            var stringRepresentation = inputValue.ToString("0.00").PadLeft(7);

            if (plusSign && inputValue > 0)
            {
                stringRepresentation = string.Format(" +{0}", inputValue.ToString("0.00").PadLeft(5));
            }
            if (inputValue < 0)
            {

                stringRepresentation = string.Format(" -{0}", (inputValue * -1).ToString("0.00").PadLeft(5));
            }

            return stringRepresentation.Replace(',', '.');
        }

        private static string GetIntAlsString(int inputValue)
        {
            var stringRepresentation = inputValue.ToString(CultureInfo.InvariantCulture).PadLeft(7);
            return stringRepresentation.Replace(',', '.');
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
