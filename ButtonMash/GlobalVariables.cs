using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonMash
{
    public class GlobalVariables
    {
        //Deklarationer
        public static int Lives, Time, Score, Combo;
        public static bool BigSlam;
        public static Random Random => random;
        private static Random random = new Random();
    }
}
