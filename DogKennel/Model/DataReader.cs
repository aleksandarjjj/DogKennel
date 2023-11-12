using ExcelDataReader;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Documents;

namespace DogKennel.Model
{
    public class DataReader
    {
        //Reads and converts Excel file from a specific path
        public static bool ReadExcel(string _filepath, out DataTable dt)
        {
            try
            {
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (FileStream stream = File.Open(_filepath, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        DataSet result = reader.AsDataSet();
                        DataTable dtRaw = result.Tables[0];

                        //Throws exception if Excel-file has the wrong format (e.g. not named "Grunddata" according to the given customer specifications
                        if (dtRaw.TableName != "Grunddata")
                        {
                            throw new Exception();
                        }

                        //Trim
                        dtRaw.Rows.RemoveAt(0);
                        dtRaw.Columns.RemoveAt(0);

                        //Clone to ensure type safety and modify
                        dt = dtRaw.Clone();

                        //Fit columns
                        int i = 0;
                        foreach(DataColumn column in  dtRaw.Columns)
                        {
                            dt.Columns[i].DataType = typeof(string);
                            dt.Columns[i].ColumnName = Enum.GetName(typeof(TblColumnsExcel), i + 1);
                            i++;
                        }
                        
                        //Fit rows
                        foreach(DataRow row in dtRaw.Rows)
                        {
                            dt.Rows.Add(row.ItemArray);
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