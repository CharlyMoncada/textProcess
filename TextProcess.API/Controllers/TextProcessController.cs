using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TextProcess.Domain;
using TextProcess.Provider;

namespace TextProcess.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextProcessController : ControllerBase
    {
        private readonly ITextProcessService textProcessService;

        public TextProcessController(ITextProcessService textProcessService)
        {
            this.textProcessService = textProcessService;
        }

        [HttpGet]
        [Route("orderOptions")]
        public ActionResult<HttpResponse<List<OrderOption>>> GetOrderOptions()
        {
            return textProcessService.GetOrderOptions();
        }

        [HttpGet]
        [Route("orderedText/{textToOrder}/{orderOption:int}")]
        public ActionResult<HttpResponse<List<string>>> GetOrderedText(string textToOrder, int orderOption)
        {
            return textProcessService.GetOrderedText(textToOrder, (OrderOptionsEnum) orderOption);
        }

        [HttpGet]
        [Route("statistics/{textToAnalyze}")]
        public ActionResult<HttpResponse<TextStatistics>> GetStatistics(string textToAnalyze)
        {
            return textProcessService.GetStatistics(textToAnalyze);
        }
    }
}
