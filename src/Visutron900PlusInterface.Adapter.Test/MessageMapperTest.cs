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

            inputData.SphäreFernRechts = 3.75;
            inputData.SphäreNahRechts = 4.5;
            inputData.ZylinderRechts = -3.5;
            inputData.AchseRechts = 45;
            inputData.PrismaRechts = 5.50;
            inputData.PupillendistanzRechts = 31.5;

            inputData.SphäreFernLinks = -1.5;
            inputData.SphäreNahLinks = -0.5;
            inputData.ZylinderLinks = -2.5;
            inputData.AchseLinks = 145;
            inputData.PrismaLinks= 3.5;
            inputData.PupillendistanzLinks= 32.5;

            inputData.Pupillendistanz = 64;

            inputData.Patientenname = "Hans.Mustermann";
            inputData.PatientenID = "0123456789";

            var bytes = MessageMapper.Map(inputData);

            var refBytes = GetRefMessage("Visutron900PlusInterface.Adapter.Test.ReferenceMessages.TelegrammVisutron900_In_Ref");


            for (int i = 0; i < refBytes.Length; i++)
            {
                var actualByte = bytes[i];
                var expectedByte = refBytes[i];

                Assert.AreEqual(expectedByte.ToString("X"), actualByte.ToString("X"), "An Stelle {0} gab es eine Abweichung.", i.ToString("X"));
            }
        }
        
        [TestMethod]
        public void MapFromOutMessageTest()
        {
            var refBytes = GetRefMessage("Visutron900PlusInterface.Adapter.Test.ReferenceMessages.TelegrammVisutron900_Out_Ref");
            var mapperResult = MessageMapper.Map(refBytes);
        }

        private byte[] GetRefMessage(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream(resourceName);

            var resouceBytes = new byte[stream.Length];

            stream.Read(resouceBytes, 0, resouceBytes.Length);

            stream.Close();
            stream.Dispose();

            return resouceBytes;
        }
    }
}
