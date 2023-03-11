using System;

namespace ButtonMash
{
    public static class GlobalVariables
    {
        public static readonly int maxLives = 6;
        public static int Lives { 
            get { return lives; }
            set { lives = value > maxLives ? maxLives : value; }
            }
        private static int lives;

        public static int Combo { 
            get { return combo; }
            set { combo = value;
                if (combo % 10 == 0 && combo != 0)
                    ++ComboProcs;
                if (value == 0)
                    ComboProcs = 0;
            }
        }
        private static int combo;
        public static int ComboProcs { get; set; }
        
        public static int Score { get; set; }
        public static int Time { get; set; }
        public static bool BigSlam { get; set; }
        public static Random Random => random;
        private static Random random = new Random();
    }
}
