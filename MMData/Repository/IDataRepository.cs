using Newtonsoft.Json.Linq;
using System.Data;

namespace MMData.Repository
{
    public interface IDataRepository
    {
        int PostQuery(string spName, JArray parameters);
        DataSet GetQuery(string spName, JArray parameters);
    }
}
