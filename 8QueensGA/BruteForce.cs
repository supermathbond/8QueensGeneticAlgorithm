using System;
using System.Globalization;
using System.IO;

namespace _8QueensGA
{
    public class BruteForce
    {
        public void Run(int attemptsForGeneration, int maxGenerations, string outputPath, IRandom random)
        {
            using (StreamWriter csvWriter = new StreamWriter(outputPath))
            {
                csvWriter.WriteLine("Generation,BestFitness,Average,Worst");

                int gen = 0;
                bool hasSolution = false;
                int[] solution = null;

                while (gen < maxGenerations && !hasSolution)
                {
                    float avg = 0;
                    int best = 0;
                    int worst = 32;
                    for (int i = 0; i < attemptsForGeneration; i++)
                    {
                        int[] arr = GenerateMatrix(random);
                        int fitness = HasCollision(arr);

                        if (fitness == 32)
                        {
                            hasSolution = true;
                            solution = arr;
                        }

                        if (fitness < worst) worst = fitness;
                        if (fitness > best) best = fitness;
                        avg += fitness;
                    }
                    avg /= (float)attemptsForGeneration;

                    PrintGenerationData(csvWriter, gen, best, worst, avg);
                    gen++;
                }

                Console.WriteLine("END!");

                if (hasSolution)
                {
                    PrintBestChromosome(solution);
                    Console.ReadKey();
                }
            }
        }

        private void PrintBestChromosome(int[] bestChrom)
        {
            Console.WriteLine("--------------Best----------");
            for (int i = 0; i < bestChrom.Length; i++)
            {
                Console.Write("(" + (i + 1).ToString() + ", " + (bestChrom[i] + 1).ToString() + "),");
            }
        }

        private void PrintGenerationData(StreamWriter csvWriter, int generationCounter, int maxFitness, int minFitness, float average)
        {
            Console.WriteLine("Round: " + generationCounter);
            Console.WriteLine("Max: " + maxFitness);
            Console.WriteLine("Min: " + minFitness);
            Console.WriteLine("Average: " + average);
            Console.WriteLine("------------------------");

            csvWriter.WriteLine("{0},{1},{2},{3}", generationCounter.ToString(CultureInfo.InvariantCulture),
                maxFitness.ToString(CultureInfo.InvariantCulture),
                average.ToString(CultureInfo.InvariantCulture), minFitness.ToString(CultureInfo.InvariantCulture));
        }


        private int[] GenerateMatrix(IRandom random)
        {
            int[] arr = new int[8];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.RandomInt(0, arr.Length);
            }

            return arr;
        }

        private int HasCollision(int[] arr)
        {
            int fitness = 32;

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] == arr[j] ||               // Same column
                        arr[i] == (arr[j] - (j - i)) ||   // Same diagonal 
                        arr[i] == (arr[j] + (j - i)))     // Same diagonal 
                    {
                        fitness--;
                    }
                }
            }

            return fitness;
        }
    }
}