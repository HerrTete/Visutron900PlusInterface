using System;
using System.IO.Ports;
using System.Text;

using Visutron900PlusInterface.Adapter;

namespace Visutron900PlusMock
{
    class Program
    {
        static void Main(string[] args)
        {
            var serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            serialPort.Open();
            serialPort.DataReceived += (sender, eventArgs) =>
            {
                var port = sender as SerialPort;
                if (port != null)
                {
                    var buffer = new byte[port.BytesToRead];
                    port.Read(buffer, 0, buffer.Length);
                    var messageMapper = new MessageMapper(Encoding.ASCII);
                    var inputeMessage = messageMapper.Map(buffer);

                    var patName = inputeMessage.Patientenname;
                    var patId = inputeMessage.PatientenID;

                    inputeMessage.PatientenID= patName;
                    inputeMessage.Patientenname = patId;

                    var sendBytes = messageMapper.Map(inputeMessage);
                    serialPort.Write(sendBytes, 0, sendBytes.Length);
                }
            };
            Console.ReadLine();
            serialPort.Close();
            serialPort.Dispose();
        }
    }
}
