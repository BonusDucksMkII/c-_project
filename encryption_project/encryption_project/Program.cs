using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace encryption_project
{
    class Program
    {
        static void Main(string[] args)
        {   
            // Console.WriteLine(args[0]);
            if (args.Length == 0){
                Console.WriteLine("This program will encrypt an input file using the AES.");
                Console.WriteLine("Usage: [input file] [output file] [-e/-d (encrypt/decrypt)]");
                return;
            } else if (args.Length < 3){
                Console.WriteLine("Usage: [input file] [output file] [-e/-d (encrypt/decrypt)]");
                return;
            } else if (args.Length > 3){
                Console.WriteLine("Usage: [input file] [output file] [-e/-d (encrypt/decrypt)]");
                return;
            }

            string fileIn = $"{args[0]}";
            string fileOut = $"{args[1]}";
            string fileOption = $"{args[2]}";

            Aes newAes = Aes.Create();

            if (fileOption == "-e"){
                EncryptFile(newAes, fileIn, fileOut);
            } else if (fileOption == "-d"){
                DecryptFile(newAes, fileIn, fileOut);
            } else {
                Console.WriteLine("Invalid option");
                return;
            }
        }
        static void EncryptFile(Aes aesObj, string fileInName, string fileOutName){
            FileStream fileRead = new(fileInName, FileMode.Open, FileAccess.Read);
            FileStream fileWrite = new(fileOutName, FileMode.OpenOrCreate, FileAccess.Write);

            // 'using' keyword disposes IDisposable object (in this case, the Aes object) at end of block
            using(Aes newAes = Aes.Create()){
                // Set up the Aes object and CryptoStream
                // Removed padding since PCKS7 (the default) wasn't working right
                newAes.Padding = PaddingMode.None;
                ICryptoTransform dataEncrypt = newAes.CreateEncryptor(aesObj.Key, aesObj.IV);
                CryptoStream writeStream = new(fileWrite, dataEncrypt, CryptoStreamMode.Write);

                byte[] buf = new byte[1024];
                UTF8Encoding format = new UTF8Encoding(true);

                // .Read method returns total amount of bytes read into buffer; method ALSO reads bytes into buffer when called
                while (fileRead.Read(buf, 0 , buf.Length) > 0)
                {
                    // UTF8Encoding object has GetString method, formats bytes into utf8
                    Console.WriteLine(format.GetString(buf));
                    // stream objects all derive from base class, CryptoStream
                    writeStream.Write(buf);
                }
                // Save the key for decrypting
                WriteKey(aesObj);
                WriteIV(aesObj);

                // Close the streams
                writeStream.Close();
                fileRead.Close();
                fileWrite.Close();
            }
        }
        static void DecryptFile(Aes aesObj, string fileInName, string fileOutName){
            FileStream fileRead = new(fileInName, FileMode.Open, FileAccess.Read);
            FileStream fileWrite = new(fileOutName, FileMode.OpenOrCreate, FileAccess.Write);

            using(Aes newAes = Aes.Create()){
                // Set up the Aes object and CryptoStream
                // Removed padding since PCKS7 (the default) wasn't working right
                newAes.Padding = PaddingMode.None;
                ICryptoTransform dataDecrypt = newAes.CreateDecryptor(ReadKey("key.des"), ReadIV("iv.des"));
                CryptoStream writeStream = new(fileWrite, dataDecrypt, CryptoStreamMode.Write);

                byte[] buf = new byte[1024];
                UTF8Encoding format = new UTF8Encoding(true);

                // .Read method returns total amount of bytes read into buffer; method ALSO reads bytes into buffer when called
                while (fileRead.Read(buf, 0 , buf.Length) > 0)
                {
                    // UTF8Encoding object has GetString method, formats bytes into utf8
                    Console.WriteLine(format.GetString(buf));
                    writeStream.Write(buf);
                }
                // Close the streams
                writeStream.Close();
                fileRead.Close();
                fileWrite.Close();
            }
        }
        static void WriteKey(Aes aesObj){
            FileStream fileWrite = new("key.des", FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buf = aesObj.Key;

            fileWrite.Write(buf);

            fileWrite.Close();
        }
        static void WriteIV(Aes aesObj){
            FileStream fileWrite = new("iv.des", FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buf = aesObj.IV;

            fileWrite.Write(buf);

            fileWrite.Close();
        }
        static byte[] ReadKey(string keyFile){
            FileStream fileRead = new(keyFile, FileMode.OpenOrCreate, FileAccess.Read);
            byte[] buf = new byte[32];

            fileRead.Read(buf, 0, buf.Length);
            return buf;
        }
        static byte[] ReadIV(string keyFile){
            FileStream fileRead = new(keyFile, FileMode.OpenOrCreate, FileAccess.Read);
            byte[] buf = new byte[16];

            fileRead.Read(buf, 0, buf.Length);
            return buf;
        }
    }
    
}



/* 
array of 4x4 bytes is the 'state', fixed block size of 128, and key size of 128, 192, or 256
128-bit key has 10 transformation rounds
KeyExpansion - get 'round keys' from 'key schedule'
KeySchedule:
    RoundConstant: 32-bit word rcon = [rc[i] 0x00 0x00 0x00], where 'i' is the round number, 
    rc[i] is 8-bit value where rc[i] = {1 if i = 1,
                                        2 * rc[i-1] if i > 1 and rc[i-1] < 0x80,
                                        (2 * rc[i-1]) XOR ('^' in C#) 0x11b if i > 1 and rc[i-1] >= 0x80}

    Schedule takes short key and expands it into separate round keys using round constants and key schedule algorithm
    key is 128, 192, or 256 bits long
    n = key.length/32
    k[n-1]: each index is part of key divided into 32-bit words
    r = 11 if key.length = 128, 13 for 192, 15 for 256
    w[4 * r - 1]: each index is part of extended key divided into 32-bit words
    RotWord(word) => {return (word << 8) || (word >> (32 - 8))}

*/