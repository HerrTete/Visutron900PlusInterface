using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter.Test
{
    [TestClass]
    public class MessageMapperTest
    {
        [TestMethod]
        public void MapToInMessageTest()
        {
            var inputData = new RefraktionDataIn();

            var bytes = MessageMapper.Map(inputData);

            var refBytes = GetRefMessage("Visutron900PlusInterface.Adapter.MessagePattern.TelegrammVisutron900_In_Ref");
        }
        
        [TestMethod]
        public void MapFromOutMessageTest()
        {
            var refBytes = GetRefMessage("Visutron900PlusInterface.Adapter.MessagePattern.TelegrammVisutron900_Out_Ref");
            var mapperResult = MessageMapper.Map(refBytes);
        }

        private byte[] GetRefMessage(string resourceName)
        {
            var assemblyPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            var targetAssemblyPath = Path.Combine(
                assemblyPath,
                "Visutron900PlusInterface.Adapter.dll");

            var assembly = Assembly.LoadFile(targetAssemblyPath);

            var stream = assembly.GetManifestResourceStream(resourceName);

            var resouceBytes = new byte[stream.Length];

            stream.Read(resouceBytes, 0, resouceBytes.Length);

            stream.Close();
            stream.Dispose();

            return resouceBytes;
        }
    }
}
