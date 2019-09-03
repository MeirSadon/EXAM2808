using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EXAM2808_Exrice2_CalcQuestions
{
    class ViewModel : DependencyObject ,INotifyPropertyChanged
    {
        private Random r = new Random();
        private List<int> FirstNumbers = new List<int>();
        private List<int> SecondNumbers = new List<int>();

        private string exriceAsString;
        public string ExriceAsString { get { return exriceAsString; } set { exriceAsString = value; OnPropertyChanged("ExriceAsString"); } }

        private int correctResult;
        public int CorrectResult { get { return correctResult; } set { correctResult = value; OnPropertyChanged("CorrectResult"); } }

        private ObservableCollection<int> wrongNumbers;
        public ObservableCollection<int> WrongNumbers { get { return wrongNumbers; } set { wrongNumbers = value; OnPropertyChanged("WrongNumbers"); } }

        private ObservableCollection<string> positions;
        public ObservableCollection<string> Positions { get{ return positions; } set { positions = value; OnPropertyChanged("Positions"); } }

        private List<bool> PositionIsFree = new List<bool>();

        private int index;
        public int Index { get { return index; }
            set
            {
                index = value;
                OnPropertyChanged("Index");
            }
        }

        public DelegateCommand CorrectResultCommand { get; set; }
        public DelegateCommand WrongResultCommand { get; set; }

        private bool playIsRunning = true;
        public static readonly DependencyProperty CounterProperty = DependencyProperty.Register("Counter", typeof(int), typeof(ViewModel), new PropertyMetadata(3));
        public int Counter
        {
            get
            {
                int x = 0;
                Action a = new Action(() =>
                {
                    x = (int)GetValue(CounterProperty);
                });
                Dispatcher.Invoke(a);
                return x;
            }
                set
            {
                Action a = new Action(() =>
                {
                    SetValue(CounterProperty, value);
                });
                Dispatcher.Invoke(a);
                 OnPropertyChanged("Counter");
            }
        }


        public ViewModel()
        {
            WrongNumbers = new ObservableCollection<int>();

            for (int i = 0; i < 4; i++)
            {
                PositionIsFree.Add(true);
            }
            Positions = new ObservableCollection<string>
            {
                "180 80 0 0",
                "180 0 0 150",
                "0 0 190 150",
                "0 80 190 0"
            };
            index = 1;
            for (int i = 0; i < 15; i++)
            {
                FirstNumbers.Add(r.Next(30));
                SecondNumbers.Add(r.Next(30));
                WrongNumbers.Add(r.Next(75));
                while (WrongNumbers[i] == (FirstNumbers[i] + SecondNumbers[i]))
                {
                    WrongNumbers[i] = r.Next(75);
                }
            }

            CorrectResult = FirstNumbers[Index - 1] + SecondNumbers[Index - 1];
            ExriceAsString = $"{FirstNumbers[Index-1]} + {SecondNumbers[Index-1]} =";
            Counter = 30;
            Task.Run(() =>
            {
                while (playIsRunning == true)
                {
                    if (Counter > 0)
                    {
                        Counter--;
                        Thread.Sleep(1000);
                    }
                }
                Counter = -1;
            });

            CorrectResultCommand = new DelegateCommand(() =>
            {
                ClickCorrectResult();
            }, () =>
            {
                return Counter > 0;
            });

            WrongResultCommand = new DelegateCommand(() =>
            {
                ClickWrongResult();
            }, () =>
            {
                return Counter > 0;
            });

            Task.Run(() =>
            {
                while (true)
                {
                    CorrectResultCommand.RaiseCanExecuteChanged();
                    WrongResultCommand.RaiseCanExecuteChanged();
                    Thread.Sleep(500);
                }
            });

        }

        private void ClickCorrectResult()
        {
            Index++;
            if (Index != 16)
            {
                ExriceAsString = $"{FirstNumbers[Index - 1]} + {SecondNumbers[Index - 1]} =";
                CorrectResult = FirstNumbers[Index - 1] + SecondNumbers[Index - 1];

                for (int i = 0; i < 4; i++)
                {
                    WrongNumbers[i] = r.Next(75);
                    while (WrongNumbers[i] == correctResult)
                    {
                        WrongNumbers[i] = r.Next(75);
                    }
                    PositionIsFree[i] = true;
                }

                for (int i = 0; i < 2; i++)
                {
                    int position = r.Next(4);
                    while (PositionIsFree[position] == false)
                    {
                        position = r.Next(4);
                    }
                    PositionIsFree[position] = false;
                    PositionIsFree[i] = false;
                    string currentPostion = Positions[i];
                    Positions[i] = Positions[position];
                    Positions[position] = currentPostion;
                }

                Counter = 30;
            }

        }

        private void ClickWrongResult()
        {
            playIsRunning = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
