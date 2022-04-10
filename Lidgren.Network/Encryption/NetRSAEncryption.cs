using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Lidgren.Network
{
    public class NetRSAEncryption : INetEncryption
    {
        private string _key;
        //The buffer size to encrypt per set

        private const int _EncryptionBufferSize = 117;

        // The buffer size to decrypt per set

        private const int _DecryptionBufferSize = 128;

        public NetRSAEncryption(string xmlString)
        {
            _key = xmlString;
        }

        public bool Encrypt(NetOutgoingMessage msg)
        {
            try
            {
                msg.m_data = EncryptData(Convert.ToBase64String(msg.m_data, 0, msg.LengthBytes), _key);
                msg.LengthBytes = msg.m_data.Length;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool Decrypt(NetIncomingMessage msg)
        {
            int mdataL;
            int decryptL;
            try
            {
                string tmp = Convert.ToBase64String(msg.m_data, 0, msg.LengthBytes);
                msg.m_data = Convert.FromBase64String(DecryptData(Convert.FromBase64String(tmp), _key));
                
                mdataL = msg.m_data.Length;
                decryptL = Convert.FromBase64String(DecryptData(Convert.FromBase64String(tmp), _key)).Length;

                msg.LengthBytes = msg.m_data.Length;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static byte[] EncryptData(string data, string publicKey)
        {

            //Create a new asymmetric algorithm object using the supplied public key

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(publicKey);

            //Encode the data

            byte[] dataEncoded = Encoding.UTF8.GetBytes(data);

            //Store every chunk of encrypted data during the encryption process in a memory stream

            using (MemoryStream ms = new MemoryStream())
            {

                //Create a buffer with the maximum allowed size

                byte[] buffer = new byte[_EncryptionBufferSize];

                int pos = 0;

                int copyLength = buffer.Length;

                while (true)
                {

                    //Check if the bytes left to read is smaller than the buffer size, then limit the buffer size to the number of bytes left

                    if (pos + copyLength > dataEncoded.Length)

                        copyLength = dataEncoded.Length - pos;

                    //Create a new buffer that has the correct size

                    buffer = new byte[copyLength];

                    //Copy as many bytes as the algorithm can handle at a time, iterate until the whole input array is encoded

                    Array.Copy(dataEncoded, pos, buffer, 0, copyLength);

                    //Start from here in next iteration

                    pos += copyLength;

                    //Encrypt the data using the public key and add it to the memory buffer

                    //_DecryptionBufferSize is the size of the encrypted data

                    ms.Write(rsa.Encrypt(buffer, false), 0, _DecryptionBufferSize);

                    //Clear the content of the buffer, otherwise we could end up copying the same data during the last iteration

                    Array.Clear(buffer, 0, copyLength);

                    //Check if we have reached the end, then exit

                    if (pos >= dataEncoded.Length)

                        break;

                }

                //Return the encrypted data

                return ms.ToArray();

            }
        }

        public static string DecryptData(byte[] data, string key)
        {

            try
            {

                //Retrieve the private key from the key file path supplied

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(_EncryptionBufferSize);
                rsa.FromXmlString(key);

                //Initialize a memory stream to hold the encrypted chunks of data, use the same size as the encrypted data (however, will be slightly smaller)

                using (MemoryStream ms = new MemoryStream(data.Length))
                {

                    //The buffer that will hold the encrypted chunks

                    byte[] buffer = new byte[_DecryptionBufferSize];

                    int pos = 0;

                    int copyLength = buffer.Length;

                    while (true)
                    {

                        //Copy a chunk of encrypted data / iteration

                        Array.Copy(data, pos, buffer, 0, copyLength);

                        //Set the next start position

                        pos += copyLength;

                        //Decrypt the data using the private key

                        //We need to store the decrypted data temporarily because we don't know the size of it; unlike with encryption where we know the size is 128 bytes. The only thing we know is that it's between 1-117 bytes

                        byte[] resp = rsa.Decrypt(buffer, false);

                        ms.Write(resp, 0, resp.Length);

                        //Cleat the buffers

                        Array.Clear(resp, 0, resp.Length);

                        Array.Clear(buffer, 0, copyLength);

                        //Are we ready to exit?

                        if (pos >= data.Length)

                            break;

                    }

                    //Return the decoded data

                    return Encoding.UTF8.GetString(ms.ToArray());

                }

            }

            //The data is probably corrupted

            catch (CryptographicException ce)
            {

                throw ce;

            }

        }

    }
}

