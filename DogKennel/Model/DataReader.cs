using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DogKennel.Model
{
    public class DataReader
    {
        //Read and converts Excel file from a specific path
        public static bool ReadExcel(string _filepath, out DataTable? dt)
        {
            //Try-catch for handling selection of wrong file type (throws exception)
            try
            {
                //Opens filestream to read file
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (FileStream stream = File.Open(_filepath, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        //Read file as dataset and assign to datatable
                        DataSet result = reader.AsDataSet();
                        DataTable dtRaw = result.Tables[0];

                        //Throws exception if Excel-file has the wrong format (e.g. not named "Grunddata" according to the given customer specifications
                        if (dtRaw.TableName != "Grunddata")
                        {
                            throw new Exception();
                        }

                        //Trim first column and first row
                        dtRaw.Rows.RemoveAt(0);
                        dtRaw.Columns.RemoveAt(0);

                        //Clone datatable (size) to make modifying possible
                        dt = dtRaw.Clone();

                        //Fit columns
                        int i = 0;
                        foreach (DataColumn column in dtRaw.Columns)
                        {
                            //Assign type of column based on extracted datatable from file
                            dt.Columns[i].DataType = typeof(string);

                            //Assign column name based on extracted datatable from file and Enum
                            //for choosing specific column
                            dt.Columns[i].ColumnName = Enum.GetName(typeof(TblColumnsExcel), i + 1);
                            i++;
                        }

                        //Copy over rows
                        foreach (DataRow row in dtRaw.Rows)
                        {
                            //Trim for whitespace by extracting itemArray, trimming the extracted array and reassigning
                            var cellList = row.ItemArray.ToList();
                            row.ItemArray = cellList.Select(x => x.ToString().Trim()).ToArray();

                            //Add row
                            dt.Rows.Add(row.ItemArray);
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                //Datatable set to null to ensure out parameter is assigned (can be used later for conditional handling)
                dt = null;
                return false;
            }
        }
    }
}