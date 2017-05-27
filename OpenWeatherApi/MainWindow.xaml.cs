using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System;
using System.Windows.Threading;

namespace OpenWeatherApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string inputText { get; set; }
        public string myApiId { get; set; }

        private string outputText;
        public string OutputText
        {
            get
            {
                return outputText;
            }
            set
            {
                if(outputText != value)
                {
                    outputText = value;
                    NotifyPropertyChanged();
                }
            }
        }
        

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();

            inputText = "Test";
            outputText = "Test";
            myApiId = "18526428403d6156a96193340616fefe";
            DataContext = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => GetData());
        }

        private void GetData()
        {
            JObject jObject = JObject.Parse(new System.Net.WebClient().
            DownloadString(string.Format("http://api.openweathermap.org/data/2.5/weather?appid={0}&q={1}", myApiId, inputText)));

            JToken weather = jObject.SelectToken("weather");

            //Application.Current.Dispatcher.BeginInvoke(
            //    DispatcherPriority.Background,
            //    new Action(() => outputBox.Text = weather.First.SelectToken("description").ToString()
            //    )
            //    );
            OutputText = weather.First.SelectToken("description").ToString();

        }
    }
}
