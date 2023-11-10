using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogKennel.Model
{
    public static class DataAccessExtensions
    {
        //Test connection by pinging SQL-server
        public static bool TestConnection(this DataAccess da)
        {
            using (SqlConnection _con = new SqlConnection(da._conString))
            {
                int timeout = 2000;
                Task task = Task.Run(() =>
                {
                    try { _con.Open(); }
                    catch (SqlException) { }
                });

                if (task.Wait(timeout))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
