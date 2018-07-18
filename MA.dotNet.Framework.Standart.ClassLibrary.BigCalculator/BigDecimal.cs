using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MA.dotNet.Framework.Standart.ClassLibrary.BigCalculator
{
    public class BigDecimal
    {
        #region Constructs

        public BigDecimal(string value, BigNumber precisionNormal)
        {
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;

            var dotIndex = value.IndexOf('.');
            if (dotIndex >= 0)
            {
                this._isNeg = value.StartsWith("-");
                var left = this._isNeg ? value.Substring(1, dotIndex - 1) : value.Substring(0, dotIndex);
                var right = value.Substring(dotIndex + 1);

                var diffRightAndPrecision = precisionNormal - right.Length;
                for (int i = 0; i < diffRightAndPrecision; i++)
                    right += '0';
                if (right.Length > precisionNormal)
                    right = right.Substring(0, (int)precisionNormal);

                this._integerLeft = left == "" ? "0" : left;
                this._integerRight = right == "" ? "0" : right;
            }
            else
            {
                this._integerLeft = value;
                this._integerRight = 0;
            }
        }

        public BigDecimal(BigInteger value, BigNumber precisionNormal)
        {
            this._isNeg = value < 0;
            this._integerLeft = value * (this._isNeg ? -1 : 1);
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(byte value, BigNumber precisionNormal)
        {
            this._integerLeft = value;
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(sbyte value, BigNumber precisionNormal)
        {
            this._isNeg = value < 0;
            this._integerLeft = value * (this._isNeg ? -1 : 1);
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(short value, BigNumber precisionNormal)
        {
            this._isNeg = value < 0;
            this._integerLeft = value * (this._isNeg ? -1 : 1);
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(ushort value, BigNumber precisionNormal)
        {
            this._integerLeft = value;
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(int value, BigNumber precisionNormal)
        {
            this._isNeg = value < 0;
            this._integerLeft = value * (this._isNeg ? -1 : 1);
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(uint value, BigNumber precisionNormal)
        {
            this._integerLeft = value;
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(long value, BigNumber precisionNormal)
        {
            this._isNeg = value < 0;
            this._integerLeft = value * (this._isNeg ? -1 : 1);
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(ulong value, BigNumber precisionNormal)
        {
            this._integerLeft = value;
            this._integerRight = 0;
            this._precisionOnlyNormal = BigNumber.Ten.Pow(precisionNormal);
            this._precisionLength = precisionNormal;
        }

        public BigDecimal(float value, BigNumber precision) : this(value.ToString(ConvertCultureInfo), precision) { }
        public BigDecimal(double value, BigNumber precision) : this(value.ToString(ConvertCultureInfo), precision) { }
        public BigDecimal(decimal value, BigNumber precision) : this(value.ToString(ConvertCultureInfo), precision) { }

        #endregion

        #region Variables

        internal static System.Globalization.CultureInfo ConvertCultureInfo = System.Globalization.CultureInfo.InvariantCulture;

        bool _isNeg = false;

        BigNumber _integerLeft;
        public BigNumber IntegerLeft
        {
            get
            {
                return _integerLeft;
            }
        }

        BigNumber _integerRight;
        public BigNumber IntegerRight
        {
            get
            {
                return _integerRight;
            }
        }

        BigNumber _precisionOnlyNormal = 0;
        public BigNumber PrecisionOnlyNormal
        {
            get
            {
                return _precisionOnlyNormal;
            }
        }

        BigNumber _precisionLength = 0;
        public BigNumber PrecisionLength
        {
            get
            {
                return _precisionLength;
            }
        }

        public const int StdProcessPrecision = 1000;

        #endregion

        #region Methods

        public BigDecimal Pow(BigNumber pow)
        {
            var result = this;
            for (int i = 1; i < pow; i++)
                result *= this;
            return result;
        }

        public BigDecimal Fixed(BigNumber precision)
        {
            var left = this;

            var netLeft_Right = left._integerRight;
            var netLeft_Right_String = netLeft_Right.ToString();
            if (netLeft_Right_String.Length < left.PrecisionLength)
                netLeft_Right_String = "".PadLeft((int)(left.PrecisionLength - netLeft_Right_String.Length), '0') + netLeft_Right_String;

            var intPrecision = (int)precision;
            if (intPrecision >= netLeft_Right_String.Length)
                return left;

            var plus = false;
            switch (netLeft_Right_String[intPrecision])
            {
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    plus = true;
                    break;
                default:
                    break;
            }

            var netLeft = (left._integerLeft * left.PrecisionOnlyNormal) + left._integerRight;
            if (plus)
                netLeft += BigNumber.Ten.Pow(left.PrecisionLength - precision);

            if (left._isNeg == true)
                netLeft *= -1;

            var net = netLeft;
            var net_isNeg = net < 0;
            if (net_isNeg == true)
                net *= -1;

            var net_string = net.ToString();
            if (net_string.Length < left.PrecisionLength + 1)
                net_string = "0" + "".PadLeft((int)(left.PrecisionLength - net_string.Length), '0') + net_string;

            var resultRight = net_string.Substring((int)(net_string.Length - left.PrecisionLength));
            var resultLeft = net_string.Substring(0, (int)(net_string.Length - resultRight.Length));
            return new BigDecimal(
                (net_isNeg ? "-" : "") + resultLeft + "." + resultRight
                , precision);
        }

        #region Operator Override

        #region Math

        public static BigDecimal operator +(BigDecimal value1, BigDecimal value2)
        {
            var left = value1;
            var right = value2;

            if (left.PrecisionLength > right.PrecisionLength)
                right = right.NewPrecision(left.PrecisionLength);
            else if (left.PrecisionLength < right.PrecisionLength)
                left = left.NewPrecision(right.PrecisionLength);

            var netLeft = (left._integerLeft * left.PrecisionOnlyNormal) + left._integerRight;
            var netRight = (right._integerLeft * right.PrecisionOnlyNormal) + right._integerRight;

            if (left._isNeg == true)
                netLeft *= -1;
            if (right._isNeg == true)
                netRight *= -1;

            var net = netLeft + netRight;
            var net_isNeg = net < 0;
            if (net_isNeg == true)
                net *= -1;

            var net_string = net.ToString();
            if (net_string.Length < left.PrecisionLength + 1)
                net_string = "0" + "".PadLeft((int)(left.PrecisionLength - net_string.Length), '0') + net_string;

            var resultRight = net_string.Substring((int)(net_string.Length - left.PrecisionLength));
            var resultLeft = net_string.Substring(0, (int)(net_string.Length - resultRight.Length));
            return new BigDecimal(
                (net_isNeg ? "-" : "") + resultLeft + "." + resultRight
                , left.PrecisionLength);
        }

        public static BigDecimal operator ++(BigDecimal value)
        {
            return value + BigDecimal.Cast(1, 0);
        }

        public static BigDecimal operator -(BigDecimal value1, BigDecimal value2)
        {
            var left = value1;
            var right = value2;

            if (left.PrecisionLength > right.PrecisionLength)
                right = right.NewPrecision(left.PrecisionLength);
            else if (left.PrecisionLength < right.PrecisionLength)
                left = left.NewPrecision(right.PrecisionLength);

            var netLeft = (left._integerLeft * left.PrecisionOnlyNormal) + left._integerRight;
            var netRight = (right._integerLeft * right.PrecisionOnlyNormal) + right._integerRight;

            if (left._isNeg == true)
                netLeft *= -1;
            if (right._isNeg == true)
                netRight *= -1;

            var net = netLeft - netRight;
            var net_isNeg = net < 0;
            if (net_isNeg == true)
                net *= -1;

            var net_string = net.ToString();
            if (net_string.Length < left.PrecisionLength + 1)
                net_string = "0" + "".PadLeft((int)(left.PrecisionLength - net_string.Length), '0') + net_string;

            var resultRight = net_string.Substring((int)(net_string.Length - left.PrecisionLength));
            var resultLeft = net_string.Substring(0, (int)(net_string.Length - resultRight.Length));
            return new BigDecimal(
                (net_isNeg ? "-" : "") + resultLeft + "." + resultRight
                , left.PrecisionLength);
        }

        public static BigDecimal operator --(BigDecimal value)
        {
            return value - BigDecimal.Cast(1, 0);
        }

        public static BigDecimal operator *(BigDecimal value1, BigDecimal value2)
        {
            var left = value1;
            var right = value2;

            if (left.PrecisionLength > right.PrecisionLength)
                right = right.NewPrecision(left.PrecisionLength);
            else if (left.PrecisionLength < right.PrecisionLength)
                left = left.NewPrecision(right.PrecisionLength);

            var netLeft = (left._integerLeft * left.PrecisionOnlyNormal) + left._integerRight;
            var netRight = (right._integerLeft * right.PrecisionOnlyNormal) + right._integerRight;

            if (left._isNeg == true)
                netLeft *= -1;
            if (right._isNeg == true)
                netRight *= -1;

            var net = netLeft * netRight;
            var net_isNeg = net < 0;
            if (net_isNeg == true)
                net *= -1;

            var net_string = net.ToString();
            if (net_string.Length < (left.PrecisionLength * 2) + 1)
                net_string = "0" + "".PadLeft((int)((left.PrecisionLength * 2) - net_string.Length), '0') + net_string;

            var resultRight = net_string.Substring((int)(net_string.Length - (left.PrecisionLength * 2)));
            var resultLeft = net_string.Substring(0, (int)(net_string.Length - resultRight.Length));
            return new BigDecimal(
                (net_isNeg ? "-" : "") + resultLeft + "." + resultRight
                , left.PrecisionLength);
        }

        public static BigDecimal operator /(BigDecimal value1, BigDecimal value2)
        {
            var left = value1;
            var right = value2;

            if (left.PrecisionLength > right.PrecisionLength)
                right = right.NewPrecision(left.PrecisionLength);
            else if (left.PrecisionLength < right.PrecisionLength)
                left = left.NewPrecision(right.PrecisionLength);

            var netLeft = (left._integerLeft * left.PrecisionOnlyNormal) + left._integerRight;
            var netRight = (right._integerLeft * right.PrecisionOnlyNormal) + right._integerRight;
            var netLeft_string = netLeft.ToString();
            var netRight_string = netRight.ToString();

            var pow = left.PrecisionLength + StdProcessPrecision + (netLeft_string.Length < netRight_string.Length ? netRight_string.Length - netLeft_string.Length : 0);
            netLeft *= BigNumber.Ten.Pow(pow);

            if (left._isNeg == true)
                netLeft *= -1;
            if (right._isNeg == true)
                netRight *= -1;

            var net = netLeft / netRight;
            var net_isNeg = net < 0;
            if (net_isNeg == true)
                net *= -1;

            var net_string = net.ToString();
            if (net_string.Length < (pow) + 1)
                net_string = "0" + "".PadLeft((int)((pow) - net_string.Length), '0') + net_string;

            var resultRight = net_string.Substring((int)(net_string.Length - (pow)));
            var resultLeft = net_string.Substring(0, (int)(net_string.Length - resultRight.Length));
            return new BigDecimal(
                (net_isNeg ? "-" : "") + resultLeft + "." + resultRight
                , left.PrecisionLength);
        }

        #endregion

        #region IF

        public static bool operator >(BigDecimal value1, BigDecimal value2)
        {
            var value1_netLeft = value1._integerLeft * (value1._isNeg ? -1 : 1);
            var value2_netLeft = value2._integerLeft * (value2._isNeg ? -1 : 1);
            return value1_netLeft > value2_netLeft || (value1_netLeft == value2_netLeft && value1._integerRight > value2._integerRight);
        }

        public static bool operator >=(BigDecimal value1, BigDecimal value2)
        {
            var value1_netLeft = value1._integerLeft * (value1._isNeg ? -1 : 1);
            var value2_netLeft = value2._integerLeft * (value2._isNeg ? -1 : 1);
            return value1_netLeft <= value2_netLeft || (value1_netLeft == value2_netLeft && value1._integerRight <= value2._integerRight);
        }

        public static bool operator <(BigDecimal value1, BigDecimal value2)
        {
            var value1_netLeft = value1._integerLeft * (value1._isNeg ? -1 : 1);
            var value2_netLeft = value2._integerLeft * (value2._isNeg ? -1 : 1);
            return value1_netLeft < value2_netLeft || (value1_netLeft == value2_netLeft && value1._integerRight < value2._integerRight);
        }

        public static bool operator <=(BigDecimal value1, BigDecimal value2)
        {
            var value1_netLeft = value1._integerLeft * (value1._isNeg ? -1 : 1);
            var value2_netLeft = value2._integerLeft * (value2._isNeg ? -1 : 1);
            return value1_netLeft <= value2_netLeft || (value1_netLeft == value2_netLeft && value1._integerRight <= value2._integerRight);
        }

        public static bool operator !=(BigDecimal value1, BigDecimal value2)
        {
            return value1._integerLeft != value2._integerLeft || value1._integerRight != value2._integerRight || value1._isNeg != value2._isNeg;
        }

        public static bool operator ==(BigDecimal value1, BigDecimal value2)
        {
            return value1._integerLeft == value2._integerLeft && value1._integerRight == value2._integerRight && value1._isNeg == value2._isNeg;
        }

        public override bool Equals(object obj)
        {
            return this == (BigDecimal)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #endregion

        #region Cast

        public BigDecimal NewPrecision(BigNumber precision)
        {
            return new BigDecimal(this.ToString(), precision);
        }

        public static BigDecimal Cast(BigInteger value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(string value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(byte value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(sbyte value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(short value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(ushort value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(int value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(uint value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(long value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(ulong value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(float value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(double value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        public static BigDecimal Cast(decimal value, BigNumber precision)
        {
            return new BigDecimal(value, precision);
        }

        #endregion

        #region Explicits

        public static explicit operator string(BigDecimal value)
        {
            return value.ToString();
        }

        public static explicit operator byte(BigDecimal value)
        {
            return (byte)value._integerLeft;
        }

        public static explicit operator sbyte(BigDecimal value)
        {
            return (sbyte)value._integerLeft;
        }

        public static explicit operator short(BigDecimal value)
        {
            return (short)value._integerLeft;
        }

        public static explicit operator ushort(BigDecimal value)
        {
            return (ushort)value._integerLeft;
        }

        public static explicit operator int(BigDecimal value)
        {
            return (int)value._integerLeft;
        }

        public static explicit operator uint(BigDecimal value)
        {
            return (uint)value._integerLeft;
        }

        public static explicit operator long(BigDecimal value)
        {
            return (long)value._integerLeft;
        }

        public static explicit operator ulong(BigDecimal value)
        {
            return (ulong)value._integerLeft;
        }

        public static explicit operator float(BigDecimal value)
        {
            return Convert.ToSingle(value.ToString(), ConvertCultureInfo);
        }

        public static explicit operator double(BigDecimal value)
        {
            return Convert.ToDouble(value.ToString(), ConvertCultureInfo);
        }

        public static explicit operator decimal(BigDecimal value)
        {
            return Convert.ToDecimal(value.ToString(), ConvertCultureInfo);
        }

        #endregion

        public override string ToString()
        {
            var rightText = _integerRight.ToString();
            return (this._isNeg ? "-" : "") + _integerLeft.ToString() + "." + rightText.PadLeft((int)PrecisionLength, '0');
        }

        #endregion
    }
}