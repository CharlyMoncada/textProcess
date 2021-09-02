using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using TextProcess.Domain;

namespace TextProcess.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string WordQuantity = "Word Quantity";
        private const string SpacesQuantity = "Spaces Quantity";
        private const string HyphensQuantity = "Hyphens Quantity";

        private readonly IHttpClientFactory httpClientFactory;

        public MainWindow(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            InitializeComponent();
            PopulateOrderOptionsComboBox();
        }

        private void PopulateOrderOptionsComboBox()
        {
            var orderOptions = GetOrderOptions();
            orderOptionsComboBox.ItemsSource =
                orderOptions.Select(x => new ComboBoxItem()
                {
                    Tag = x.Id,
                    Content = x.Option
                });

            orderOptionsComboBox.SelectedIndex = 0;
        }

        private void orderTextButton_Click(object sender, RoutedEventArgs e)
        {
            var text = analyzeTextBox.Text;

            if (string.IsNullOrEmpty(text)) { return; }

            var order = orderOptionsComboBox.SelectedItem as ComboBoxItem;
            var result = GetOrderedText(text, (int)order.Tag);

            orderedTextListView.ItemsSource = result;
        }

        private void statisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var text = analyzeTextBox.Text;

            if (string.IsNullOrEmpty(text)) { return; }
            
            var response = GetStatistics(text);

            statisticsWordQuantityLabel.Content = $"{WordQuantity}: {response.WordQuantity}";
            statisticsSpacesQuantityLabel.Content = $"{SpacesQuantity}: {response.SpacesQuantity}";
            statisticsHyphensQuantityLabel.Content = $"{HyphensQuantity}: {response.HyphensQuantity}";
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            analyzeTextBox.Text = string.Empty;
            orderedTextListView.ItemsSource = null;
            statisticsWordQuantityLabel.Content = $"{WordQuantity}: ?";
            statisticsSpacesQuantityLabel.Content = $"{SpacesQuantity}: ?";
            statisticsHyphensQuantityLabel.Content = $"{HyphensQuantity}: ?";
            orderOptionsComboBox.SelectedIndex = 0;
        }

        private IEnumerable<OrderOption> GetOrderOptions()
        {
            var response = ProcessRequest<HttpResponse<List<OrderOption>>>($"textProcess/orderOptions").Result;

            return response.IsSuccess ? response.Data : throw new Exception(response.Error);
        }

        private IEnumerable<string> GetOrderedText(string textToOrder, int orderOption)
        {
            var textEncoded = HttpUtility.UrlEncode(textToOrder);
            var response = ProcessRequest<HttpResponse<List<string>>>($"textProcess/orderedText/{textEncoded}/{orderOption}").Result;

            return response.IsSuccess ? response.Data : throw new Exception(response.Error);
        }

        private TextStatistics GetStatistics(string textToAnalyze)
        {
            var textEncoded = HttpUtility.UrlEncode(textToAnalyze);
            var response = ProcessRequest<HttpResponse<TextStatistics>>($"textProcess/statistics/{textEncoded}").Result;

            return response.IsSuccess ? response.Data : throw new Exception(response.Error);
        }

        private async Task<T> ProcessRequest<T>(string url)
        {
            using var httpClient = httpClientFactory.CreateClient("local");

            var response = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, url));

            try
            {
                return response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())
                    : throw new Exception();
            }
            finally
            {
                response.Dispose();
            }
        }
    }
}
