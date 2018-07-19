using System;
using System.Collections.Generic;
using System.Linq;

namespace MA.dotNet.Framework.Standart.ClassLibrary.Security
{
    public class Key
    {
        #region Constructors
        private Key()
        { }

        /// <summary>
        /// Created new random key
        /// </summary>
        /// <param name="lengthKey">Key length</param>
        public Key(int lengthKey)
        {
            this.ConvertKey = new byte[lengthKey, 256];
            this.LengthKey = lengthKey;

            Random rndm = new Random();

            #region Create Convert Key
            for (int queue = 0; queue < LengthKey; queue++)
            {
                for (int byteValue = 0; byteValue < ConvertKey.GetLength(1); byteValue++)
                {
                    while (true)
                    {
                        var newByteValue = (byte)rndm.Next(0, 256);

                        var existsNewByteValue = false;
                        if (byteValue > 0)
                        {
                            for (int controlByteValue = 0; controlByteValue < byteValue; controlByteValue++)
                            {
                                if (newByteValue == ConvertKey[queue, controlByteValue])
                                {
                                    existsNewByteValue = true;
                                    break;
                                }
                            }
                        }

                        if (existsNewByteValue == false)
                        {
                            ConvertKey[queue, byteValue] = newByteValue;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        #region Variables
        public int LengthKey { get; private set; }

        #region Keys

        public byte[,] ConvertKey { get; private set; }

        #endregion
        #endregion

        #region Methods

        #region Encryption & Decryption
        /// <summary>
        /// This function encrypt to your data
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] buffer)
        {
            List<byte> result = new List<byte>();
            short sumValue = 0;
            for (int queueValueIndex = 0; queueValueIndex < buffer.Length; queueValueIndex++)
            {
                var queueValue = buffer[queueValueIndex];
                byte newByteValue = 0;
                for (int byteValueIndex = 0; byteValueIndex < this.ConvertKey.GetLength(1); byteValueIndex++)
                {
                    var byteValue = this.ConvertKey[queueValueIndex % this.LengthKey, byteValueIndex];
                    if (byteValue == queueValue)
                    {
                        newByteValue = (byte)byteValueIndex;
                        break;
                    }
                }
                result.Add(newByteValue);

                sumValue += (short)(queueValue + newByteValue);
                if (sumValue > byte.MaxValue * 2)
                {
                    sumValue %= 256;
                    result.Add((byte)(255 - sumValue));
                }
            }

            var resultAsArray = result.ToArray();
            result.Clear();
            return resultAsArray;
        }

        /// <summary>
        /// This function decrypt to your encrypted data
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] buffer)
        {
            List<byte> result = new List<byte>();
            short sumValue = 0;
            int diffQueueValueIndex = 0;
            for (int queueValueIndex = 0; queueValueIndex < buffer.Length; queueValueIndex++)
            {
                var netQueueValueIndex = queueValueIndex - diffQueueValueIndex;
                var value = buffer[queueValueIndex];
                var oldValue = this.ConvertKey[netQueueValueIndex % this.LengthKey, value];
                result.Add(oldValue);

                sumValue += (short)(oldValue + value);
                if (sumValue > byte.MaxValue * 2)
                {
                    sumValue %= 256;
                    queueValueIndex++;
                    diffQueueValueIndex++;
                }
            }

            var resultAsArray = result.ToArray();
            result.Clear();
            return resultAsArray;
        }
        #endregion

        #region Save & Read
        /// <summary>
        /// This function save key values to a byte array for it's to be readable and writable
        /// </summary>
        /// <returns></returns>
        public byte[] SaveAsByteArray()
        {
            byte[] buffer = new byte[
                (this.LengthKey * this.ConvertKey.GetLength(1))
                ];

            int lastIndexForSave = 0;

            #region Save Convert Key
            for (int queue = 0; queue < this.LengthKey; queue++)
            {
                for (int byteValue = 0; byteValue < this.ConvertKey.GetLength(1); byteValue++)
                {
                    buffer[lastIndexForSave] = this.ConvertKey[queue, byteValue];
                    lastIndexForSave++;
                }
            }
            #endregion

            return buffer;
        }

        /// <summary>
        /// This function read key from byte array
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Key ReadFromByteArray(byte[] buffer, int lengthKey)
        {
            Key key = new Key()
            {
                ConvertKey = new byte[lengthKey, 256],
                LengthKey = lengthKey
            };
            int lastIndexForRead = 0;

            #region Read Convert Key
            for (int queue = 0; queue < key.LengthKey; queue++)
            {
                for (int byteValue = 0; byteValue < key.ConvertKey.GetLength(1); byteValue++)
                {
                    key.ConvertKey[queue, byteValue] = buffer[lastIndexForRead];
                    lastIndexForRead++;
                }
            }
            #endregion
            
            return key;
        }
        #endregion

        #endregion
    }
}
