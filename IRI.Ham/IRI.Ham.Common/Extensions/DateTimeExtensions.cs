using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DateTimeExtensions
    {                
        public static string ToShortTimeCode(this DateTime value)
        {
            return $"{value.Hour}{value.Minute}{value.Second}";            
        }
    } 
}
