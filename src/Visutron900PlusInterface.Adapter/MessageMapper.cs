using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    internal class MessageMapper
    {
        internal static byte[] Map(RefraktionData inputData)
        {
            var template = ValueConverter.GetInputPattern();
            var returnBuffer = new List<byte>();

            var valuePosition = 0;

            foreach (var templateByte in template)
            {
                returnBuffer.Add(templateByte);

                if (templateByte == ':')
                {
                    var valuePart = GetValue(inputData, valuePosition);
                    returnBuffer.AddRange(valuePart);
                    valuePosition++;
                }
            }

            return returnBuffer.ToArray();
        }

        internal static RefraktionData Map(byte[] inputData)
        {
            var isInValuePart = false;
            var valueCounter = 0;
            var valueContent = new List<byte>();
            var contentList = new List<string>();
            foreach (var inputByte in inputData)
            {
                if (inputByte == 0x0D)//0x0D => Carriage Return
                {
                    if (isInValuePart)
                    {
                        valueCounter++;
                        contentList.Add(Encoding.ASCII.GetString(valueContent.ToArray()));
                    }
                    isInValuePart = false;
                }
                if (isInValuePart)
                {
                    valueContent.Add(inputByte);
                }
                if (inputByte == ':')
                {
                    if (!isInValuePart)
                    {
                        valueContent = new List<byte>();
                    }
                    isInValuePart = true;
                }
            }

            return GenerateRefraktionData(contentList);
        }

        private static RefraktionData GenerateRefraktionData(List<string> valueList)
        {
            var refraktionData = new RefraktionData();
            var properties = refraktionData.GetType().GetProperties();

            for (int i = 0; i < valueList.Count; i++)
            {
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.SetMethod == null)
                    {
                        continue;
                    }
                    var attributes = propertyInfo.GetCustomAttributes();
                    foreach (var attribute in attributes)
                    {
                        var indexer = attribute as IndexAttribute;
                        if (indexer != null && indexer.Index == i)
                        {
                            var targetType = propertyInfo.PropertyType;
                            var targetTypeString = targetType.ToString();
                            object targetValue = null;
                            var sourceStringValue = valueList[i];
                            try
                            {
                                switch (targetTypeString)
                                {
                                    case "System.String":
                                        targetValue = sourceStringValue;
                                        break;
                                    case "System.Double":
                                        targetValue = ValueConverter.ConvertStringToDouble(sourceStringValue);
                                        break;
                                    case "System.Int32":
                                        targetValue = int.Parse(sourceStringValue);
                                        break;
                                    case "Visutron900PlusInterface.Adapter.DTOs.PrismaHorizontal":
                                        targetValue = ValueConverter.ConvertStringToPrismaHorizontal(sourceStringValue);
                                        break;
                                    case "Visutron900PlusInterface.Adapter.DTOs.PrismaVertikal":
                                        targetValue = ValueConverter.ConvertStringToPrismaVertikal(sourceStringValue);
                                        break;
                                    case "System.DateTime":
                                        var currentValue = (DateTime)propertyInfo.GetValue(refraktionData);
                                        targetValue = ValueConverter.ConvertStringsToDataTime(sourceStringValue, currentValue);
                                        break;
                                }

                                propertyInfo.SetValue(refraktionData, targetValue);
                            }
                            catch (Exception exception)
                            {
                                Trace.WriteLine("Exception in [GenerateRefraktionData] bei " + propertyInfo.Name);
                                Trace.WriteLine(exception);
                            }
                        }
                    }
                }
            }

            return refraktionData;
        }

        private static byte[] GetValue(RefraktionData inputData, int index)
        {
            string outputString = null;
            var data = GetValueWithIndex(inputData, index);
            var forcePluseSign = GetIndexSettingWithIndex(inputData, index);
            outputString = ValueConverter.GetValueAsString(data, forcePluseSign);
            return outputString == null ? null : Encoding.ASCII.GetBytes(outputString);
        }

        private static bool GetIndexSettingWithIndex(RefraktionData instance, int index)
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
                                retVal = ValueConverter.GetDoubleAlsString((double)retVal) + " " + propertyInfo.GetValue(instance);
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
    }

    internal class ValueConverter
    {
        private static List<string> PrismaValues { get; set; }

        static ValueConverter()
        {
            PrismaValues = new List<string>();
            PrismaValues.AddRange(Enum.GetNames(typeof(PrismaHorizontal)));
            PrismaValues.AddRange(Enum.GetNames(typeof(PrismaVertikal)));
        }

        internal static double ConvertStringToDouble(string inputString)
        {
            foreach (var prismaValue in PrismaValues)
            {
                if (inputString.Contains(prismaValue))
                {
                    inputString = inputString.Replace(prismaValue, string.Empty);
                }
            }

            inputString = inputString.Replace(" ", string.Empty);
            inputString = inputString.Replace('.', ',');
            return double.Parse(inputString);
        }

        internal static string GetValueAsString(object value, bool focePlusSign = false)
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

        internal static string GetDoubleAlsString(double inputValue, bool focePlusSign = false)
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

        internal static byte[] GetInputPattern()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream("Visutron900PlusInterface.Adapter.MessagePattern.TelegrammVisutron900_In_Pattern");

            var resouceBytes = new byte[stream.Length];

            stream.Read(resouceBytes, 0, resouceBytes.Length);

            stream.Close();
            stream.Dispose();

            return resouceBytes;
        }

        private static string GetIntAlsString(int inputValue)
        {
            var stringRepresentation = inputValue.ToString(CultureInfo.InvariantCulture).PadLeft(7);
            return stringRepresentation;
        }

        public static PrismaHorizontal ConvertStringToPrismaHorizontal(string sourceStringValue)
        {
            var wert = sourceStringValue.Split(' ').LastOrDefault();
            return (PrismaHorizontal)Enum.Parse(typeof(PrismaHorizontal), wert);
        }

        public static PrismaVertikal ConvertStringToPrismaVertikal(string sourceStringValue)
        {
            var wert = sourceStringValue.Split(' ').LastOrDefault();
            return (PrismaVertikal)Enum.Parse(typeof(PrismaVertikal), wert);
        }

        public static DateTime ConvertStringsToDataTime(string sourceStringValue, DateTime currentValue)
        {
            var hour = currentValue.Hour;
            var minute = currentValue.Minute;
            var day = currentValue.Day;
            var month = currentValue.Month;
            var year = currentValue.Year;

            sourceStringValue = sourceStringValue.Trim();

            if (sourceStringValue.Contains(':'))
            {
                var split = sourceStringValue.Split(':');
                hour = int.Parse(split[0]);
                minute = int.Parse(split[1]);

            }
            else
            {
                var split = sourceStringValue.Split('.');
                day = int.Parse(split[0]);
                month = int.Parse(split[1]);
                year = int.Parse(split[2]);

            }
            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}
