﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMData.Repository
{
    interface IAccountRepository
    {
        int ValidateUser(string spName, JArray parameters);
    }
}
