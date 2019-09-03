using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace EXAM2808_Exrice3_Web
{

    public partial class MainWindow : Window
    {
        public ViewModel vm = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.MyStopWatch.Restart();
            string text = "";

            StartWithDispatcherBtn.IsEnabled = false;
            string myUrl = UrlTxtBx.Text;

            Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    text = client.DownloadString(myUrl);
                }

                int size = ASCIIEncoding.ASCII.GetByteCount(text);

                Action uiwork = () =>
                {
                    SizeTxtBlck.Text = $"The Size Of Your Site Is: {size} Bytes.";
                    StartWithDispatcherBtn.IsEnabled = true;
                };

                SafeInvoke(uiwork);
                vm.MyStopWatch.Stop();
            });
        }

        private void SafeInvoke(Action work)
        {
            if(Dispatcher.CheckAccess())
            {
                work.Invoke();
                return;
            }
            Dispatcher.Invoke(work);
        }

        private void Counter()
        {
            while (vm.MyStopWatch.IsRunning)
            {
                SafeInvoke(new Action(() => { TimeTxtBlck.Text = vm.ValueOfStopWatch.ToString(); }));
                Thread.Sleep(10);
            }
            TimeTxtBlck.Text = $"You received The Respone At: {vm.ValueOfStopWatch.ToString()} MilliSeconds";
        }
    }
}
