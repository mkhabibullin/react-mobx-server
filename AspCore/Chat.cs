using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AspCore
{
    public class Chat : Hub
    {
        public async Task Send(string nick, string message)
        {
            await Clients.All.SendAsync("Send", nick, message);
        }
    }
}
