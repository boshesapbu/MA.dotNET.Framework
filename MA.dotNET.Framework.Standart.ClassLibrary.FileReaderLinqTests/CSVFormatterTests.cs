using MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MA.dotNET.Framework.Standart.ClassLibrary.FileReaderLinqTests
{
    [TestClass]
    public class CSVFormatterTests
    {
        #region Constructors
        static CSVFormatterTests()
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
            Reader reader = new Reader(Path.Combine(LocalDirectory, "ExampleFile.csv"));
            LineDataReader lineDataReader = new LineDataReader(reader, Encoding.UTF8);
            CSVFormatter csvFormatter = new CSVFormatter(lineDataReader);
            int lastRowIndex = 0;
            
            csvFormatter.Read((rowIndex, columns) =>
                {
                    lastRowIndex = rowIndex;
                    switch (rowIndex)
                    {
                        case 0:
                            Assert.AreEqual(columns[0], "NAME");
                            Assert.AreEqual(columns[1], "SURNAME");
                            Assert.AreEqual(columns[2], "OLD");
                            break;
                        case 1:
                            Assert.AreEqual(columns[0], "MUHAMMED");
                            Assert.AreEqual(columns[1], "KANDEMIR");
                            Assert.AreEqual(columns[2], "23");
                            break;
                        case 2:
                            Assert.AreEqual(columns[0], "HAMZA");
                            Assert.AreEqual(columns[1], "KANDEMIR");
                            Assert.AreEqual(columns[2], "19");
                            break;
                        case 3:
                            Assert.AreEqual(columns[0], "MUHAMMED");
                            Assert.AreEqual(columns[1], "KANDEMIR");
                            Assert.AreEqual(columns[2], "2\"3");
                            break;
                        case 4:
                            Assert.AreEqual(columns[0], "\"HAMZA");
                            Assert.AreEqual(columns[1], "KAN\"DEMIR");
                            Assert.AreEqual(columns[2], "1\"\"9\"");
                            break;
                        case 5:
                            Assert.AreEqual(columns[0], "MUHAMMED");
                            Assert.AreEqual(columns[1], "KANDEMIR");
                            Assert.AreEqual(columns[2], "23");
                            break;
                        case 6:
                            Assert.AreEqual(columns[0], "HAMZA");
                            Assert.AreEqual(columns[1], "KAN\"DEMIR");
                            Assert.AreEqual(columns[2], "1\"\"9\"");
                            break;
                        default:
                            break;
                    }
                });

            Assert.AreEqual(lastRowIndex, 6);
        }
    }
}
