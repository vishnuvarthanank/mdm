using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MMData.Repository
{
    public class DataRepository : IDataRepository
    {
        private string connectionString = "";
        public DataRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["strConn"].ToString();
        }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
        // Start - Insert, Update related methods
        public int PostQuery(string spName, JArray parameters)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            int intAffectRow = 0;
            try
            {
                con = DbConnection();
                cmd = DbCommand(con, spName);
                intAffectRow = ProcessPostQuery(cmd, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return intAffectRow;
        }

        private int ProcessPostQuery(SqlCommand cmd, JArray parameters)
        {
            int intAffectRow = 0;
            SqlTransaction trans = null;
            try
            {
                bool blnTransStart = true;
                foreach (JObject record in parameters)
                {
                    if (parameters.Count > 1 && blnTransStart)
                    {
                        trans = cmd.Connection.BeginTransaction();
                        cmd.Transaction = trans;
                        blnTransStart = false;
                    }
                    foreach (KeyValuePair<string, JToken> filed in record)
                    {
                        cmd.Parameters.AddWithValue("@" + filed.Key, filed.Value.ToString());
                    }
                    intAffectRow = intAffectRow + Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                }
                if (parameters.Count > 1)
                {
                    trans.Commit();
                }
            }
            catch(Exception ex)
            {
                if (parameters.Count > 1)
                {
                    trans.Rollback();
                }
                throw ex;
            }
            return intAffectRow;
        }
        
        // End - Insert, Update related methods

        // Start - Retrieve related methods
        public DataSet GetQuery(string spName, JArray parameters)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            DataSet dbSet = null;
            try
            {
                con = DbConnection();
                cmd = DbCommand(con, spName);
                dbSet = ProcessGetQuery(cmd, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dbSet;
        }

        private DataSet ProcessGetQuery(SqlCommand cmd, JArray parameters)
        {
            DataSet dbSet = new DataSet();
            try
            {
                if (parameters != null)
                {
                    foreach (JObject record in parameters)
                    {
                        foreach (KeyValuePair<string, JToken> filed in record)
                        {
                            cmd.Parameters.AddWithValue("@" + filed.Key, filed.Value);
                        }
                    }
                }
                GetResult(ref dbSet, cmd);
                cmd.Parameters.Clear();
                return dbSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet GetResult(ref DataSet dbSet, SqlCommand cmd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dbSet);
            return dbSet;
        }

        // End - Retrieve related methods

        // Start - Common Methods 
        public SqlConnection DbConnection()
        {
            SqlConnection con = new SqlConnection(this.ConnectionString);
            con.Open();
            return con;
        }

        public SqlCommand DbCommand(SqlConnection con, string spName)
        {
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }
        // End - Common Methods 
    }
}