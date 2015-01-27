using System;
using System.IO.Ports;
using System.Text;

using Visutron900PlusInterface.Contracts;
using Visutron900PlusInterface.Messages;
using Visutron900PlusInterface.Messages.DTOs;

namespace Visutron900PlusInterface.Model
{
    public class ModelLayer : IModelLayer
    {
        private CanStates _canStates = new CanStates();
        private SerialPort _serialPort = null;
        private MessageMapper _messageMapper = new MessageMapper(Encoding.ASCII);

        public void CreateConnection(SerialConnectionSettings serialConnectionSettings)
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
                _serialPort.Dispose();
            }
            _serialPort = new SerialPort(
                serialConnectionSettings.PortName,
                serialConnectionSettings.BaudRate,
                serialConnectionSettings.Parity,
                serialConnectionSettings.DataBits,
                serialConnectionSettings.StopBits);

            _serialPort.DataReceived += (sender, args) =>
            {
                var port = sender as SerialPort;
                if (port != null)
                {
                    var buffer = new byte[port.BytesToRead];
                    port.Read(buffer, 0, buffer.Length);
                    OnRefraktionDataReceived(_messageMapper.Map(buffer));
                }
            };

            _canStates.CanCreateConnection = false;
            _canStates.CanOpenConnection = true;
            _canStates.CanChangeConnectionSettings = false;
            OnCanChanged(_canStates);
        }

        public void CloseConnection()
        {
            _serialPort.Close();

            _canStates.CanChangeConnectionSettings = true;
            _canStates.CanCreateConnection = true;
            _canStates.CanCloseConnection = false;
            _canStates.CanSend = false;
            OnCanChanged(_canStates);
        }

        public void OpenConnection()
        {
            _serialPort.Open();

            _canStates.CanCloseConnection = true;
            _canStates.CanOpenConnection = false;
            _canStates.CanSend = true;
            OnCanChanged(_canStates);
        }

        public void SendRefraktionData(RefraktionData refraktionData)
        {
            var bytes = _messageMapper.Map(refraktionData);
            _serialPort.Write(bytes, 0, bytes.Length);
        }

        public event Action<CanStates> OnCanChanged;
        public event Action<RefraktionData> OnRefraktionDataReceived;
    }
}