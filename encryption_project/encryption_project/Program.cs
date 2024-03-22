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
                Console.WriteLine("Usage: [input file] [output file]");
                return;
            } else if (args.Length < 2){
                Console.WriteLine("Usage: [input file] [output file]");
                return;
            }

            string fileIn = $"{args[0]}";
            string fileOut = $"{args[1]}";

            FileStream fileRead = new(fileIn, FileMode.Open, FileAccess.Read);
            FileStream fileWrite = new(fileOut, FileMode.OpenOrCreate, FileAccess.Write);

            using(Aes newAes = Aes.Create()){
                // Set up the Aes object and CryptoStream
                ICryptoTransform dataEncrypt = newAes.CreateEncryptor(newAes.Key, newAes.IV);
                CryptoStream writeStream = new(fileWrite, dataEncrypt, CryptoStreamMode.Write);

                byte[] buf = new byte[1024];
                UTF8Encoding format = new UTF8Encoding(true);
                int fileSize;
                // .Read method returns total amount of bytes read into buffer; method ALSO reads bytes into buffer when called
                while ((fileSize = fileRead.Read(buf, 0 , buf.Length)) > 0)
                {
                    // UTF8Encoding object has GetString method, formats bytes into utf8
                    Console.WriteLine(format.GetString(buf));
                    writeStream.Write(buf);
                }
                
                fileRead.Close();
                fileWrite.Close();
            }
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