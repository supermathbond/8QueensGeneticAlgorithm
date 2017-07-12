using System;

namespace _8QueensGA
{
    public class SimpleRandomSingleton : IRandom
    {
        private static readonly IRandom _instance = new SimpleRandomSingleton();

        public static IRandom Instance
        {
            get
            {
                return _instance;
            }
        }

        private Random _randomNumber;
        
        private SimpleRandomSingleton()
        {
            _randomNumber = new Random();
        }

        public int RandomInt(int lowerBound, int upperBound)
        {
            return _randomNumber.Next(lowerBound, upperBound);
        }

        public double RandomDoubleBetweenZeroAndOne()
        {
            return _randomNumber.NextDouble();
        }
    }
}
