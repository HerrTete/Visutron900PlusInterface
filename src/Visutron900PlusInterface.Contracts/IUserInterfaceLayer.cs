using System;

using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Contracts
{
    public interface IUserInterfaceLayer
    {
        event Action<SerialConnectionSettings> OnCreateConnection;
        event Action OnOpenConnection;
        event Action OnCloseConnection;
        event Action<RefraktionData> OnSendRefraktionData;

        void DisplayRefraktionData(RefraktionData refraktionData);
        void CanChanged(CanStates canStates);

        void Show();
    }
}