using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;

namespace DogKennel.Model
{
    public class DataAccess
    {
        private static string _conString = ConfigurationManager.ConnectionStrings["DatabaseServerInstance"].ConnectionString;
        private SqlCommand _command = new SqlCommand();
        
        public SqlConnection Con { get;} = new SqlConnection(_conString);

        //SQL PUSHES
        public bool BulkInsert<TParam, TResult>(TParam parameter, Func<TParam, TResult> func)
        {
            using (Con)
            {
                try
                {
                    Con.Open();
                    func(parameter);

                    _command.ExecuteNonQuery();
                    _command = new SqlCommand();

                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        //SQL COMMANDS
        public DataTable CommandBulkInsert(DataTable dt)
        {
            return new DataTable();
        }
    }
}
