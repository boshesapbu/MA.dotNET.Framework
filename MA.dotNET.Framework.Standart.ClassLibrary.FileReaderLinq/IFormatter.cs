using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinq
{
    public interface IFormatter<TReturnDataReader, TReturnFormatter>
    {
        #region Variables
        IDataReader<TReturnDataReader> DataReader { get; }
        #endregion

        #region Methods
        void Read(Action<int, TReturnFormatter> action);
        #endregion
    }
}
