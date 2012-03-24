using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public static class RandomExtentions
    {
        public static DateTime NextDateTime(this Random rand, DateTime min,DateTime max)
        {
            return new DateTime((((long)(((max.Ticks - min.Ticks) * rand.NextDouble()) + min.Ticks))/100000)* 100000);
        }

        public static string NextString(this Random rand, int maxLength)
        {
            int length =  rand.Next(maxLength);

            Byte[] randomBytes = new Byte[length];
            rand.NextBytes(randomBytes);
            char[] chars = new char[length];

            String allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";//!*()-_I1@#$&+={}[]:;?/><.,|\\^`";

            int allowedCharCount = allowedChars.Length;

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }
    }
}
