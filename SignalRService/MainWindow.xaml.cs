using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
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

[assembly: OwinStartup(typeof(SignalRService.Startup))]
namespace SignalRService
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 定义
        public IDisposable SingalR { get; set; }
        private static string ServiceUrl = "http://localhost:12345";
        
        IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
