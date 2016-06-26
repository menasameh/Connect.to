using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace backOne.Hubs
{
    public class NotificationsHub : Hub
    {
        public void Hello()
        {
            Clients.All.warrning();
        }
    }
}