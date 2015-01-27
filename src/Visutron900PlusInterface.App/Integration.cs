using Visutron900PlusInterface.UserInterface;

namespace Visutron900PlusInterface.App
{
    public class Integration
    {
        public void Run()
        {
            var model = new ModelLayer();
            var ui = new UserInterfaceLayer();

            ui.OnCloseConnection += model.CloseConnection;
            ui.OnCreateConnection+= model.CreateConnection;
            ui.OnOpenConnection += model.OpenConnection;
            ui.OnSendRefraktionData += model.SendRefraktionData;
            model.OnRefraktionDataReceived += ui.DisplayRefraktionData;
            model.OnCanChanged += ui.CanChanged;

            ui.Show();
        }
    }
}