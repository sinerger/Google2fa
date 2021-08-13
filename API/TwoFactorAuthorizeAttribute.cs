using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace API
{
    public class TwoFactorAuthorizeAttribute : Attribute, IResourceFilter
    {
        private bool _isCorrectPIN = true;

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var leadID = context.HttpContext.User.Identities.ToList()[0].Name;
            var key = leadID; //TODO: Обращаемся к сервису или к бд и получаем секред кей для ТФА

            var pin = context.HttpContext.Request.Cookies["TFAPin"];
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            TimeSpan ts = new TimeSpan(0, 0, 1);
            _isCorrectPIN = tfa.ValidateTwoFactorPIN(key, pin, ts);

            if (!_isCorrectPIN)
            {
                context.Result = new ContentResult() {StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}
