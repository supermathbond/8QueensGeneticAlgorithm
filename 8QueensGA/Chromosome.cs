namespace _8QueensGA
{
    class Chromosome
    {
        /// <summary>
        /// Each index define a row in the chess board.
        /// Each value define the column of the location of the queen.
        /// Location = (row, column) => (index, value).
        /// </summary>
        public int[] QueensLocation { get; set; }

        /// <summary>
        /// The fitness calculation of the chromosome.
        /// </summary>
        public int Fitness {
            get { return CalculateFitness(); }  }

        /// <summary>
        /// Max number of collisions.
        /// </summary>
        public int MaxFitness {
            get { return (QueensLocation.Length)*QueensLocation.Length/2; }
        }

        /// <summary>
        /// Crossover of 2 chromosomes. single point crossover.
        /// </summary>
        /// <param name="dadChromosome"></param>
        /// <param name="momChromosome"></param>
        /// <param name="crossOverLocation"></param>
        /// <returns></returns>
        public static Chromosome[] CrossoverChromosomes(Chromosome dadChromosome, Chromosome momChromosome,
            int crossOverLocation)
        {
            int[] firstQueensLocation = new int[dadChromosome.QueensLocation.Length];
            int[] secondQueensLocation = new int[momChromosome.QueensLocation.Length];
            for (int i = 0; i < firstQueensLocation.Length; i++)
            {
                if (i < crossOverLocation)
                {
                    firstQueensLocation[i] = dadChromosome.QueensLocation[i];
                    secondQueensLocation[i] = momChromosome.QueensLocation[i];
                }
                else
                {
                    firstQueensLocation[i] = momChromosome.QueensLocation[i];
                    secondQueensLocation[i] = dadChromosome.QueensLocation[i];
                }
            }

            return new Chromosome[] {new Chromosome(firstQueensLocation), new Chromosome(secondQueensLocation) };
        }

        public Chromosome(IRandom random, int numberOfQueens = 8)
        {
            // GenerateMatrix
            QueensLocation = new int[numberOfQueens];
            for (int i = 0; i < QueensLocation.Length; i++)
            {
                QueensLocation[i] = random.RandomInt(0, QueensLocation.Length);
            }
        }

        public Chromosome(int[] queensLocation)
        {
            QueensLocation = queensLocation;
        }

        public int CalculateFitness()
        {
            int fitness = MaxFitness; // Max number of collisions.

            for (int i = 0; i < QueensLocation.Length; i++)
            {
                for (int j = i + 1; j < QueensLocation.Length; j++)
                {
                    if (QueensLocation[i] == QueensLocation[j] ||               // Same column
                        QueensLocation[i] == (QueensLocation[j] - (j - i)) ||   // Same diagonal 
                        QueensLocation[i] == (QueensLocation[j] + (j - i)))     // Same diagonal 
                    {
                        fitness--;
                    }
                }
            }

            return fitness;
        }

        public void Mutate(float mutationProbabilty, IRandom random)
        {
            for (int i = 0; i < QueensLocation.Length; i++)
            {
                if (random.RandomDoubleBetweenZeroAndOne() < mutationProbabilty)
                {
                    QueensLocation[i] = random.RandomInt(0, QueensLocation.Length);
                }
            }
        }
    }
}
