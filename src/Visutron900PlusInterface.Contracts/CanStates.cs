﻿namespace Visutron900PlusInterface.Contracts
{
    public class CanStates
    {
        public bool CanOpenConnection { get; set; }
        public bool CanCloseConnection { get; set; }
        public bool CanChangeConnectionSettings { get; set; }
        public bool CanCreateConnection { get; set; }
        public bool CanSend { get; set; }
    }
}