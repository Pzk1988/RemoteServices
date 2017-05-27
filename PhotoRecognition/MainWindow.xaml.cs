using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PhotoRecognition
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string azureRecognitionKey = "1c3d29bde51b4299a715f6e2cd1fa5bf";
        private string imageSource;
        public string ImageSource
        {
            get
            {
                return imageSource;
            }

            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string queryResult;
        public string QueryResult
        {
            get
            {
                return queryResult;
            }

            set
            {
                if(queryResult != value)
                {
                    queryResult = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string query;
        public string Query
        {
            get
            {
                return query;
            }

            set
            {
                if (query != value)
                {
                    query = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ImagePath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ImageSource = openFileDialog.FileName;
                MakeAnalysisRequest(ImageSource);
            }

        }

        async void MakeAnalysisRequest(string imageFilePath)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureRecognitionKey);

            string requestParameters;
            if (combo.SelectedIndex == 0)
            {
                requestParameters = "details=Landmarks&language=en";
            }
            else
            {
                requestParameters = "details=Celebrities&language=en";
            }

            string uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;
            Query = uri;

            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
            }

            string temp = await response.Content.ReadAsStringAsync();
            QueryResult = JsonHelper.FormatJson(temp);
        }

        byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
