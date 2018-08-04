using MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using System.Text;

namespace MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinqTests
{
    [TestClass]
    public class LineDataReaderTests
    {
        #region Constructors
        static LineDataReaderTests()
        {
            LocalDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        #endregion

        #region Variables
        public static string LocalDirectory = null;
        #endregion

        [TestMethod]
        public void Read()
        {
            Reader reader = new Reader(Path.Combine(LocalDirectory, "ExampleFile.txt"));
            LineDataReader lineDataReader = new LineDataReader(reader, Encoding.UTF8);
            var lastRowIndex = 0;
            lineDataReader.Read((rowIndex, rowData) =>
            {
                lastRowIndex = rowIndex;

                switch (rowIndex)
                {
                    case 0:
                        Assert.AreEqual(rowData, "Line1");
                        break;
                    case 1:
                        Assert.AreEqual(rowData, "Line2");
                        break;
                    case 2:
                        Assert.AreEqual(rowData, "");
                        break;
                    case 3:
                        Assert.AreEqual(rowData, "Line3");
                        break;
                    default:
                        break;
                }
            });
            Assert.AreEqual(lastRowIndex, 3);
        }
    }
}
