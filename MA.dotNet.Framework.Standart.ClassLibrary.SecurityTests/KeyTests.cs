using MA.dotNet.Framework.Standart.ClassLibrary.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MA.dotNet.Framework.Standart.ClassLibrary.SecurityTests
{
    [TestClass]
    public class KeyTests
    {
        [TestMethod]
        public void Constructor()
        {
            #region Same Value in Convert Key
            {
                var key = new Key(256);
                for (int queue = 0; queue < key.LengthKey; queue++)
                {
                    byte[] values = new byte[256];
                    for (int valueIndex = 0; valueIndex < values.Length; valueIndex++)
                    {
                        values[valueIndex] = key.ConvertKey[queue, valueIndex];
                    }

                    var countValuesByGroup = values.GroupBy(o => o).Count();
                    Assert.AreEqual(countValuesByGroup, values.Length, message: "Exists same value in the Convert Key!");
                }
            }
            #endregion
        }

        [TestMethod]
        public void SaveAndRead()
        {
            #region Are Equal Save key and Read key
            {
                var keyForSave = new Key(256);
                var buffer = keyForSave.SaveAsByteArray();
                var keyForRead = Key.ReadFromByteArray(buffer, keyForSave.LengthKey);

                #region Check vue Control Key
                for (int queue = 0; queue < keyForSave.LengthKey; queue++)
                {
                    for (int byteValue = 0; byteValue < keyForSave.ConvertKey.GetLength(1); byteValue++)
                    {
                        Assert.AreEqual(keyForSave.ConvertKey[queue, byteValue], keyForRead.ConvertKey[queue, byteValue], message: "Save - Convert Key value is not Read - Convert Key value!");
                    }
                }
                #endregion
            }
            #endregion
        }

        [TestMethod]
        public void EncryptAndDecrypt()
        {
            Random rndm = new Random();

            #region Are Equal Encryption and Decryption
            // Create new random buffer
            var buffer = new byte[36];
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)rndm.Next(0, 256);

            var key = new Key(36);
            var afterEncryption = key.Encrypt(buffer);
            var afterDecryption = key.Decrypt(afterEncryption);

            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.AreEqual(buffer[i], afterDecryption[i], message: "Decryption value is not buffer value!");
            }
            #endregion
        }
    }
}
