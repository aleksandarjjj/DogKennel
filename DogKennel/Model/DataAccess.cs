using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace DogKennel.Model
{
    public class DataAccess
    {
        public string _conString = ConfigurationManager.ConnectionStrings["DatabaseServerInstance"].ConnectionString;
        private SqlCommand _command = new SqlCommand();

        //SQL SPECIFIC COMMANDS
        public bool CommandTestConnection()
        {
            using (SqlConnection _con = new SqlConnection(_conString))
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

        //SQL COMMANDBUILDERS
        public bool CommandBulkInsertBuilder(DataTable dataTable, Enum e)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();

                    //Initialize
                    SqlBulkCopy sqlBulk = new SqlBulkCopy(_conString, SqlBulkCopyOptions.FireTriggers);

                    //Map columns based on Enum type
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        List<string> columnTypes = new List<string>(Enum.GetNames(e.GetType()));


                        if (columnTypes.Contains(column.ColumnName))
                        {
                            sqlBulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                    }

                    //Determine table and execute
                    sqlBulk.DestinationTableName = $"dbo.Temp" + e.GetType().Name;
                    sqlBulk.WriteToServer(dataTable);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public bool CommandSelectAllBuilder(Enum e, out DataTable? dataTable)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    DataTable loadedDataTable = new DataTable();

                    _command = new SqlCommand($"SELECT * FROM dbo." + e.GetType().Name, _con);
                    loadedDataTable.Load(_command.ExecuteReader());

                    dataTable = loadedDataTable;
                    return true;
                }
                catch (Exception)
                {
                    dataTable = null;
                    return false;
                }
            }
        }
        public bool CommandTruncateBuilder(Enum e)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    _command = new SqlCommand($"DELETE FROM " + e.GetType().Name, _con);
                    _command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public bool CommandDelete(string pedigreeID)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    _command = new SqlCommand($"EXEC spDelete_Dog @PedigreeID", _con);
                    _command.Parameters.Add("@PedigreeID", SqlDbType.NVarChar).Value = pedigreeID;
                    _command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        //HELPER METHODS
        public static Dog DogConstructor(object[] convertedRow)
        {
            Dog dog = new Dog();
            PropertyInfo[] properties = dog.GetType().GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                if (convertedRow[i].Equals(System.DBNull.Value))
                {
                    convertedRow[i] = String.Empty;
                }

                property.SetValue(dog, convertedRow[i]);
                i++;
            }

            return dog;
        }
        public static Health HealthConstructor(object[] convertedRow)
        {
            Health health = new Health();
            PropertyInfo[] properties = health.GetType().GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                if (convertedRow[i].Equals(System.DBNull.Value))
                {
                    convertedRow[i] = String.Empty;
                }

                property.SetValue(health, convertedRow[i]);
                i++;
            }

            return health;
        }
        public static Pedigree PedigreeConstructor(object[] convertedRow)
        {
            Pedigree pedigree = new Pedigree();
            PropertyInfo[] properties = pedigree.GetType().GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                if (convertedRow[i].Equals(System.DBNull.Value))
                {
                    convertedRow[i] = String.Empty;
                }

                property.SetValue(pedigree, convertedRow[i]);
                i++;
            }

            return pedigree;
        }
    }
}
