using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Visutron900PlusInterface.Contracts;
using Visutron900PlusInterface.Messages;
using Visutron900PlusInterface.Messages.DTOs;

namespace Visutron900PlusInterface.Model
{
    public class SerialPortAdapter : IDisposable
    {
        SerialPort _serialPort = null;
        MessageMapper _messageMapper = null;
        
        public void Create(SerialConnectionSettings serialConnectionSettings)
        {
            if(_messageMapper == null)
            {
                _messageMapper = new MessageMapper(Encoding.ASCII);
            }

            DisposeSerialPort();

            _serialPort = new SerialPort(
                serialConnectionSettings.PortName,
                serialConnectionSettings.BaudRate,
                serialConnectionSettings.Parity,
                serialConnectionSettings.DataBits,
                serialConnectionSettings.StopBits);

            _serialPort.DataReceived += ReadData;
        }

        public void Open()
        {
            if(_serialPort != null && !_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void SendRefraktionData(RefraktionData refraktionData)
        {
            Trace.WriteLine("Sending Message:");
            PrintMessage(refraktionData);            
            var bytes = _messageMapper.Map(refraktionData);
            _serialPort.Write(bytes, 0, bytes.Length);
        }

        public event Action<RefraktionData> RefraktionDataReceived;

        private void ReadData(object sender, SerialDataReceivedEventArgs e)
        {
            var port = sender as SerialPort;
            if (port != null)
            {
                var buffer = new List<byte>();

                byte lastByte = 0x00;
                while (lastByte != 0x03)
                {
                    lastByte = (byte)port.ReadByte();
                    buffer.Add(lastByte);
                }
                BytesReceived(buffer.ToArray());
            }
        }

        private void BytesReceived(byte[] bytes)
        {
            var message = _messageMapper.Map(bytes);
            Trace.WriteLine("MessageReceived:");
            PrintMessage(message);
            RefraktionDataReceived(message);
        }

        public void Dispose()
        {
            DisposeSerialPort();
        }

        private void DisposeSerialPort()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }

                _serialPort.DataReceived -= ReadData;
                _serialPort.Dispose();
                _serialPort = null;
            }
        }


        private void PrintMessage(RefraktionData data)
        {
            var width = 30;
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
