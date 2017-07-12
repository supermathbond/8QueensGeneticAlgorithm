using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace _8QueensGA
{
    /// <summary>
    /// Create the generations and manages the genetic changes.
    /// </summary>
    class PopulationManager
    {
        private List<Chromosome> _population;

        private float _mutationProbabilty;

        private IRandom _random;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="numberOfChromosomes"> Population size. </param>
        /// <param name="mutationProbabilty"> Probability for mutation. </param>
        /// <param name="random"> Random object. </param>
        public PopulationManager(int numberOfChromosomes, float mutationProbabilty, IRandom random)
        {
            // Create population.
            _population = new List<Chromosome>(numberOfChromosomes);
            for (int i = 0; i < numberOfChromosomes; i++)
            {
                _population.Add(new Chromosome(random));
            }

            // Set Probability for mutation.
            _mutationProbabilty = mutationProbabilty;

            _random = random;
        }

        /// <summary>
        /// Sort all the chromosomes according to their fitness. Descending order.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        private List<Chromosome> SortByFitness(List<Chromosome> population)
        {
            population.Sort((x, y) =>
            {
                if (x.Fitness > y.Fitness) return 1;
                if (x.Fitness == y.Fitness) return 0;
                return -1;
            });
            population.Reverse();

            return population;
        }

        private void PrintBestChromosome(int[] bestChrom)
        {
            Console.WriteLine("--------------Best----------");
            for (int i = 0; i < bestChrom.Length; i++)
            {
                Console.Write("(" + (i + 1).ToString() + ", " + (bestChrom[i] + 1).ToString() + "),");
            }
        }

        private void PrintGenerationData(StreamWriter csvWriter, int generationCounter)
        {
            float average = (float)_population.Select(chromosome => chromosome.Fitness).Sum() / _population.Count;
            int maxFitness = _population[0].Fitness;
            int minFitness = _population[_population.Count - 1].Fitness;

            Console.WriteLine("Round: " + generationCounter);
            Console.WriteLine("Max: " + maxFitness);
            Console.WriteLine("Min: " + minFitness);
            Console.WriteLine("Average: " + average);
            Console.WriteLine("------------------------");

            csvWriter.WriteLine("{0},{1},{2},{3}", generationCounter.ToString(CultureInfo.InvariantCulture),
                maxFitness.ToString(CultureInfo.InvariantCulture),
                average.ToString(CultureInfo.InvariantCulture), minFitness.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Mutate all chromosomes in population
        /// </summary>
        /// <param name="mutationProbabilty"></param>
        private void MutatePopulation(float mutationProbabilty)
        {
            foreach (var chromosome in _population)
            {
                chromosome.Mutate(mutationProbabilty, _random);
            }
        }

        private void Mate()
        {
            List<Chromosome> nextGeneration = new List<Chromosome>(_population.Count);

            while (_population.Count > 1)
            {
                int parentIndex = _random.RandomInt(0, _population.Count);
                Chromosome dad = _population[parentIndex];
                _population.RemoveAt(parentIndex);
                parentIndex = _random.RandomInt(0, _population.Count);
                Chromosome mom = _population[parentIndex];
                _population.RemoveAt(parentIndex);

                // crossover over the first gene will return the same parents.
                Chromosome[] children = Chromosome.CrossoverChromosomes(dad, mom, _random.RandomInt(1, dad.QueensLocation.Length));

                var family = SortByFitness(new List<Chromosome> { dad, mom, children[0], children[1] });
                family.RemoveRange(2, 2);
                nextGeneration.AddRange(family);
            }

            _population = nextGeneration;
        }

        /// <summary>
        /// Run Evolution process.
        /// </summary>
        public void StartEvolution(int maxGenerations, string outputPath)
        {
            int generationCounter = 0;

            SortByFitness(_population);

            using (StreamWriter csvWriter = new StreamWriter(outputPath))
            {
                csvWriter.WriteLine("Generation,BestFitness,Average,Worst");

                // Run until max fitness rich to the maximum size - 32. (population is always in descending order.!)
                while (_population[0].Fitness < 32 && generationCounter < maxGenerations)
                {
                    PrintGenerationData(csvWriter, generationCounter);

                    Mate();

                    SortByFitness(_population);

                    MutatePopulation(_mutationProbabilty);

                    generationCounter++;
                }

                // Prints the last generation.
                PrintGenerationData(csvWriter, generationCounter);

                PrintBestChromosome(_population[0].QueensLocation);
            }
        }
    }
}
