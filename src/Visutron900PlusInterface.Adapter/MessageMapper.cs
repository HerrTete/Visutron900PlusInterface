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
                if (inputPattern[i] == ':')
                {
                    var valuePart = GetValue(inputData, valuePosition);
                    AppendValues(ref output, ref outputPos, valuePart);
                    valuePosition++;
                }
            }
            return output;
        }
        
        internal static RefraktionDataOut Map(byte[] inputData)
        {
            return new RefraktionDataOut();
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

        private static byte[] GetValue(RefraktionDataIn inputData, int index)
        {
            string outputString = null;
            var data = GetValueWithIndex(inputData, index);
            var forcePluseSign = GetIndexSettingWithIndex(inputData, index);
            outputString = GetValueAsString(data, forcePluseSign);
            return outputString == null ? null : Encoding.ASCII.GetBytes(outputString);
        }

        private static bool GetIndexSettingWithIndex(RefraktionDataIn instance, int index)
        {
            bool retVal = false;
            var properties = instance.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var reihenfolgeAttribute = propertyInfo.GetCustomAttributes(typeof(IndexAttribute), true);
                foreach (var attribute in reihenfolgeAttribute)
                {
                    var indexAttribute = attribute as IndexAttribute;
                    if (indexAttribute != null)
                    {
                        if (indexAttribute.Index == index)
                        {
                            retVal = indexAttribute.ForcePlusSign;
                        }
                    }
                }
            }
            return retVal;
        }

        private static object GetValueWithIndex(object instance, int i)
        {
            object retVal = null;
            var properties = instance.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var reihenfolgeAttribute = propertyInfo.GetCustomAttributes(typeof(IndexAttribute), true);
                foreach (var attribute in reihenfolgeAttribute)
                {
                    var reihenfolge = attribute as IndexAttribute;
                    if (reihenfolge != null)
                    {
                        if (reihenfolge.Index == i)
                        {
                            if (retVal != null)
                            {
                                retVal = GetDoubleAlsString((double)retVal) + " " + propertyInfo.GetValue(instance);
                            }
                            else
                            {
                                retVal = propertyInfo.GetValue(instance);
                            }
                        }
                    }
                }
            }
            return retVal;
        }

        private static string GetValueAsString(object value, bool focePlusSign = false)
        {
            string retVal = null;
            var type = value.GetType();
            var typeString = type.ToString();
            switch (typeString)
            {
                case "System.Double":
                    retVal = GetDoubleAlsString((double)value, focePlusSign);
                    break;
                case "System.Int32":
                    retVal = GetIntAlsString((int)value);
                    break;
                case "System.String":
                    retVal = ((string)value);
                    break;
            }

            return retVal;
        }

        private static string GetDoubleAlsString(double inputValue, bool focePlusSign = false)
        {
            var stringRepresentation = inputValue.ToString("0.00").PadLeft(7);

            if (focePlusSign && inputValue > 0)
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
