using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcess.Domain;

namespace TextProcess.Provider
{
    public interface ITextProcessService
    {
        HttpResponse<List<OrderOption>> GetOrderOptions();

        HttpResponse<List<string>> GetOrderedText(string textToOrder, OrderOptionsEnum orderOption);

        HttpResponse<TextStatistics> GetStatistics(string textToAnalyze);
    }
}
