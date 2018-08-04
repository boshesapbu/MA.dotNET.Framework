using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinq
{
    public class CSVFormatter : IFormatter<string, string[]>
    {
        #region Constructors
        public CSVFormatter(LineDataReader lineDataReader)
        {
            this.DataReader = lineDataReader;
            this.ClearEmptyRows = true;
        }
        #endregion

        #region Variables
        public IDataReader<string> DataReader { get; private set; }
        public bool ClearEmptyRows { get; set; }
        #endregion

        #region Methods

        public void Read(Action<int, string[]> action)
        {
            this.DataReader.Read((rowIndex, rowData) =>
            {
                if (this.ClearEmptyRows == true && string.IsNullOrEmpty(rowData))
                    return;

                List<string> columns = new List<string>();
                string columnData = "";
                bool firstCharIsDoubleQuotes = false;

                for (int i = 0; i < rowData.Length; i++)
                {
                    var charData = rowData[i];
                    if (firstCharIsDoubleQuotes == false && (charData == ';' || charData == ','))
                    {
                        columns.Add(columnData);
                        columnData = "";
                    }
                    else if (columnData.Length == 0 && charData == '\"')
                        firstCharIsDoubleQuotes = true;
                    else if (firstCharIsDoubleQuotes == true && charData == '\"')
                        firstCharIsDoubleQuotes = false;
                    else
                    {
                        if (charData == '\\')
                        {
                            if (firstCharIsDoubleQuotes == true)
                            {
                                i++;
                                if (i < rowData.Length)
                                    charData = rowData[i];
                                else
                                    break;
                            }
                        }

                        columnData += charData;
                    }
                }
                if (string.IsNullOrEmpty(columnData) == false)
                    columns.Add(columnData);
                columnData = null;

                var columnsAsArray = columns.ToArray();
                columns.Clear();
                action(rowIndex, columnsAsArray);
                columnsAsArray = null;
            });
        }
        #endregion
    }
}
