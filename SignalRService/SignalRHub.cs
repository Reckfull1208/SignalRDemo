using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SignalRService
{
    public class SignalRHub : Hub
    {
        public override Task OnConnected()
        {
            try
            {
                  
            }
            catch
            {

            }
             
            return base.OnConnected();
        }
          

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        public void SendMsg(string user, string msg)
        {
            Clients.All.boradcastMsg(user, msg);
        }

        public void Register(string user)
        {
            Clients.Others.Register(user);
        }

    }
}
