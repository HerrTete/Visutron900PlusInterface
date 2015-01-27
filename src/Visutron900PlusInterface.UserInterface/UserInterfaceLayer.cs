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
        private readonly SerialPortConnectionControlViewModel _serialPortConnectionControlViewModel = null;
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
                SphäreFernRechts = _refraktionInputDataControlViewModel.SphäreFernRechts,
                SphäreNahRechts = _refraktionInputDataControlViewModel.SphäreNahRechts,
                ZylinderRechts = _refraktionInputDataControlViewModel.ZylinderRechts,
                AchseRechts = _refraktionInputDataControlViewModel.AchseRechts,
                PrismaRechts = _refraktionInputDataControlViewModel.PrismaRechts,
                GesamtprismaHorizontal = _refraktionInputDataControlViewModel.GesamtprismaHorizontal,
                PupillendistanzRechts = _refraktionInputDataControlViewModel.PupillendistanzRechts,

                SphäreFernLinks = _refraktionInputDataControlViewModel.SphäreFernLinks,
                SphäreNahLinks = _refraktionInputDataControlViewModel.SphäreNahLinks,
                ZylinderLinks = _refraktionInputDataControlViewModel.ZylinderLinks,
                AchseLinks = _refraktionInputDataControlViewModel.AchseLinks,
                PrismaLinks = _refraktionInputDataControlViewModel.PrismaLinks,
                GesamtprismaVertikal = _refraktionInputDataControlViewModel.GesamtprismaVertikal,
                PupillendistanzLinks = _refraktionInputDataControlViewModel.PupillendistanzLinks,

                Pupillendistanz = _refraktionInputDataControlViewModel.Pupillendistanz,
                Patientenname = _refraktionInputDataControlViewModel.Patientenname,
                PatientenID = _refraktionInputDataControlViewModel.PatientenID
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
            _serialPortSettingsControlViewModel.IsEnabled = canStates.CanChangeConnectionSettings;
            _serialPortSettingsControlViewModel.OnPropertyChanged("IsEnabled");
        }

        public void DisplayRefraktionData(RefraktionData refraktionData)
        {
            _refraktionResultDataControlViewModel.SphäreFernRechts = refraktionData.SphäreFernRechts;
            _refraktionResultDataControlViewModel.SphäreNahRechts = refraktionData.SphäreNahRechts;
            _refraktionResultDataControlViewModel.ZylinderRechts = refraktionData.ZylinderRechts;
            _refraktionResultDataControlViewModel.AchseRechts = refraktionData.AchseRechts;
            _refraktionResultDataControlViewModel.PrismaRechts = refraktionData.PrismaRechts;
            _refraktionResultDataControlViewModel.GesamtprismaHorizontal = refraktionData.GesamtprismaHorizontal;
            _refraktionResultDataControlViewModel.PupillendistanzRechts = refraktionData.PupillendistanzRechts;

            _refraktionResultDataControlViewModel.Visus_S_C_Rechts = refraktionData.Visus_S_C_Rechts;
            _refraktionResultDataControlViewModel.Visus_C_C_Rechts = refraktionData.Visus_C_C_Rechts;
            _refraktionResultDataControlViewModel.AkkommodationsbreiteRechts = refraktionData.AkkommodationsbreiteRechts;

            _refraktionResultDataControlViewModel.SphäreFernLinks = refraktionData.SphäreFernLinks;
            _refraktionResultDataControlViewModel.SphäreNahLinks = refraktionData.SphäreNahLinks;
            _refraktionResultDataControlViewModel.ZylinderLinks = refraktionData.ZylinderLinks;
            _refraktionResultDataControlViewModel.AchseLinks = refraktionData.AchseLinks;
            _refraktionResultDataControlViewModel.PrismaLinks = refraktionData.PrismaLinks;
            _refraktionResultDataControlViewModel.GesamtprismaVertikal = refraktionData.GesamtprismaVertikal;
            _refraktionResultDataControlViewModel.PupillendistanzLinks = refraktionData.PupillendistanzLinks;

            _refraktionResultDataControlViewModel.Visus_S_C_Links = refraktionData.Visus_S_C_Links;
            _refraktionResultDataControlViewModel.Visus_C_C_Links = refraktionData.Visus_C_C_Links;
            _refraktionResultDataControlViewModel.AkkommodationsbreiteLinks = refraktionData.AkkommodationsbreiteLinks;

            _refraktionResultDataControlViewModel.HornhautScheitelAbstand = refraktionData.HornhautScheitelAbstand;
            _refraktionResultDataControlViewModel.Pupillendistanz = refraktionData.Pupillendistanz;
            _refraktionResultDataControlViewModel.Fusionsbreite = refraktionData.Fusionsbreite;
            _refraktionResultDataControlViewModel.Visus_S_C = refraktionData.Visus_S_C;
            _refraktionResultDataControlViewModel.Visus_C_C = refraktionData.Visus_C_C;
            _refraktionResultDataControlViewModel.Patientenname = refraktionData.Patientenname;
            _refraktionResultDataControlViewModel.PatientenID = refraktionData.PatientenID;
            _refraktionResultDataControlViewModel.RefraktionsZeitpunkt = refraktionData.RefraktionsZeitpunkt;
            

        }

        public void Show()
        {
            _mainWindow.ShowDialog();
        }
    }
}