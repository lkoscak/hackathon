using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BaseApiContext.Validation.Attribute
{
    public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;

            if (!modelState.IsValid)
            {
                var message = string.Join(", ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }
    }
}
