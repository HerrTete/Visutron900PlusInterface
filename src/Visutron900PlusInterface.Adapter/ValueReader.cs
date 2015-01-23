using System.Collections.Generic;
using System.Linq;
using System.Text;

using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    internal class ValueReader
    {
        private static Encoding encoding;

        internal ValueReader(Encoding encoding)
        {
            ValueReader.encoding = encoding;
        }

        internal List<byte[]> ReadValues(RefraktionData inputData)
        {
            var propertyCount = inputData.GetType().GetProperties().Length - 1;
            var values = new List<byte[]>();
            for (int i = 0; i < propertyCount; i++)
            {
                values.Add(ReadValues(inputData, i));
            }
            return values;
        }

        internal List<string> ReadValues(byte[] inputData)
        {
            var isInValuePart = false;
            var valueContent = new List<byte>();
            var contentList = new List<string>();
            foreach (var inputByte in inputData)
            {
                if (inputByte == 0x0D)//0x0D => Carriage Return
                {
                    if (isInValuePart)
                    {
                        contentList.Add(encoding.GetString(valueContent.ToArray()));
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
            return contentList;
        }

        private byte[] ReadValues(RefraktionData inputData, int index)
        {
            var data = GetValueWithIndex(inputData, index);
            var forcePluseSign = GetForcePlusSignForIndex(inputData, index);
            var outputString = ValueConverter.ConvertToString(data, forcePluseSign);
            return outputString == null ? null : encoding.GetBytes(outputString);
        }

        private bool GetForcePlusSignForIndex(RefraktionData instance, int index)
        {
            var forcePlusSign = false;
            var properties = instance.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var reihenfolgeAttribute = propertyInfo.GetCustomAttributes(typeof(IndexAttribute), true);
                foreach (var indexAttribute in reihenfolgeAttribute.OfType<IndexAttribute>().Where(indexAttribute => indexAttribute.Index == index))
                {
                    forcePlusSign = indexAttribute.ForcePlusSign;
                }
            }
            return forcePlusSign;
        }

        private object GetValueWithIndex(object instance, int i)
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
                            var isComposed = retVal != null;
                            if (isComposed)
                            {
                                retVal = ValueConverter.ConvertToString((double)retVal) + " " + propertyInfo.GetValue(instance);
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
}