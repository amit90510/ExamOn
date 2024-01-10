using Dapper;
using ExamOn.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ExamOn.ServiceLayer
{
    public static class DapperService
    {
        public static List<T> GetDapperData<T>(string query, object param = null, string DBName = "") where T : GenericModel
        {
            try
            {
                using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString(DBName)))
                {
                    var GetModel = mainDB.Query<T>(query, param);
                    return GetModel.ToList();
                }
            }
            catch(Exception _)
            {
                return null; 
            }
        }

        public static async Task<string> ExecuteQuery(string query, object param = null, string DBName = "")
        {
            string executed = string.Empty;
            try
            {
                using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString(DBName)))
                {
                    var GetModel = await mainDB.ExecuteAsync(query, param);
                }
            }
            catch (Exception queryExc)
            {
                executed = queryExc.Message;
            }
            return executed;
        }
    }
}