using Microsoft.AspNetCore.Mvc;

namespace ems.Helpers.Alert
{
    public static class AlertExtensions
    {
        public static ActionResult WithSuccess(this ActionResult result, string title, string body)
        {
            return Alert(result, "success", title, body);
        }

        private static ActionResult Alert(ActionResult result, string type, string title, string body)
        {
            return new AlertDecoratorResult(result, type, title, body);
        }
    }
}