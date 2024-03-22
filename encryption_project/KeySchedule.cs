using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Key{
    public List<byte> _key = new List<byte> ( new byte[16] );

}

public class KeySchedule{
    static List<byte> GetKS(Key newKey){
            int n = newKey._key.Count / 32;
            return;
        }
    private class RoundConstant{
        static List<byte> GetRC(Key newKey){
            int n = newKey._key.Count / 32;
            Console.WriteLine(n);
            byte[] rcon = new byte[4];
            for (int i = 0; i < 10; i++){
                rcon[0] = ;
            }
            return ;
        }
    }
}
