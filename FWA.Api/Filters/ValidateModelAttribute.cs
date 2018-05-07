using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FWA.Api.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                log.Debug(string.Format("{0}= BadRequest{1}", actionContext.Request.RequestUri.AbsolutePath, JsonConvert.SerializeObject(actionContext.ModelState, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}