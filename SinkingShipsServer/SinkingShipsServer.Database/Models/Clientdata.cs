using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class ClientData
    {
        [Key]
        public int PrimaryKey
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
