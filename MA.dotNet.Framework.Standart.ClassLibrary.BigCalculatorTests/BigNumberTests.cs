using Microsoft.VisualStudio.TestTools.UnitTesting;
using MA.dotNet.Framework.Standart.ClassLibrary.BigCalculator;
using System.Globalization;

namespace MA.dotNet.Framework.Standart.ClassLibrary.BigCalculatorTests
{
    [TestClass]
    public class BigNumberTests
    {
        #region Add
        [TestMethod]
        public void Example1_Add()
        {
            long dotNetValue = (long)123456 + (long)987564;
            var myLibValue = new BigNumber("123456") + new BigNumber("987564");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example2_Add()
        {
            long dotNetValue = (long)564897323 + (long)352341212;
            var myLibValue = new BigNumber("564897323") + new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example3_Add()
        {
            long dotNetValue = (long)0 + (long)352341212;
            var myLibValue = new BigNumber("0") + new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example4_Add()
        {
            long dotNetValue = (long)0 + (long)352341212;
            var myLibValue = new BigNumber("352341212") + new BigNumber("0");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }
        #endregion

        #region Subtract
        [TestMethod]
        public void Example1_Subtract()
        {
            long dotNetValue = (long)123456 - (long)987564;
            var myLibValue = new BigNumber("123456") - new BigNumber("987564");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example2_Subtract()
        {
            long dotNetValue = (long)564897323 - (long)352341212;
            var myLibValue = new BigNumber("564897323") - new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example3_Subtract()
        {
            long dotNetValue = (long)0 - (long)352341212;
            var myLibValue = new BigNumber("0") - new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example4_Subtract()
        {
            long dotNetValue = (long)352341212 - (long)0;
            var myLibValue = new BigNumber("352341212") - new BigNumber("0");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }
        #endregion

        #region Multiply
        [TestMethod]
        public void Example1_Multiply()
        {
            long dotNetValue = (long)123456 * (long)987564;
            var myLibValue = new BigNumber("123456") * new BigNumber("987564");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example2_Multiply()
        {
            long dotNetValue = (long)564897323 * (long)352341212;
            var myLibValue = new BigNumber("564897323") * new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example3_Multiply()
        {
            long dotNetValue = (long)0 * (long)352341212;
            var myLibValue = new BigNumber("0") * new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example4_Multiply()
        {
            long dotNetValue = (long)0 * (long)352341212;
            var myLibValue = new BigNumber("352341212") * new BigNumber("0");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }
        #endregion

        #region Divide
        [TestMethod]
        public void Example1_Divide()
        {
            long dotNetValue = (long)123456 / (long)987564;
            var myLibValue = new BigNumber("123456") / new BigNumber("987564");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example2_Divide()
        {
            long dotNetValue = (long)564897323 / (long)352341212;
            var myLibValue = new BigNumber("564897323") / new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example3_Divide()
        {
            long dotNetValue = (long)0 / (long)352341212;
            var myLibValue = new BigNumber("0") / new BigNumber("352341212");

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }
        #endregion
    }
}
