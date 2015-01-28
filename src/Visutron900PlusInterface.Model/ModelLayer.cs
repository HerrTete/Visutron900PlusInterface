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

        private SerialPortAdapter _serialPortAdapter = null;

        public ModelLayer()
        {
            _serialPortAdapter = new SerialPortAdapter();
        }

        public void CreateConnection(SerialConnectionSettings serialConnectionSettings)
        {
            _serialPortAdapter.Create(serialConnectionSettings);
            _serialPortAdapter.RefraktionDataReceived += OnRefraktionDataReceived;
            _canStates.CanCreateConnection = false;
            _canStates.CanOpenConnection = true;
            _canStates.CanChangeConnectionSettings = false;
            OnCanChanged(_canStates);
        }

        public void CloseConnection()
        {
            _serialPortAdapter.Close();

            _canStates.CanChangeConnectionSettings = true;
            _canStates.CanCreateConnection = true;
            _canStates.CanCloseConnection = false;
            _canStates.CanSend = false;
            OnCanChanged(_canStates);
        }

        public void OpenConnection()
        {
            _serialPortAdapter.Open();

            _canStates.CanCloseConnection = true;
            _canStates.CanOpenConnection = false;
            _canStates.CanSend = true;
            OnCanChanged(_canStates);
        }

        public void SendRefraktionData(RefraktionData refraktionData)
        {
            _serialPortAdapter.SendRefraktionData(refraktionData);
        }

        public event Action<CanStates> OnCanChanged;
        public event Action<RefraktionData> OnRefraktionDataReceived;
    }
}