using MA.dotNET.Framework.Standart.ClassLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace MA.dotNET.Framework.Standart.ClassLibrary.LoggerTests
{
    [TestClass]
    public class LogWriterTests
    {
        #region Constructors
        static LogWriterTests()
        {
            var localDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Firstly, we have to remove the log file
            // Because, When we not remove the log file, to be row count bigger 2
            // When executing add, append new rows and not clear before rows
            foreach (var file in Directory.GetFiles(localDirectory))
            {
                if (Path.GetFileName(file).StartsWith("Log"))
                    File.Delete(file);
            }

            Log = new LogWriter(new string[]
            {
                "NAME",
                "SURNAME",
                "OLD"
            }, localDirectory, "Log", "ddMMyyyy" /*This is for daily log. If you want montly log then use "MMyyyy".*/);
        }
        #endregion

        #region Variables
        public static LogWriter Log = null;
        #endregion

        [TestMethod]
        public void Write()
        {
            // You have to using with lock for multi thread!
            lock (Log)
            {
                Log.Add("MUHAMMED", "KANDEMIR", "23");
                Log.Add("HAMZA", "KANDEMIR", "19");
            }
            // If you don't dispose then not read log file on other processes
            Log.Dispose();

            var logText = File.ReadAllText(Log.FilePath);
            var controlText =
                @"""NAME"",""SURNAME"",""OLD""" + Environment.NewLine +
                @"""MUHAMMED"",""KANDEMIR"",""23""" + Environment.NewLine +
                @"""HAMZA"",""KANDEMIR"",""19""" + Environment.NewLine;
            Assert.AreEqual(logText, controlText, message: "Log file wrong text!");
        }
    }
}
