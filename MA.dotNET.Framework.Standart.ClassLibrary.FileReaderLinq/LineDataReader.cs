using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinq
{
    public class LineDataReader : IDataReader<string>
    {
        #region Constructors
        public LineDataReader(Reader reader, Encoding encoding)
        {
            this.Reader = reader;
            this.Encoding = encoding;
        }
        #endregion

        #region Variables
        public Reader Reader { get; private set; }
        public Encoding Encoding { get; set; }
        #endregion

        #region Methods
        private string read()
        {
            List<byte> bufferLine = new List<byte>();
            while (true)
            {
                var byteFromReader = this.Reader.ReadByte();
                if (byteFromReader == null)
                {
                    if (bufferLine.Count == 0)
                        return null;
                    break;
                }

                // Is enter
                if (byteFromReader.Value == 13 || byteFromReader.Value == 10)
                {
                    byteFromReader = this.Reader.ReadByte();
                    if (byteFromReader != null && byteFromReader.Value != 13 && byteFromReader.Value != 10)
                        this.Reader.Position--;
                    break;
                }

                bufferLine.Add(byteFromReader.Value);
            }

            if (bufferLine.Count == 0)
                return "";

            var bufferLineAsArray = bufferLine.ToArray();
            bufferLine.Clear();
            string result = this.Encoding.GetString(bufferLineAsArray);
            bufferLineAsArray = null;
            return result;
        }

        public void Read(Action<int, string> action)
        {
            int rowIndex = 0;
            while (true)
            {
                var rowData = this.read();
                if (rowData == null)
                    break;

                action(rowIndex, rowData);
                rowIndex++;
            }
        }
        #endregion
    }
}
