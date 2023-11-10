using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Documents;
using System.Collections.Generic;

namespace DogKennel.Model
{
    public class DataAccess
    {
        public string _conString = ConfigurationManager.ConnectionStrings["DatabaseServerInstance"].ConnectionString;
        private SqlCommand _command = new SqlCommand();

        //SQL PUSHES
        public bool BulkInsert<Tp>(Tp tp, Action<Tp> func)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    func(tp);
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        //SQL COMMANDS
        public void CommandBulkInsert<Tp>(Tp dataTable)
        {
            DataTable dt = dataTable as DataTable;

            SqlBulkCopy sqlBulk = new SqlBulkCopy(_conString);
            sqlBulk.DestinationTableName = "dbo.Dogs";

            foreach (DataColumn column in dt.Columns)
            {
                List<string> database = new List<string>(Enum.GetNames(typeof(Dogs)));


                if (database.Contains(column.ColumnName))
                {
                    sqlBulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }
            }
            sqlBulk.WriteToServer(dt);
        }
    }
}
