using MMData.Models;
using MMData.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace MMData.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        private AccountRepository _accountRepository = null;
        public AccountController()
        {
            _accountRepository = new AccountRepository();
        }

        [HttpPost]
        [Route("validate")]
        public HttpResponseMessage ValidateUser(GenericObject objParam)
        {
            int intResult = _accountRepository.ValidateUser(objParam.spName, objParam.parameters);
            bool isAuthenticated = (intResult > 0) ? true : false;
            if (isAuthenticated)
            {
                FormsAuthentication.SetAuthCookie(GetUserFromParameter(objParam.parameters), false);
            }
            return Request.CreateResponse(HttpStatusCode.OK, intResult);
        }

        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage Logout()
        {
            FormsAuthentication.SignOut();
            return Request.CreateResponse(HttpStatusCode.OK, "0");
        }

        private string GetUserFromParameter(JArray parameters)
        {
            JObject user = parameters.ToObject<List<JObject>>().FirstOrDefault();
            if(user != null)
            {
                return user["UserName"].ToString();
            }
            return string.Empty;
        }
    }
}
