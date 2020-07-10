using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

[assembly: OwinStartup(typeof(SignalRService.Startup))]
namespace SignalRService
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 定义
        public IDisposable SignalR { get; set; }
        private static string ServiceUrl = "http://localhost:12345";
        private string CurrentPath;
        IHubContext hub;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            CurrentPath = Directory.GetCurrentDirectory();
            Closed += MainWindow_Closed;
            StartService();
            hub = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
             
        }
         
        /// <summary>
        /// 启动服务
        /// </summary>
        private void StartService()
        {
            try
            {
                SignalR = WebApp.Start(ServiceUrl);
                Dispatcher.Invoke(() => StartBtn.IsEnabled = false);
                Dispatcher.Invoke(() => StopBtn.IsEnabled = true);
                Dispatcher.Invoke(() => ServiceInforTb.Text = "服务启动成功");
            }
            catch(TargetInvocationException)
            {
                Dispatcher.Invoke(() => StartBtn.IsEnabled = true);
                Dispatcher.Invoke(() => StopBtn.IsEnabled = false);
                Dispatcher.Invoke(() => ServiceInforTb.Text = "服务启动失败");
            }
        }

        #region 事件
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (SignalR != null)
                SignalR.Dispose();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartService();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SignalR != null)
            {
                SignalR.Dispose();
                Dispatcher.Invoke(() => StartBtn.IsEnabled = true);
                Dispatcher.Invoke(() => StopBtn.IsEnabled = false);
                Dispatcher.Invoke(() => ServiceInforTb.Text = "服务已停止");
            }        
        }

        private void ReStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SignalR != null)
            {
                SignalR.Dispose(); 
            }
            Dispatcher.Invoke(() => ServiceInforTb.Text = "服务正在重启");
            StartService();
        }

        #endregion


    }
}
