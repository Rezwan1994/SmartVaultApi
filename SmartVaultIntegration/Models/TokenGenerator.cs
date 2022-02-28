using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ASPNET_MVC_HelloWorldSVRest.Models
{
    public class TokenGenerator
    {
        class ByteWriter
        {
            private byte[] _array;
            private int _position;
            public ByteWriter(byte[] array)
            {
                _array = array;
                _position = 0;
            }
            public ByteWriter(byte[] array, int position)
            {
                _array = array;
                _position = position;
            }
            public byte[] Buffer { get { return _array; } }
            public void Write(byte b)
            {
                _array[_position++] = b;
            }
            public void Write(string t)
            {
                for (int i = 0; i < t.Length; ++i)
                {
                    _array[_position++] = (byte)t[i];
                }
            }
            public void WriteStrLen(string t)
            {
                int length = t == null ? 0 : t.Length;
                if (length > 255) throw new ArgumentException();
                Write((byte)length);
                if (length != 0)
                {
                    Write(t);
                }
            }
        }
        private const int AuthSchemeSize = 5;
        private const string Scheme = "SLF00";
        private RSACryptoServiceProvider GetPrivateKey(string txt)
        {
            RSACryptoServiceProvider priProvider = null;
            using (var rdr = new StringReader(txt))
            {
                var x = new PemReader(rdr);
                var y = x.ReadObject();
                RsaPrivateCrtKeyParameters pri = null;
                AsymmetricCipherKeyPair k = y as AsymmetricCipherKeyPair;
                pri = k.Private as RsaPrivateCrtKeyParameters;
                priProvider = (RSACryptoServiceProvider)RSACryptoServiceProvider.Create();
                var pria = new RSAParameters();
                // private exponent
                pria.D = pri.Exponent.ToByteArrayUnsigned();
                // exponent1
                pria.DP = pri.DP.ToByteArrayUnsigned();
                // exponent2
                pria.DQ = pri.DQ.ToByteArrayUnsigned();
                // e, the public exponent
                pria.Exponent = pri.PublicExponent.ToByteArrayUnsigned();
                // coefficent
                pria.InverseQ = pri.QInv.ToByteArrayUnsigned();
                // n, modulus
                pria.Modulus = pri.Modulus.ToByteArrayUnsigned();
                // prime1
                pria.P = pri.P.ToByteArrayUnsigned();
                // prime2
                pria.Q = pri.Q.ToByteArrayUnsigned();
                priProvider.ImportParameters(pria);
            }
            return priProvider;
        }
        private byte[] JoinArrays(byte[] array1, byte[] array2)
        {
            byte[] newArray =
                  new byte[array1.Length + array2.Length];
            Array.Copy(array1, newArray, array1.Length);
            Array.Copy(array2, 0, newArray, array1.Length, array2.Length);
            return newArray;
        }
        public string Generate(string privateKey, string clientId, string nonce)
        {
            int rawTokenSize = 1 + clientId.Length + 1 + nonce.Length + AuthSchemeSize;
            ByteWriter writer = new ByteWriter(new byte[rawTokenSize]);
            writer.Write(Scheme);
            writer.WriteStrLen(clientId);
            writer.WriteStrLen(nonce);
            byte[] buffer = writer.Buffer;
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(buffer);
            byte[] signedData = GetPrivateKey(privateKey).SignHash(hash.ToArray(), CryptoConfig.MapNameToOID("SHA256"));
            byte[] signedToken = JoinArrays(buffer, signedData);
            return Convert.ToBase64String(signedToken);
        }
    }

}