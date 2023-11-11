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
        public bool BulkInsert<Tp, Te>(Tp tp, Te te, Action<Tp,Te> func)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    func(tp,te);
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

        //SQL COMMANDBUILDERS
        public void CommandBulkInsertBuilder(DataTable dataTable, Enum e)
        {
            //Initialize
            SqlBulkCopy sqlBulk = new SqlBulkCopy(_conString, SqlBulkCopyOptions.FireTriggers);
            DataTable dt = dataTable as DataTable;

            //Map columns based on Enum type
            foreach (DataColumn column in dt.Columns)
            {
                List<string> database = new List<string>(Enum.GetNames(e.GetType()));


                if (database.Contains(column.ColumnName))
                {
                    sqlBulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }
            }

            //Determine table and execute
            sqlBulk.DestinationTableName = $"dbo.Temp" + e.GetType().Name;
            sqlBulk.WriteToServer(dt);
        }
    }
}
