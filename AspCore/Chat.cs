using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore
{
    public class Chat : Hub
    {
        public async Task send(string nick, string message)
        {
            await Clients.All.SendAsync("Send", message);
        }
    }
}
