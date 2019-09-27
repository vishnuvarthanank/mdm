using MMData.Models;
using MMData.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MMData.Controllers
{
    [RoutePrefix("api/Data")]
    [OnlyAuthUsers]
    public class DataController : ApiController
    {
        private DataRepository _dataRepository = null;
        public DataController()
        {
            _dataRepository = new DataRepository();
        }
        [HttpGet]
        [Route("Get")]
        public HttpResponseMessage GetQuery([FromUri]GenericObject objParam)
        {
            return ProcessQuery("Get", objParam.spName, objParam.parameters);
        }

        [HttpPost]
        [Route("Post")]
        public HttpResponseMessage PostQuery(GenericObject objParam)
        {
            return ProcessQuery("Post", objParam.spName, objParam.parameters);
        }

        private HttpResponseMessage ProcessQuery(string action, string spName, JArray _parameters)
        {
            if (action == "Get")
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataRepository.GetQuery(spName, _parameters));
            }
            return Request.CreateResponse(HttpStatusCode.OK, _dataRepository.PostQuery(spName, _parameters));
        }
    }
}
