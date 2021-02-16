using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class UserData
    {
        public UserData(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }

        public UserData()
        {
                
        }

        public string Name { get; set; }
        public string Password { get; set; }
    }
}
