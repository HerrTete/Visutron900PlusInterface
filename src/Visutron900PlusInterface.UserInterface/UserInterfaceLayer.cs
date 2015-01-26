using System;
using System.Collections.Generic;
using System.IO.Ports;

using Visutron900PlusInterface.Adapter.DTOs;
using Visutron900PlusInterface.Contracts;
using Visutron900PlusInterface.UserInterface.ViewModel;
using Visutron900PlusInterface.UserInterface.WpfTools;

namespace Visutron900PlusInterface.UserInterface
{
    public class UserInterfaceLayer : IUserInterfaceLayer
    {
        public event Action<SerialConnectionSettings> OnCreateConnection;
        public event Action OnOpenConnection;
        public event Action OnCloseConnection;
        public event Action<RefraktionData> OnSendRefraktionData;

        private readonly MainWindow _mainWindow = null;

        private readonly RefraktionInputDataControlViewModel _refraktionInputDataControlViewModel = null;
        private readonly RefraktionResultDataControlViewModel _refraktionResultDataControlViewModel = null;
        private SerialPortConnectionControlViewModel _serialPortConnectionControlViewModel = null;
        private readonly SerialPortSettingsControlViewModel _serialPortSettingsControlViewModel = null;

        public UserInterfaceLayer()
        {
            _mainWindow = new MainWindow();

            _refraktionInputDataControlViewModel = new RefraktionInputDataControlViewModel();
            _refraktionResultDataControlViewModel = new RefraktionResultDataControlViewModel();
            _serialPortConnectionControlViewModel = new SerialPortConnectionControlViewModel();
            _serialPortSettingsControlViewModel = new SerialPortSettingsControlViewModel();

            _serialPortConnectionControlViewModel.CreateCommand = new BaseCommand(CreateConnection);
            _serialPortConnectionControlViewModel.OpenCommand = new BaseCommand(OnOpenConnection);
            _serialPortConnectionControlViewModel.CloseCommand = new BaseCommand(OnCloseConnection);
            _refraktionInputDataControlViewModel.SendCommand = new BaseCommand(SendData);

            _mainWindow.RefraktionResultDataControl.DataContext = _refraktionResultDataControlViewModel;
            _mainWindow.RefraktionInputDataControl.DataContext = _refraktionInputDataControlViewModel;
            _mainWindow.SerialPortConnectionControl.DataContext = _serialPortConnectionControlViewModel;
            _mainWindow.SerialPortSettingsControl.DataContext = _serialPortSettingsControlViewModel;

            SetValueRanges();
            SetDefaultValues();
        }

        private void CreateConnection()
        {
            var settings = GetSelectedValues();
            OnCreateConnection(settings);
        }

        private void SetDefaultValues()
        {
            _serialPortSettingsControlViewModel.Port = _serialPortSettingsControlViewModel.Ports[0];
            _serialPortSettingsControlViewModel.BaudRate = _serialPortSettingsControlViewModel.BaudRates[0];
            _serialPortSettingsControlViewModel.Databit= _serialPortSettingsControlViewModel.Databits[0];
            _serialPortSettingsControlViewModel.Parity= _serialPortSettingsControlViewModel.Parities[0];
            _serialPortSettingsControlViewModel.Stopbit= _serialPortSettingsControlViewModel.Stopbits[1];
        }

        private void SendData()
        {
            var data = GetInputData();
            OnSendRefraktionData(data);
        }

        private RefraktionData GetInputData()
        {
            return new RefraktionData
            {
                SphäreFernRechts = _refraktionInputDataControlViewModel.SphäreFernRechts
            };
        }

        private SerialConnectionSettings GetSelectedValues()
        {
            return new SerialConnectionSettings
            {
                PortName = _serialPortSettingsControlViewModel.Port,
                BaudRate = _serialPortSettingsControlViewModel.BaudRate,
                DataBits = _serialPortSettingsControlViewModel.Databit,
                Parity = (Parity)Enum.Parse(typeof(Parity), _serialPortSettingsControlViewModel.Parity),
                StopBits = (StopBits)Enum.Parse(typeof(StopBits), _serialPortSettingsControlViewModel.Stopbit)
            };
        }

        private void SetValueRanges()
        {
            var ports = SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                ports = new[] { "NoPortFound" };
            }
            _serialPortSettingsControlViewModel.Ports = new List<string>(ports);
            _serialPortSettingsControlViewModel.BaudRates = new List<int> { 9600 };
            _serialPortSettingsControlViewModel.Databits = new List<int> { 8,7,6 };
            _serialPortSettingsControlViewModel.Parities = new List<string>(Enum.GetNames(typeof(Parity)));
            _serialPortSettingsControlViewModel.Stopbits = new List<string> (Enum.GetNames(typeof(StopBits)));
        }

        public void CanChanged(CanStates canStates)
        {
            
        }

        public void DisplayRefraktionData(RefraktionData refraktionData)
        {
            _refraktionResultDataControlViewModel.SphäreFernRechts = refraktionData.SphäreFernRechts;

            _refraktionResultDataControlViewModel.OnPropertyChanged("SphäreFernRechts");
        }

        public void Show()
        {
            _mainWindow.ShowDialog();
        }
    }
}