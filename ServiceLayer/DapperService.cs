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

        public static List<dynamic> GetDapperDataDynamic<dynamic>(string query, object param = null, string DBName = "")
        {
            try
            {
                using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString(DBName)))
                {
                    var GetModel = mainDB.Query<dynamic>(query, param);
                    return GetModel.ToList();
                }
            }
            catch (Exception _)
            {
                return null;
            }
        }

        public static List<T2> GetDapperDataMultipleRelation<T, T1, T2>(string query, System.Func<T, T1, T2> func, string spiltOn, object param = null, string DBName = "") 
            where T : GenericModel
            where T1 : GenericModel
            where T2 : GenericModel
        {
            try
            {
                using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString(DBName)))
                {
                    var GetModel = mainDB.Query<T, T1, T2>(query,func, param, splitOn: spiltOn);
                    return GetModel.ToList();
                }
            }
            catch (Exception _)
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
        public static async Task<string> ExecuteQueryMultiple(string multipleQueryWithSemicolon, object param = null, string DBName = "")
        {
            string executed = string.Empty;
            try
            {
                using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString(DBName)))
                {
                   await mainDB.QueryMultipleAsync(multipleQueryWithSemicolon, param).ConfigureAwait(false);
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