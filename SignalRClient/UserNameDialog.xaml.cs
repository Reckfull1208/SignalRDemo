using Newtonsoft.Json;
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
    /// UserNameDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UserNameDialog : Window
    {
        public string UserName;
        public UserNameDialog()
        {
            InitializeComponent();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(UserNameTb.Text))
            {
                UserName = UserNameTb.Text;
                DialogResult = true;
                Close();
            } 
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
