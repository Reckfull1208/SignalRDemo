using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using SignalRClient;
using SignalRModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalRCommon
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 定义
        public static string ServiceUrl = "http://localhost:12345/signalr";
        public IHubProxy hubProxy { get; set; }
        public HubConnection hubConnection { get; set; }
        private string ClientId { get; set; }
        private EasyXml xml;
        /// <summary>
        /// 文件名
        /// </summary>
        private static string FileName { get; set; } 
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
            LoginBtn.IsEnabled = true;
            StopBtn.IsEnabled = false;
            xml = new EasyXml();
        }

        #region 方法

        /// <summary>
        /// 连接服务器
        /// </summary>
        private async void ConnetService()
        {
            hubConnection = new HubConnection(ServiceUrl);
            //hubConnection.Closed += HubConnection_Closed;
            hubProxy = hubConnection.CreateHubProxy("SignalRHub");
             
            //接收
            hubProxy.On<string, string>("SendMsg", (name, message) => this.Dispatcher.Invoke(() =>
                //log
                ClientInfor.Text = name + "----" + message
             ));
             
            try
            {
                await hubConnection.Start();
                ClientInfor.Text = "连接服务成功";
                LoginBtn.IsEnabled = false;
                StopBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //log
                ClientInfor.Text = "连接服务失败";
                LoginBtn.IsEnabled = true;
                StopBtn.IsEnabled = false;
                return;
            }
        }
         

        /// <summary>
        /// 向服务器注册本地信息，并写入本地
        /// </summary>
        private bool RegisterInfoToService()
        {
            //拉取本地数据
            var user = xml.ReadDocs().FirstOrDefault();
            //无数据需要创建数据并发送给服务器 并从服务器返回当前使用的clientid
            if(user == null || user.User == null)
            {
                UserNameDialog nd = new UserNameDialog();
                if(nd.ShowDialog() == true)
                {
                    xml.WriteDoc(JsonConvert.SerializeObject(new UserInfor() { ClientId = "", User = nd.UserName }));

                    //发送至服务器
                    hubProxy.On<UserInfor>("Register", UserInfor => SynchronizationContext.Current.Post(delegate {
                        var a = UserInfor;
                    }, null));
                } 
            } 
            return true;
        }

        #endregion



        #region 事件

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (hubConnection != null)
            {
                hubConnection.Stop();
                hubConnection.Dispose();
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnetService(); 
            
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (hubConnection != null)
            {
                hubConnection.Stop();
                hubConnection.Dispose();
            }
            ClientInfor.Text = "停止连接服务";
            LoginBtn.IsEnabled = true;
            StopBtn.IsEnabled = false;
        }

        private void ReLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (hubConnection != null)
            //{
            //    hubConnection.Stop();
            //    hubConnection.Dispose();
            //}
            //ConnetService();
            RegisterInfoToService();
        }

        private void CorrentBtn_Click(object sender, RoutedEventArgs e)
        {
            //拉取本地数据
            var user = xml.ReadDocs().FirstOrDefault(); 
            UserNameDialog nd = new UserNameDialog();
            if (nd.ShowDialog() == true)
            {
                UserInfor info = new UserInfor();
                info.OldUser = user.User;
                info.ClientId = user.ClientId;
                info.User = nd.UserName;

                //发送至服务器

            }
        }
        #endregion


    }
}
