using System.Collections.Generic;
using System.Linq;
using TextProcess.API.Controllers;
using TextProcess.Domain;
using TextProcess.Provider;
using Xunit;

namespace TextProcess.API.Tests.Controllers
{

    public class TextProcessControllerTest
    {
        private TextProcessController controller;
        private readonly ITextProcessService textProcessService;
        public TextProcessControllerTest()
        {
            textProcessService = new TextProcessService();
            controller = new TextProcessController(textProcessService);
        }

        [Fact]
        public void GetOrderOptionsShouldReturnOk()
        {
            var response = controller.GetOrderOptions();

            Assert.True(response.Value.IsSuccess);
            Assert.NotNull(response.Value.Data);
            Assert.True(response.Value.Data.Count == 3);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetOrderedTextDataGenerator), MemberType = typeof(TestDataGenerator))]
        public void GetOrderedTextShouldReturnOk(string text, int order, List<string> expectedOrderedList)
        {
            var response = controller.GetOrderedText(text, order);

            Assert.True(response.Value.IsSuccess);
            Assert.NotNull(response.Value.Data);
            Assert.True(response.Value.Data.SequenceEqual(expectedOrderedList));
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetStatisticsDataGenerator), MemberType = typeof(TestDataGenerator))]
        public void GetStatisticsShouldReturnOk(string text, TextStatistics expectedResult)
        {
            var response = controller.GetStatistics(text);

            Assert.True(response.Value.IsSuccess);
            Assert.NotNull(response.Value.Data);
            Assert.Equal(response.Value.Data.SpacesQuantity, expectedResult.SpacesQuantity);
            Assert.Equal(response.Value.Data.HyphensQuantity, expectedResult.HyphensQuantity);
            Assert.Equal(response.Value.Data.WordQuantity, expectedResult.WordQuantity);
        }
    }

    public class TestDataGenerator
    {
        public static IEnumerable<object[]> GetOrderedTextDataGenerator()
        {
            yield return new object[] { "this hello instead", 1, new List<string>() { "hello", "instead", "this" } };
            yield return new object[] { "this hello instead", 2, new List<string>() { "this", "instead", "hello" } };
            yield return new object[] { "this hello instead", 3, new List<string>() { "this", "hello", "instead" } };
        }

        public static IEnumerable<object[]> GetStatisticsDataGenerator()
        {
            yield return new object[] { "this hello instead", new TextStatistics() { WordQuantity = 3, HyphensQuantity = 0, SpacesQuantity = 2} };
            yield return new object[] { "this-hello instead", new TextStatistics() { WordQuantity = 2, HyphensQuantity = 1, SpacesQuantity = 1 } };
        }
    }
}
