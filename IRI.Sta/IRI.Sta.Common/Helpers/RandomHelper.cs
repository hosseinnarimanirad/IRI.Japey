using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Helpers
{
    public class RandomHelper
    {
        static RandomHelper()
        {
            _random = new Random();

            syncLock = new object();
        } 

        private static readonly Random _random;

        private static readonly object syncLock;

        public static int Get(int minValue, int maxValue)
        {
            lock (syncLock)
            { // synchronize
                return _random.Next(minValue, maxValue);
            }
        }

        public static int Get(int maxValue)
        {
            return Get(0, maxValue);
        }
    }
}
