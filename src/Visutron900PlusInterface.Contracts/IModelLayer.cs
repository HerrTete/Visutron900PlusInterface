using System;

using Visutron900PlusInterface.Messages.DTOs;

namespace Visutron900PlusInterface.Contracts
{
    public interface IModelLayer
    {
        void CloseConnection();
        void CreateConnection(SerialConnectionSettings serialConnectionSettings);
        void OpenConnection();
        void SendRefraktionData(RefraktionData refraktionData);

        event Action<CanStates> OnCanChanged;
        event Action<RefraktionData> OnRefraktionDataReceived;
    }
}