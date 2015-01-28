using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using Visutron900PlusInterface.Contracts;
using Visutron900PlusInterface.Messages;
using Visutron900PlusInterface.Messages.DTOs;
using Visutron900PlusInterface.Model;

namespace Visutron900PlusMock
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            var settings = new SerialConnectionSettings
            { 
                PortName = "COM1",
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One
            };
            var serialPort = new SerialPortAdapter();
            serialPort.Create(settings);
            serialPort.Open();
            Console.WriteLine("Port: " + settings.PortName + "opened.");
            serialPort.RefraktionDataReceived += data =>
            {

                var patName = data.Patientenname;
                var patId = data.PatientenID;

                data.PatientenID = patName;
                data.Patientenname = patId;

                serialPort.SendRefraktionData(data);

            };
            Console.ReadLine();
            serialPort.Close();
            serialPort.Dispose();
        }
    }
}
