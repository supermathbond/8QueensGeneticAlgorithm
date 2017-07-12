using System;

namespace _8QueensGA
{
    class Program
    {
        static void Main(string[] args)
        {
            IRandom random = SimpleRandomSingleton.Instance;

            int populationSize = 100;
            int maxGenerations = 5000;
            float mutationProbabilty = 0.07f;
            string gaOutputPath = "C:\\QueensRes-Genetic.csv";

            PopulationManager population = new PopulationManager(populationSize, mutationProbabilty, random);
            population.StartEvolution(maxGenerations, gaOutputPath);

            Console.ReadKey();

            // Now compare it to brute-force perfomance.
            string bfOutputPath = "C:\\QueensRes-BruteForce.csv";

            BruteForce bf = new BruteForce();
            bf.Run(populationSize, maxGenerations, bfOutputPath, random);

            Console.ReadKey();

        }
    }
}
