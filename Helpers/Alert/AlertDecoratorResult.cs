using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ems.Helpers.Alert
{
    public class AlertDecoratorResult : ActionResult
    {
        private readonly IActionResult _result;
        private readonly string _title;
        private readonly string _type;
        private readonly string _body;

        public AlertDecoratorResult(IActionResult result, string type, string title, string body)
        {
            _result = result;
            _title = title;
            _type = type;
            _body = body;
        }


        public override Task ExecuteResultAsync(ActionContext context)
        {
            var tempDataFactory = context.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory.GetTempData(context.HttpContext);

            tempData["alert-title"] = _title;
            tempData["alert-type"] = _type;
            tempData["alert-body"] = _body;
            return _result.ExecuteResultAsync(context);

        }
    }
}