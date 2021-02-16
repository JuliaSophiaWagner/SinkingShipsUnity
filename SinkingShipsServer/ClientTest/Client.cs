using Newtonsoft.Json;
using ServerLogic;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    public class Client
    {
        private readonly HttpClient client;

        public Client()
        {
            this.client = new HttpClient();
        }

        public async Task Test()
        {
            //string response = await this.client.GetStringAsync("https://localhost:44367/ShipSinking");
            var content = new ClientData(0,"test","test");
            var json = JsonConvert.SerializeObject(content);
            var data = new StringContent(json, Encoding.UTF8);

            var response = await this.client.PostAsync("https://localhost:44367/ShipSinking/api/requestGame", data);
            
            var result = await response.Content.ReadAsStringAsync();
            var info2 = JsonConvert.DeserializeObject<ClientData>(result);

            Console.ReadKey();
        }
    }
}
