using System;
using System.IO;

namespace MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinq
{
    public class Reader : IDisposable
    {
        #region Constructors
        public Reader(string file)
        {
            this._Stream = new FileStream(file, FileMode.Open, FileAccess.Read);
        }
        #endregion

        #region Variables
        public string FilePath { get; set; }
        private FileStream _Stream = null;
        public long Position
        {
            get
            {
                return this._Stream.Position;
            }
            set
            {
                this._Stream.Position = value;
            }
        }

        private byte[] _readByteArray = new byte[1];
        #endregion

        #region Methods
        public byte? ReadByte()
        {
            int count = _Stream.Read(this._readByteArray, 0, 1);
            if (count == 0)
                return null;
            
            return this._readByteArray[0];
        }

        public void Dispose()
        {
            this._Stream.Dispose();
        }
        #endregion
    }
}
