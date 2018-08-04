using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinq
{
    public interface IDataReader<TReturn>
    {
        #region Variables
        Reader Reader { get; }
        #endregion

        #region Methods
        void Read(Action<int, TReturn> action);
        #endregion
    }
}
