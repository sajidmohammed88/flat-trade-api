using FlatTradeLibrary;
using FlatTradeLibrary.Models;
using System;
using System.Threading.Tasks;

namespace FlatTradeConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TokenGenerator tokenGenerator = new TokenGenerator();
            TokenResponse response = await tokenGenerator.LoginAndGetAccessTokenAsync(new Request
            {
                ApiKey = "dca393302cb57d519c32004e080c2a07f2376e2455a0f1fec5ff76772ac749c0",
                UserName = "INV102",
                Password = "*****",
                Pan = "ASASA1111A",
                ApiSecret = "0ccf18c2f2923ebb800b08a1bb377678545a419630faff8dfd2573ede641dc6a"
            });

            Console.WriteLine(response.Token);
        }
    }
}
