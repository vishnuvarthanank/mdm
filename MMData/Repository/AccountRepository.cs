using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MMData.Repository
{
    public class AccountRepository : IAccountRepository
    {
        DataRepository dataRepository;
        public AccountRepository()
        {
            dataRepository = new DataRepository();
        }

        public int ValidateUser(string spName, JArray parameters)
        {
            return dataRepository.PostQuery(spName, parameters);
        }
    }
}