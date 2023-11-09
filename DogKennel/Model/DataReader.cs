using ExcelDataReader;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;

namespace DogKennel.Model
{
    public class DataReader
    {
        //Reads Excel file from a specific path
        public static bool ReadExcel(string _filepath, out DataTable? dt)
        {
            try
            {
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (FileStream stream = File.Open(_filepath, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        DataSet result = reader.AsDataSet();
                        dt = result.Tables[0];

                        //Remove first row (column names) to only handle raw data
                        dt.Rows.RemoveAt(0);
                        dt.Columns.RemoveAt(0);

                        //Throws exception if Excel-file has the wrong format (e.g. not named "Grunddata" according to the given customer specifications
                        if (dt.TableName != "Grunddata")
                        {
                            throw new Exception();
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                dt = null;
                return false;
            }
        }
    }
}