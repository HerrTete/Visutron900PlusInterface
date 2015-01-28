using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using Visutron900PlusInterface.Messages;
using Visutron900PlusInterface.Messages.DTOs;

namespace Visutron900PlusMock
{
    class Program
    {
        static void Main(string[] args)
        {
            var serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            serialPort.Open();
            Console.WriteLine("Port: " + serialPort.PortName + "opened.");
            serialPort.DataReceived += (sender, eventArgs) =>
            {
                ReadBytes(sender as SerialPort, (bytes) =>
                {
                    var messageMapper = new MessageMapper(Encoding.ASCII);
                    
                    Console.WriteLine(bytes.Length + " bytes received.");
                    
                    var inputeMessage = messageMapper.Map(bytes);
                    PrintMessage(inputeMessage);

                    var patName = inputeMessage.Patientenname;
                    var patId = inputeMessage.PatientenID;

                    inputeMessage.PatientenID = patName;
                    inputeMessage.Patientenname = patId;

                    SendRefraktionData(inputeMessage, sender as SerialPort);
                });
            };
            Console.ReadLine();
            serialPort.Close();
            serialPort.Dispose();
        }

        static void ReadBytes(SerialPort port, Action<byte[]> workWithBytes)
        {
            if (port != null)
            {
                var buffer = new List<byte>();

                byte lastByte = 0x00;
                while (lastByte != 0x03)
                {
                    lastByte = (byte)port.ReadByte();
                    buffer.Add(lastByte);
                }
                workWithBytes(buffer.ToArray());
            }
        }

        static void SendRefraktionData(RefraktionData data, SerialPort port)
        {
            var messageMapper = new MessageMapper(Encoding.ASCII);
            var sendBytes = messageMapper.Map(data);
            port.Write(sendBytes, 0, sendBytes.Length);

        }


        static void PrintMessage(RefraktionData data)
        {
            var width = 20;
            var prevForColor = Console.BackgroundColor;
            var prevBackColor = Console.ForegroundColor;

            Console.ForegroundColor = prevBackColor;
            Console.BackgroundColor = prevForColor;

            Console.WriteLine("RefraktionData:");
            Console.WriteLine("+++++++++++++++");
            Console.WriteLine(string.Empty);
            Console.Write("SphäreFernRechts:".PadLeft(width));
            Console.WriteLine(data.SphäreFernRechts);
            Console.Write("SphäreNahRechts:".PadLeft(width));
            Console.WriteLine(data.SphäreNahRechts);
            Console.Write("ZylinderRechts:".PadLeft(width));
            Console.WriteLine(data.ZylinderRechts);
            Console.Write("AchseRechts:".PadLeft(width));
            Console.WriteLine(data.AchseRechts);
            Console.Write("PrismaRechts:".PadLeft(width));
            Console.WriteLine(data.PrismaRechts);
            Console.Write("GesamtprismaHorizontal:".PadLeft(width));
            Console.WriteLine(data.GesamtprismaHorizontal);
            Console.Write("PupillendistanzRechts:".PadLeft(width));
            Console.WriteLine(data.PupillendistanzRechts);

            Console.Write("Visus_S_C_Rechts:".PadLeft(width));
            Console.WriteLine(data.Visus_S_C_Rechts);
            Console.Write("Visus_C_C_Rechts:".PadLeft(width));
            Console.WriteLine(data.Visus_C_C_Rechts);
            Console.Write("AkkommodationsbreiteRechts:".PadLeft(width));
            Console.WriteLine(data.AkkommodationsbreiteRechts);

            Console.Write("SphäreFernLinks:".PadLeft(width));
            Console.WriteLine(data.SphäreFernLinks);
            Console.Write("SphäreFernRechts:".PadLeft(width));
            Console.WriteLine(data.SphäreNahLinks);
            Console.Write("ZylinderLinks:".PadLeft(width));
            Console.WriteLine(data.ZylinderLinks);
            Console.Write("AchseLinks:".PadLeft(width));
            Console.WriteLine(data.AchseLinks);
            Console.Write("PrismaLinks:".PadLeft(width));
            Console.WriteLine(data.PrismaLinks);
            Console.Write("GesamtprismaVertikal:".PadLeft(width));
            Console.WriteLine(data.GesamtprismaVertikal);
            Console.Write("PupillendistanzLinks:".PadLeft(width));
            Console.WriteLine(data.PupillendistanzLinks);

            Console.Write("Visus_S_C_Links:".PadLeft(width));
            Console.WriteLine(data.Visus_S_C_Links);
            Console.Write("Visus_C_C_Links:".PadLeft(width));
            Console.WriteLine(data.Visus_C_C_Links);
            Console.Write("AkkommodationsbreiteLinks:".PadLeft(width));
            Console.WriteLine(data.AkkommodationsbreiteLinks);

            Console.Write("HornhautScheitelAbstand:".PadLeft(width));
            Console.WriteLine(data.HornhautScheitelAbstand);
            Console.Write("Pupillendistanz:".PadLeft(width));
            Console.WriteLine(data.Pupillendistanz);
            Console.Write("Fusionsbreite:".PadLeft(width));
            Console.WriteLine(data.Fusionsbreite);
            Console.Write("Visus_S_C:".PadLeft(width));
            Console.WriteLine(data.Visus_S_C);
            Console.Write("Visus_C_C:".PadLeft(width));
            Console.WriteLine(data.Visus_C_C);
            Console.Write("Patientenname:".PadLeft(width));
            Console.WriteLine(data.Patientenname);
            Console.Write("PatientenID:".PadLeft(width));
            Console.WriteLine(data.PatientenID);
            Console.Write("RefraktionsZeitpunkt:".PadLeft(width));
            Console.WriteLine(data.RefraktionsZeitpunkt);
        }
    }
}
