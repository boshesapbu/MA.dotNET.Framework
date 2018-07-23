using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MA.dotNET.Framework.Standart.ClassLibrary.Logger
{
    public class LogWriter : IDisposable
    {
        #region Constructs
        /// <summary>
        /// Create a new LogWriter
        /// </summary>
        /// <param name="columns">CSV Columns</param>
        /// <param name="fileSaveDirectory">Save directory</param>
        /// <param name="fileStartName">
        /// File start name
        /// <para></para>
        /// Example : "Log_", "ApiGatewayLog_", "ServiceALog_"
        /// </param>
        /// <param name="fileDateFormat">
        /// File end name
        /// <para></para>
        /// Example : "ddMMyyyy" => this is for daily log, "MMyyyy" => this is for monthly log, ....
        /// </param>
        public LogWriter(string[] columns, string fileSaveDirectory, string fileStartName, string fileDateFormat)
        {
            this.Columns = columns;
            this.FileSaveDirectory = fileSaveDirectory;
            this.FileStartName = fileStartName;
            this.FileDateFormat = fileDateFormat;

            #region Fill Private Variables
            var fileExists = File.Exists(this.FilePath);

            this._fileStream = new FileStream(this.FilePath, FileMode.Append, FileAccess.Write);
            this._streamWriter = new StreamWriter(this._fileStream);

            // If file not exists then write columns name to first row
            if (fileExists == false)
                this._streamWriter.WriteLine(ColumnsStr);
            this._lastWroteFileEndName = DateTime.Now.ToString(FileDateFormat);
            #endregion
        }
        #endregion

        #region Variables
        public readonly string SplitChar = ",";
        string[] columns = null;
        /// <summary>
        /// CSV Columns
        /// </summary>
        public string[] Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                ColumnsStr = joinDataByCSV(columns);
            }
        }
        /// <summary>
        /// Get Columns with join
        /// </summary>
        public string ColumnsStr { get; private set; }

        /// <summary>
        /// File end name
        /// <para></para>
        /// Example : "ddMMyyyy" => this is for daily log, "MMyyyy" => this is for monthly log, ....
        /// </summary>
        public string FileDateFormat { get; private set; }
        /// <summary>
        /// File start name
        /// <para></para>
        /// Example : "Log_", "ApiGatewayLog_", "ServiceALog_"
        /// </summary>
        public string FileStartName { get; private set; }
        /// <summary>
        /// Save directory
        /// </summary>
        public string FileSaveDirectory { get; private set; }

        /// <summary>
        /// This is net file name
        /// <para></para>
        /// FileSaveDirectory + FileStartName + FileDateFormat + ".csv"
        /// </summary>
        public string FilePath
        {
            get
            {
                return Path.Combine(FileSaveDirectory, FileStartName + DateTime.Now.ToString(FileDateFormat) + ".csv");
            }
        }

        private FileStream _fileStream;
        private StreamWriter _streamWriter;
        private string _lastWroteFileEndName = "";
        #endregion

        #region Methods
        /// <summary>
        /// Add new log to file
        /// </summary>
        /// <param name="row"></param>
        public void Add(params string[] row)
        {
            // If needed new file
            if (this._lastWroteFileEndName != DateTime.Now.ToString(FileDateFormat))
            {
                // Firstly dispose
                this._streamWriter.Dispose();
                this._fileStream.Dispose();

                var fileExists = File.Exists(this.FilePath);

                this._fileStream = new FileStream(this.FilePath, FileMode.Append, FileAccess.Write);
                this._streamWriter = new StreamWriter(this._fileStream);

                // If file not exists then write columns name to first row
                if (fileExists == false)
                    this._streamWriter.WriteLine(ColumnsStr);
                this._lastWroteFileEndName = DateTime.Now.ToString(FileDateFormat);
            }

            // Write new row to CSV File
            this._streamWriter.WriteLine(joinDataByCSV(row));
            this._streamWriter.Flush();
        }

        private string joinDataByCSV(IEnumerable<string> datas)
        {
            return string.Join(SplitChar, datas.Select(o => "\"" + o.Replace("\"", "\"\"").Replace("\n", " ").Replace("\r", " ") + "\"").ToArray());
        }

        public void Dispose()
        {
            this._streamWriter.Dispose();
            this._fileStream.Dispose();
        }
        #endregion
    }
}
