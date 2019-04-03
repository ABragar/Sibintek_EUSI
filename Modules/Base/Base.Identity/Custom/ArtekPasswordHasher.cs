using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Base.Identity.Core;
using Microsoft.AspNet.Identity;

namespace Base.Identity.Custom
{




    public class ArtekPasswordHasher : ICustomPasswordStorage
    {
        private class CustomRfc2898DeriveBytes : DeriveBytes
        {
            private byte[] m_buffer;
            private byte[] m_salt;
            private HashAlgorithm m_hmac;
            private byte[] m_password;
            private uint m_iterations;
            private uint m_block;
            private int m_startIndex;
            private int m_endIndex;

            /// <summary>
            /// Gets or sets the key salt value for the operation.
            /// </summary>
            /// 
            /// <returns>
            /// The key salt value for the operation.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes. </exception><exception cref="T:System.ArgumentNullException">The salt is null. </exception>
            public byte[] Salt
            {
                get
                {
                    return (byte[])m_salt.Clone();
                }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException("value");
                    if (value.Length < 8)
                        throw new ArgumentException();
                    m_salt = (byte[])value.Clone();
                    Initialize();
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes"/> class using a password, a salt, and number of iterations to derive the key.
            /// </summary>
            /// <param name="password">The password used to derive the key. </param><param name="salt">The key salt used to derive the key.</param><param name="iterations">The number of iterations for the operation. </param><exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1. </exception><exception cref="T:System.ArgumentNullException">The password or salt is null. </exception>
            [SecuritySafeCritical]
            public CustomRfc2898DeriveBytes(byte[] password, byte[] salt, uint iterations)
            {
                Salt = salt;
                m_iterations = iterations;
                m_password = password;
                m_hmac = new HMACSHA512(password);
                Initialize();
            }

            /// <summary>
            /// Returns the pseudo-random key for this object.
            /// </summary>
            /// 
            /// <returns>
            /// A byte array filled with pseudo-random key bytes.
            /// </returns>
            /// <param name="cb">The number of pseudo-random key bytes to generate. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="cb "/>is out of range. This parameter requires a non-negative number.</exception>
            public override byte[] GetBytes(int cb)
            {
                if (cb <= 0)
                    throw new ArgumentOutOfRangeException("cb");
                byte[] numArray1 = new byte[cb];
                int dstOffsetBytes = 0;
                int byteCount = m_endIndex - m_startIndex;
                if (byteCount > 0)
                {
                    if (cb >= byteCount)
                    {
                        Buffer.BlockCopy(m_buffer, m_startIndex, numArray1, 0, byteCount);
                        m_startIndex = m_endIndex = 0;
                        dstOffsetBytes += byteCount;
                    }
                    else
                    {
                        Buffer.BlockCopy(m_buffer, m_startIndex, numArray1, 0, cb);
                        m_startIndex = m_startIndex + cb;
                        return numArray1;
                    }
                }
                while (dstOffsetBytes < cb)
                {
                    byte[] numArray2 = Func();
                    int num1 = cb - dstOffsetBytes;
                    if (num1 > 20)
                    {
                        Buffer.BlockCopy(numArray2, 0, numArray1, dstOffsetBytes, 20);
                        dstOffsetBytes += 20;
                    }
                    else
                    {
                        Buffer.BlockCopy(numArray2, 0, numArray1, dstOffsetBytes, num1);
                        int num2 = dstOffsetBytes + num1;
                        Buffer.BlockCopy(numArray2, num1, m_buffer, m_startIndex, 20 - num1);
                        m_endIndex = m_endIndex + (20 - num1);
                        return numArray1;
                    }
                }
                return numArray1;
            }

            /// <summary>
            /// Resets the state of the operation.
            /// </summary>
            public override void Reset()
            {
                Initialize();
            }

            /// <summary>
            /// Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes"/> class and optionally releases the managed resources.
            /// </summary>
            /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (!disposing)
                    return;
                if (m_hmac != null)
                    m_hmac.Dispose();
                if (m_buffer != null)
                    Array.Clear(m_buffer, 0, m_buffer.Length);
                if (m_salt == null)
                    return;
                Array.Clear(m_salt, 0, m_salt.Length);
            }

            private void Initialize()
            {
                if (m_buffer != null)
                    Array.Clear(m_buffer, 0, m_buffer.Length);
                m_buffer = new byte[20];
                m_block = 1U;
                m_startIndex = m_endIndex = 0;
            }

            private static readonly byte[] EmptyArray = new byte[0];

            private byte[] Func()
            {
                byte[] inputBuffer1 = Int(m_block);
                m_hmac.TransformBlock(m_salt, 0, m_salt.Length, null, 0);
                m_hmac.TransformBlock(inputBuffer1, 0, inputBuffer1.Length, null, 0);
                m_hmac.TransformFinalBlock(EmptyArray, 0, 0);
                byte[] inputBuffer2 = m_hmac.Hash;
                m_hmac.Initialize();
                byte[] numArray = inputBuffer2;
                for (int index1 = 2; (long)index1 <= (long)m_iterations; ++index1)
                {
                    m_hmac.TransformBlock(inputBuffer2, 0, inputBuffer2.Length, null, 0);
                    m_hmac.TransformFinalBlock(EmptyArray, 0, 0);
                    inputBuffer2 = m_hmac.Hash;
                    for (int index2 = 0; index2 < 20; ++index2)
                        numArray[index2] ^= inputBuffer2[index2];
                    m_hmac.Initialize();
                }
                m_block = m_block + 1U;
                return numArray;
            }
            internal static byte[] Int(uint i)
            {
                return new byte[]
                {
                    (byte) (i >> 24),
                    (byte) (i >> 16),
                    (byte) (i >> 8),
                    (byte) i
                };
            }
        }


        private bool IsCustomHash(string hashedPassword)
        {
            return hashedPassword.StartsWith("pbkdf2_sha256$");
        }

        private bool VerifyCustomPassword(string hashedPassword, string providedPassword)
        {

            var parameters = hashedPassword.Split('$');

            uint iteration;

            if (parameters.Length == 4 && uint.TryParse(parameters[1], out iteration))
            {
                var salt = Convert.FromBase64String(parameters[2]);
                var hash = Convert.FromBase64String(parameters[3]);

                using (var alg = new CustomRfc2898DeriveBytes(new UTF8Encoding(false).GetBytes(providedPassword), salt,
                        iteration))
                {
                    var bytes = alg.GetBytes(hash.Length);

                    return ByteArraysEqual(bytes, hash);

                }


            }
            return false;

        }



        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (int index = 0; index < a.Length; ++index)
            {
                if (a[index] == b[index])
                    return false;
            }
            return true;
        }


        public Task<Tuple<SetPasswordResult, string>> SetPasswordAsync(string login, string password)
        {

            return Task.FromResult(Tuple.Create(SetPasswordResult.UseDefault, (string)null));

        }

        public Task<VerifyPasswordResult> VerifyPasswordAsync(string login, string hash, string password)
        {
            if (!IsCustomHash(hash))
                return Task.FromResult(VerifyPasswordResult.UseDefault);

            if (VerifyCustomPassword(hash, password))
                return Task.FromResult(VerifyPasswordResult.NeedReset);

            return Task.FromResult(VerifyPasswordResult.Failed);
        }

        public Task<HasPasswordResult> HasPasswordAsync(string login, string hash)
        {
            return IsCustomHash(hash) ? Task.FromResult(HasPasswordResult.True) : Task.FromResult(HasPasswordResult.UseDefault);
        }
    }
}