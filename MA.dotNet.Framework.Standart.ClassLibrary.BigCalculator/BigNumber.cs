using System;
using System.Numerics;

namespace MA.dotNet.Framework.Standart.ClassLibrary.BigCalculator
{
    public class BigNumber
    {
        #region Constructs

        private BigNumber(BigInteger value)
        {
            this._integer = value;
        }

        public BigNumber(string value)
        {
            this._integer = BigInteger.Parse(value);
        }

        public BigNumber(byte value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(sbyte value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(short value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(ushort value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(int value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(uint value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(long value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(ulong value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(float value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(double value)
        {
            this._integer = new BigInteger(value);
        }

        public BigNumber(decimal value)
        {
            this._integer = new BigInteger(value);
        }

        #endregion

        #region Variables

        BigInteger _integer;
        public BigInteger Integer
        {
            get
            {
                return _integer;
            }
        }

        public static BigNumber MinusOne
        {
            get
            {
                return BigInteger.MinusOne;
            }
        }
        public static BigNumber One
        {
            get
            {
                return BigInteger.MinusOne;
            }
        }
        public static BigNumber Zero
        {
            get
            {
                return BigInteger.MinusOne;
            }
        }
        public static BigNumber Ten = new BigNumber(10);
        public int Sign
        {
            get
            {
                return this._integer.Sign;
            }
        }
        public bool IsPowerOfTwo
        {
            get
            {
                return this._integer.IsPowerOfTwo;
            }
        }
        public bool IsZero
        {
            get
            {
                return this._integer.IsZero;
            }
        }
        public bool IsOne
        {
            get
            {
                return this._integer.IsOne;
            }
        }
        public bool IsEven
        {
            get
            {
                return this._integer.IsEven;
            }
        }

        #endregion

        #region Methods

        #region Operator Override

        #region Math

        public static BigNumber operator +(BigNumber value1, BigNumber value2)
        {
            return BigInteger.Add(value1, value2);
        }

        public static BigNumber operator ++(BigNumber value)
        {
            return value._integer++;
        }

        public static BigNumber operator -(BigNumber value1, BigNumber value2)
        {
            return BigInteger.Subtract(value1, value2);
        }

        public static BigNumber operator --(BigNumber value)
        {
            return value._integer--;
        }

        public static BigNumber operator *(BigNumber value1, BigNumber value2)
        {
            return BigInteger.Multiply(value1._integer, value2._integer);
        }

        public static BigNumber operator /(BigNumber value1, BigNumber value2)
        {
            return BigInteger.Divide(value1._integer, value2._integer);
        }

        public static BigNumber operator %(BigNumber value1, BigNumber value2)
        {
            return value1._integer % value2._integer;
        }

        public static BigNumber operator ^(BigNumber value1, BigNumber value2)
        {
            return value1._integer ^ value2._integer;
        }

        public static BigNumber operator <<(BigNumber value1, int value2)
        {
            return value1._integer << value2;
        }

        public static BigNumber operator >>(BigNumber value1, int value2)
        {
            return value1._integer >> value2;
        }

        public static BigNumber operator &(BigNumber value1, int value2)
        {
            return value1._integer & value2;
        }

        public static BigNumber operator |(BigNumber value1, int value2)
        {
            return value1._integer | value2;
        }

        public static BigNumber operator ~(BigNumber value)
        {
            return ~value._integer;
        }

        #endregion

        #region IF

        public static bool operator >(BigNumber value1, BigNumber value2)
        {
            return value1._integer > value2._integer;
        }

        public static bool operator >=(BigNumber value1, BigNumber value2)
        {
            return value1._integer >= value2._integer;
        }

        public static bool operator <(BigNumber value1, BigNumber value2)
        {
            return value1._integer < value2._integer;
        }

        public static bool operator <=(BigNumber value1, BigNumber value2)
        {
            return value1._integer <= value2._integer;
        }

        public static bool operator !=(BigNumber value1, BigNumber value2)
        {
            return value1._integer != value2._integer;
        }

        public static bool operator ==(BigNumber value1, BigNumber value2)
        {
            return value1._integer == value2._integer;
        }

        public override bool Equals(object obj)
        {
            return _integer.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _integer.GetHashCode();
        }

        #endregion

        #endregion

        #region Implicits

        public static implicit operator BigInteger(BigNumber value)
        {
            return value._integer;
        }

        public static implicit operator BigNumber(BigInteger value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(string value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(byte value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(sbyte value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(short value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(ushort value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(int value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(uint value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(long value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(ulong value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(float value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(double value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(decimal value)
        {
            return new BigNumber(value);
        }

        #endregion

        #region Explicits

        public static explicit operator string(BigNumber value)
        {
            return value.ToString();
        }

        public static explicit operator byte(BigNumber value)
        {
            return (byte)value._integer;
        }

        public static explicit operator sbyte(BigNumber value)
        {
            return (sbyte)value._integer;
        }

        public static explicit operator short(BigNumber value)
        {
            return (short)value._integer;
        }

        public static explicit operator ushort(BigNumber value)
        {
            return (ushort)value._integer;
        }

        public static explicit operator int(BigNumber value)
        {
            return (int)value._integer;
        }

        public static explicit operator uint(BigNumber value)
        {
            return (uint)value._integer;
        }

        public static explicit operator long(BigNumber value)
        {
            return (long)value._integer;
        }

        public static explicit operator ulong(BigNumber value)
        {
            return (ulong)value._integer;
        }

        public static explicit operator float(BigNumber value)
        {
            return (float)value._integer;
        }

        public static explicit operator double(BigNumber value)
        {
            return (double)value._integer;
        }

        public static explicit operator decimal(BigNumber value)
        {
            return (decimal)value._integer;
        }

        #endregion

        public static bool TryParse(string value, out BigNumber result)
        {
            BigInteger resultOrg;
            bool control = BigInteger.TryParse(value, out resultOrg);

            result = resultOrg;
            return control;
        }

        #region Math

        public BigNumber GreatestCommonDivisor(BigNumber right)
        {
            return BigInteger.GreatestCommonDivisor(this, right);
        }

        public BigNumber Log(double baseValue)
        {
            return BigInteger.Log(this, baseValue);
        }

        public BigNumber Log()
        {
            return BigInteger.Log(this);
        }

        public BigNumber Log10()
        {
            return BigInteger.Log10(this);
        }

        public BigNumber Max(BigNumber right)
        {
            return BigInteger.Max(this._integer, right._integer);
        }

        public BigNumber Min(BigNumber right)
        {
            return BigInteger.Min(this._integer, right._integer);
        }

        public BigNumber Abs()
        {
            return BigInteger.Abs(this._integer);
        }

        public BigNumber Pow(BigNumber pow)
        {
            BigInteger value = this._integer;
            BigInteger powWithCast = pow._integer;

            BigInteger total = 1;
            while (powWithCast > int.MaxValue)
            {
                powWithCast -= int.MaxValue;
                total = total * BigInteger.Pow(value, int.MaxValue);
            }
            total = total * BigInteger.Pow(value, (int)powWithCast);

            return total;
        }

        #endregion

        public override string ToString()
        {
            return this._integer.ToString(BigDecimal.ConvertCultureInfo);
        }

        #endregion
    }
}