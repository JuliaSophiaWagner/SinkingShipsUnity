using ServerLogic.GameParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic
{
    public class ClientData
    {
        public ClientData(string id, string name, string password)
        {
            this.Name = name;
            this.Password = password;
            this.ID = id;
            this.Token = string.Empty;
            this.GameRequests = new List<Player>();
            this.History = new List<History>();
        }

        public ClientData()
        {

        }

        public string Token
        {
            get;
            set;
        }

        public string ID
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public List<Player> GameRequests
        {
            get; 
            set;
        }

        public List<History> History
        {
            get;
            set;
        }

        public int Won
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }
    }
}
