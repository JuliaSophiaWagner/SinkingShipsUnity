using System;
using System.Threading.Tasks;

namespace ClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Client client = new Client();

            await client.Test();
        }
    }
}
