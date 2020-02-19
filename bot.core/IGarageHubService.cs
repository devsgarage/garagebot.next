using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bot.core
{
    public interface IGarageHubService
    {
        Task Connect();
        Task SendAlert(string user);
    }
}
