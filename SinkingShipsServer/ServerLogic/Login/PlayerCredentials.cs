using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic.Login
{
    public class PlayerCredentials
    {
        public PlayerCredentials()
        {

        }
        public PlayerCredentials(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }

        public int ClientID //TODO
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
