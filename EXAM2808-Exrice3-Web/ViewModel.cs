using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EXAM2808_Exrice3_Web
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string startOfUrl;
        private string myUrl;
        public string MyUrl { get { return this.myUrl; } set { this.myUrl = value; OnPropertyChanged("MyUrl"); } }

        private string urlSize;
        public string UrlSize { get { return urlSize; } set { urlSize = value; OnPropertyChanged("UrlSize"); } }

        public string ValueOfStopWatch
        {
            get
            {
                if (myStopWatch.ElapsedMilliseconds == 0) return "0";
                if(MyStopWatch.IsRunning == false && myStopWatch.ElapsedMilliseconds != 0) return $"You received The Respone At: {MyStopWatch.ElapsedMilliseconds.ToString()} MilliSeconds";
                return MyStopWatch.ElapsedMilliseconds.ToString();
            }
            set
            {
                OnPropertyChanged("ValueOfStopWatch");
            }
        }

        private Stopwatch myStopWatch;
        public Stopwatch MyStopWatch { get { return this.myStopWatch; } set { this.myStopWatch = value;} }


        public DelegateCommand StartCommand { get; set; }
        public bool ProccessIsDone { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel()
        {
            startOfUrl = "http";
            myUrl = "";
            urlSize = "Please Wait...";
            myStopWatch = new Stopwatch();

            StartCommand = new DelegateCommand(() =>
            {
                CheckUrl(MyUrl);
            },
            () =>
            {
                return MyUrl.StartsWith(startOfUrl);
            });

            Task.Run(() =>
            {
                while (true)
                {
                    StartCommand.RaiseCanExecuteChanged();
                    Thread.Sleep(500);
                }
            });
            Task.Run(() =>
            {
                while (true)
                {
                    ValueOfStopWatch = MyStopWatch.ElapsedMilliseconds.ToString();
                }
            });
        }

        public async void CheckUrl(string url)
        {
            ProccessIsDone = false;
            MyStopWatch.Restart();
            WebRequest webRequest = WebRequest.Create(url);
            WebResponse response = await webRequest.GetResponseAsync();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string numberOfCharacters = await reader.ReadToEndAsync();
                UrlSize = $"The Size Of Your Site Is: {numberOfCharacters.Length.ToString()} Bytes."; 
            }
            ProccessIsDone = true;
            MyStopWatch.Stop();
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


    }
}