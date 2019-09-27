using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMData.Models
{
    public class GenericObject
    {
        public string spName { get; set; }
        public JArray parameters { get; set; }
    }
}