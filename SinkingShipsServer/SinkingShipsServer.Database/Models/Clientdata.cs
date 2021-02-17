using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class Clientdata
    {
        public int Id
        {
            get;
            set;
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
