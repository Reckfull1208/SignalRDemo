using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SignalRClient
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
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (hubConnection != null)
            {
                hubConnection.Stop();
                hubConnection.Dispose();
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        private async void ConnetService()
        {
            hubConnection = new HubConnection(ServiceUrl);
            //hubConnection.Closed += HubConnection_Closed;
            hubProxy = hubConnection.CreateHubProxy("SignalRTest");

            //hubProxy.On<string, string>("AddMessage", (name, message) => this.Dispatcher.Invoke(() =>
            //    //log
            //    ConnectTb.Text = name + "----" + message
            // ));

            hubProxy.On("nodify", () => { MessageBox.Show("123"); });

            try
            {
                await hubConnection.Start();
              //  ConnectTb.Text = "连接服务成功";
            }
            catch (HttpClientException ex)
            {
                //log
           //     ConnectTb.Text = "连接服务失败";
                return;
            }
        }

    }
}
