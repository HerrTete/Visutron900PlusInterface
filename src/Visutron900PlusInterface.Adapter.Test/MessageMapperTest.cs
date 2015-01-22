using System;
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
            inputData.PrismaLinks = 3.5;
            inputData.PupillendistanzLinks = 32.5;

            inputData.Pupillendistanz = 64;

            inputData.Patientenname = "Hans Mustermann";
            inputData.PatientenID = "0123456789";

            var bytes = MessageMapper.Map(inputData);

            var refBytes = GetRefMessage("Visutron900PlusInterface.Adapter.Test.ReferenceMessages.TelegrammVisutron900_In_Ref");


            for (int i = 0; i < refBytes.Length; i++)
            {
                var actualByte = bytes[i];
                var expectedByte = refBytes[i];

                Assert.AreEqual(expectedByte.ToString("X"), actualByte.ToString("X"), "An Stelle {0}/{1} gab es eine Abweichung.", i.ToString("X"), i);
            }
        }

        [TestMethod]
        public void MapFromOutMessageTest()
        {

            var outputDataRef = new RefraktionDataOut();

            outputDataRef.SphäreFernRechts = 3.75;
            outputDataRef.SphäreNahRechts = 4.5;
            outputDataRef.ZylinderRechts = -3.5;
            outputDataRef.AchseRechts = 45;
            outputDataRef.PrismaRechts = 5.50;
            outputDataRef.PupillendistanzRechts = 31.5;

            outputDataRef.AkkommodationsbreiteRechts = 0.25;
            outputDataRef.Visus_S_C_Rechts = 0.50;
            outputDataRef.Visus_C_C_Rechts = 0.80;

            outputDataRef.SphäreFernLinks = -1.5;
            outputDataRef.SphäreNahLinks = -0.5;
            outputDataRef.ZylinderLinks = -2.5;
            outputDataRef.AchseLinks = 145;
            outputDataRef.PrismaLinks = 3.5;
            outputDataRef.PupillendistanzLinks = 32.5;

            outputDataRef.AkkommodationsbreiteRechts = 0.25;
            outputDataRef.Visus_S_C_Rechts = 0.40;
            outputDataRef.Visus_C_C_Rechts = 0.80;

            outputDataRef.Pupillendistanz = 64;

            outputDataRef.HornhautScheitelAbstand = 16;
            outputDataRef.Fusionsbreite = 0.60;
            outputDataRef.Visus_S_C= 0.60;
            outputDataRef.Visus_C_C= 0.80;

            outputDataRef.Patientenname = "Hans Mustermann";
            outputDataRef.PatientenID = "0123456789*abc";

            outputDataRef.RefraktionsZeitpunkt = new DateTime(2003, 12, 31, 15, 28, 0);

            var refBytes = GetRefMessage("Visutron900PlusInterface.Adapter.Test.ReferenceMessages.TelegrammVisutron900_Out_Ref");

            var output = MessageMapper.Map(refBytes);

            Assert.AreEqual(outputDataRef.AchseLinks, output.AchseLinks);
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
