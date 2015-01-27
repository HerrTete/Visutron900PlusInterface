using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Visutron900PlusInterface.Messages.DTOs;

namespace Visutron900PlusInterface.Messages
{
    internal class ValueConverter
    {
        private static List<string> PrismaValues { get; set; }

        static ValueConverter()
        {
            PrismaValues = new List<string>();
            PrismaValues.AddRange(Enum.GetNames(typeof(PrismaHorizontal)));
            PrismaValues.AddRange(Enum.GetNames(typeof(PrismaVertikal)));
        }

        internal static string ConvertToString(object value, bool focePlusSign = false)
        {
            string retVal = null;
            var type = value.GetType();
            var typeString = type.ToString();
            switch (typeString)
            {
                case "System.Double":
                    retVal = ConvertDoubleToString((double)value, focePlusSign);
                    break;
                case "System.Int32":
                    retVal = ConvertIntToString((int)value);
                    break;
                case "System.String":
                    retVal = ((string)value);
                    break;
            }

            return retVal;
        }

        internal static object ConvertFromString(string targetTypeString, string sourceStringValue, object currentValue)
        {
            object targetValue = null;
            switch (targetTypeString)
            {
                case "System.String":
                    targetValue = sourceStringValue;
                    break;
                case "System.Double":
                    targetValue = ConvertStringToDouble(sourceStringValue);
                    break;
                case "System.Int32":
                    targetValue = int.Parse(sourceStringValue);
                    break;
                case "Visutron900PlusInterface.Messages.DTOs.PrismaHorizontal":
                    targetValue = ConvertStringToPrismaHorizontal(sourceStringValue);
                    break;
                case "Visutron900PlusInterface.Messages.DTOs.PrismaVertikal":
                    targetValue = ConvertStringToPrismaVertikal(sourceStringValue);
                    break;
                case "System.DateTime":
                    targetValue = ConvertStringsToDataTime(
                        sourceStringValue,
                        (DateTime)currentValue);
                    break;
            }
            return targetValue;
        }

        private static string ConvertDoubleToString(double inputValue, bool focePlusSign = false)
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

        private static string ConvertIntToString(int inputValue)
        {
            var stringRepresentation = inputValue.ToString(CultureInfo.InvariantCulture).PadLeft(7);
            return stringRepresentation;
        }

        private static PrismaHorizontal ConvertStringToPrismaHorizontal(string sourceStringValue)
        {
            var wert = sourceStringValue.Split(' ').LastOrDefault();
            return (PrismaHorizontal)Enum.Parse(typeof(PrismaHorizontal), wert);
        }

        private static PrismaVertikal ConvertStringToPrismaVertikal(string sourceStringValue)
        {
            var wert = sourceStringValue.Split(' ').LastOrDefault();
            return (PrismaVertikal)Enum.Parse(typeof(PrismaVertikal), wert);
        }

        private static DateTime ConvertStringsToDataTime(string sourceStringValue, DateTime currentValue)
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

        private static double ConvertStringToDouble(string inputString)
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
    }
}