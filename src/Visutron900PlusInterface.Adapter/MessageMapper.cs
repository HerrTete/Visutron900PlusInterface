using System.Reflection;

using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    internal class MessageMapper
    {
        internal static byte[] Map(RefraktionDataIn inputData)
        {
            return new byte[0];
        }

        internal static RefraktionDataOut Map(byte[] inputData)
        {
            return new RefraktionDataOut();
        }
    }
}
