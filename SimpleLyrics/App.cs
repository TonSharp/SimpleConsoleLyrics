using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLyrics
{
    public static class App
    {
        public static bool IsActive = true;

        public static void Start()
        {
            IsActive = true;
        }
        public static void Stop()
        {
            IsActive = false;
        }
    }
}
