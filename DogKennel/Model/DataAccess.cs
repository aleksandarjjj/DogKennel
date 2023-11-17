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
        public bool CommandSelect(Enum e, out DataTable? dataTable, string pedigreeID)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    DataTable loadedDataTable = new DataTable();

                    //Determine table from enum type and execute
                    _command = new SqlCommand($"SELECT * FROM dbo." + e.GetType().Name + "WHERE PedigreeID = @PedigreeID}", _con);
                    _command.Parameters.Add("@PedigreeID", SqlDbType.NVarChar).Value = pedigreeID;
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
        public bool CommandInsert(string[] strings)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    //Build connectionstring and parameters manually by loops
                    _command = new SqlCommand();
                    string commandName = "EXEC spInsert_Into_TblDogs_TblDogHealth_TblDogPedigree ";

                    for (int i = 0; i < strings.Length; i++)
                    {
                        if (strings[i] == "" || strings[i] == null)
                        {
                            _command.Parameters.Add($"@{i + 1}", SqlDbType.NVarChar).Value = System.DBNull.Value;
                        }
                        else
                        {
                            _command.Parameters.Add($"@{i + 1}", SqlDbType.NVarChar).Value = strings[i];
                        }

                        commandName = string.Concat(commandName, $"@{i + 1}, ");
                    }
                    
                    //Fit command name
                    commandName = commandName.Remove(commandName.Length - 2);

                    //Execution
                    _command.CommandText = commandName;
                    _command.Connection = _con;
                    _con.Open();
                    _command.ExecuteNonQuery();
                    return true;
                }

                catch (Exception)
                {
                    return false;
                }
            }
        }
        public bool CommandDelete(string? pedigreeID)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    //Determine table from enum type, provide parameter and execute
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
        public bool CommandTestConnection()
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                //Timeout time
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
        public bool CommandBulkInsertAllBuilder(Enum e, DataTable dataTable)
        {
            using (SqlConnection _con = new SqlConnection(_conString))
            {
                try
                {
                    _con.Open();
                    SqlBulkCopy sqlBulk = new SqlBulkCopy(_conString, SqlBulkCopyOptions.FireTriggers);

                    //Map SQL columns based on enum types specifically for the Excel columns has to be done
                    //when only selecting a number of columns from the available datatable

                    //List with desired columns initialized from provided enum argument
                    List<string> columnTypes = new List<string>(Enum.GetNames(e.GetType()));

                    //Loop over datacolumns in provided datatable
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        //Assing column to SqlBulkCopy if it matches the desired enum column
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

                    //Determine table from enum type and execute
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

                    //Determine table from enum type and executre
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

        //HELPER METHODS
        //Construct Dog object from record
        public static Dog DogConstructor(object[] convertedRow)
        {
            Dog dog = new Dog();

            //Get properties from object type
            PropertyInfo[] properties = dog.GetType().GetProperties();

            //Loop through properties and convert (all properties defined as string)
            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                //Handle NULL values from database
                if (convertedRow[i].Equals(System.DBNull.Value))
                {
                    convertedRow[i] = String.Empty;
                }

                //Assign property
                property.SetValue(dog, convertedRow[i]);
                i++;
            }

            return dog;
        }
        public static Health HealthConstructor(object[] convertedRow)
        {
            Health health = new Health();

            //Get properties from object type
            PropertyInfo[] properties = health.GetType().GetProperties();

            //Loop through properties and convert (all properties defined as string)
            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                //Handle NULL values from database
                if (convertedRow[i].Equals(System.DBNull.Value))
                {
                    convertedRow[i] = String.Empty;
                }

                //Assign property
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
