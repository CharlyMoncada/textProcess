using System;
using System.Collections.Generic;
using System.Linq;
using TextProcess.Domain;
using TextProcess.Provider.Helpers;

namespace TextProcess.Provider
{
    public class TextProcessService : ITextProcessService
    {
        public HttpResponse<List<OrderOption>> GetOrderOptions()
        {
            var httpResponse = new HttpResponse<List<OrderOption>>();

            try
            {
                var orderOptions = Enum.GetValues(typeof(OrderOptionsEnum))
                    .Cast<OrderOptionsEnum>()
                    .Select(x => new OrderOption() { Id = (int)x, Option = x.ToString() })
                    .ToList();

                httpResponse.Data = orderOptions;
                httpResponse.IsSuccess = true;

            }
            catch (Exception e)
            {
                httpResponse.Error = e.Message;
            }

            return httpResponse;
        }

        public HttpResponse<List<string>> GetOrderedText(string textToOrder, OrderOptionsEnum orderOption)
        {
            var httpResponse = new HttpResponse<List<string>>();

            try
            {
                textToOrder = textToOrder.UrlDecode();

                var wordList = textToOrder.SeparateBySpaces();
                var listOrdered = GetOrderedList(wordList, orderOption);

                httpResponse.Data = listOrdered;

                httpResponse.IsSuccess = true;
            }
            catch (Exception e)
            {
                httpResponse.Error = e.Message;
            }

            return httpResponse;
        }

        public HttpResponse<TextStatistics> GetStatistics(string textToAnalyze)
        {
            var httpResponse = new HttpResponse<TextStatistics>();

            try
            {
                textToAnalyze = textToAnalyze.UrlDecode();

                var wordsQuantity = textToAnalyze.SeparateBySpaces().Count;
                var hyphenFrequency = textToAnalyze.CountFrequencyByChar('-');
                var spaceFrequency = textToAnalyze.CountFrequencyByChar(' ');

                var statistics = new TextStatistics
                {
                    HyphensQuantity = hyphenFrequency,
                    WordQuantity = wordsQuantity,
                    SpacesQuantity = spaceFrequency
                };

                httpResponse.Data = statistics;
                httpResponse.IsSuccess = true;
            }
            catch (Exception e)
            {
                httpResponse.Error = e.Message;
            }

            return httpResponse;
        }

        private static List<string> GetOrderedList(IEnumerable<string> words, OrderOptionsEnum orderOption)
        {
            return orderOption switch
            {
                OrderOptionsEnum.AlphabeticAcs => words.OrderBy(x => x).ToList(),
                OrderOptionsEnum.AlphabeticDesc => words.OrderByDescending(x => x).ToList(),
                OrderOptionsEnum.LenghtAsc => words.OrderBy(x => x.Length).ToList(),
                _ => throw new ArgumentOutOfRangeException(nameof(orderOption), orderOption, null)
            };
        }
    }
}
