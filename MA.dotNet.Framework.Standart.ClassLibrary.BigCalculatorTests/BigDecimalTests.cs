using Microsoft.VisualStudio.TestTools.UnitTesting;
using MA.dotNet.Framework.Standart.ClassLibrary.BigCalculator;
using System.Globalization;

namespace MA.dotNet.Framework.Standart.ClassLibrary.BigCalculatorTests
{
    [TestClass]
    public class BigDecimalTests
    {
        #region Add
        [TestMethod]
        public void Example1_Add()
        {
            decimal dotNetValue = 123456.243m + 987564.764m;
            var myLibValue = new BigDecimal("123456.243", 3) + new BigDecimal("987564.764", 3);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example2_Add()
        {
            decimal dotNetValue = 564897323.67m + 352341212.99m;
            var myLibValue = new BigDecimal("564897323.67", 2) + new BigDecimal("352341212.99", 2);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example3_Add()
        {
            decimal dotNetValue = 0.4561m + 352341212.234m;
            var myLibValue = new BigDecimal("0.4561", 4) + new BigDecimal("352341212.234", 4);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example4_Add()
        {
            decimal dotNetValue = 0.123345m + 352341212.45231m;
            var myLibValue = new BigDecimal("352341212.45231", 6) + new BigDecimal("0.123345", 6);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }
        #endregion

        #region Subtract
        [TestMethod]
        public void Example1_Subtract()
        {
            decimal dotNetValue = 123456.243m - 987564.764m;
            var myLibValue = new BigDecimal("123456.243", 3) - new BigDecimal("987564.764", 3);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example2_Subtract()
        {
            decimal dotNetValue = 564897323.67m - 352341212.99m;
            var myLibValue = new BigDecimal("564897323.67", 2) - new BigDecimal("352341212.99", 2);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example3_Subtract()
        {
            decimal dotNetValue = 0.4561m - 352341212.234m;
            var myLibValue = new BigDecimal("0.4561", 4) - new BigDecimal("352341212.234", 4);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example4_Subtract()
        {
            decimal dotNetValue = 352341212.45231m - 0.123345m;
            var myLibValue = new BigDecimal("352341212.45231", 6) - new BigDecimal("0.123345", 6);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }
        #endregion

        #region Multiply
        [TestMethod]
        public void Example1_Multiply()
        {
            decimal dotNetValue = 123456.243m * 987564.764m;
            var myLibValue = new BigDecimal("123456.243", 6) * new BigDecimal("987564.764", 6);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example2_Multiply()
        {
            decimal dotNetValue = 564897323.67m * 352341212.99m;
            var myLibValue = new BigDecimal("564897323.67", 4) * new BigDecimal("352341212.99", 4);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example3_Multiply()
        {
            decimal dotNetValue = 0.4561m * 352341212.234m;
            var myLibValue = new BigDecimal("0.4561", 7) * new BigDecimal("352341212.234", 7);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }

        [TestMethod]
        public void Example4_Multiply()
        {
            decimal dotNetValue = 352341212.45231m * 0.123345m;
            var myLibValue = new BigDecimal("352341212.45231", 11) * new BigDecimal("0.123345", 11);

            Assert.AreEqual(dotNetValue.ToString(CultureInfo.InvariantCulture), myLibValue.ToString());
        }
        #endregion

        #region Divide
        [TestMethod]
        public void Example1_Divide()
        {
            decimal dotNetValue = 123456.243m / 987564.764m;
            var dotNetValueAsString = dotNetValue.ToString(CultureInfo.InvariantCulture);
            var dotNetValueLenghtAfterDot = dotNetValueAsString.Split('.')[1].Length + 1;

            var myLibValue = new BigDecimal("123456.243", dotNetValueLenghtAfterDot) / new BigDecimal("987564.764", dotNetValueLenghtAfterDot);

            Assert.AreEqual(dotNetValueAsString, myLibValue.Fixed(dotNetValueLenghtAfterDot - 1).ToString());
        }

        [TestMethod]
        public void Example2_Divide()
        {
            decimal dotNetValue = 564897323.67m / 352341212.99m;
            var dotNetValueAsString = dotNetValue.ToString(CultureInfo.InvariantCulture);
            var dotNetValueLenghtAfterDot = dotNetValueAsString.Split('.')[1].Length + 1;

            var myLibValue = new BigDecimal("564897323.67", dotNetValueLenghtAfterDot) / new BigDecimal("352341212.99", dotNetValueLenghtAfterDot);

            Assert.AreEqual(dotNetValueAsString, myLibValue.Fixed(dotNetValueLenghtAfterDot - 1).ToString());
        }

        [TestMethod]
        public void Example3_Divide()
        {
            decimal dotNetValue = 0.4561m / 352341212.234m;
            var dotNetValueAsString = dotNetValue.ToString(CultureInfo.InvariantCulture);
            var dotNetValueLenghtAfterDot = dotNetValueAsString.Split('.')[1].Length + 1;

            var myLibValue = new BigDecimal("0.4561", dotNetValueLenghtAfterDot) / new BigDecimal("352341212.234", dotNetValueLenghtAfterDot);

            Assert.AreEqual(dotNetValueAsString, myLibValue.Fixed(dotNetValueLenghtAfterDot - 1).ToString());
        }

        [TestMethod]
        public void Example4_Divide()
        {
            decimal dotNetValue = 352341212.45231m / 0.123345m;
            var dotNetValueAsString = dotNetValue.ToString(CultureInfo.InvariantCulture);
            var dotNetValueLenghtAfterDot = dotNetValueAsString.Split('.')[1].Length + 1;

            var myLibValue = new BigDecimal("352341212.45231", dotNetValueLenghtAfterDot) / new BigDecimal("0.123345", dotNetValueLenghtAfterDot);

            Assert.AreEqual(dotNetValueAsString, myLibValue.Fixed(dotNetValueLenghtAfterDot - 1).ToString());
        }
        #endregion
    }
}
