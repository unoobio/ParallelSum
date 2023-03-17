namespace ParallelSum
{
    internal class SumCalculator
    {
        public static double Sum(List<double> numbers)
            => numbers.Sum();

        public static double SumManualy(List<double> numbers)
        {
            double sum = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                sum += numbers[i];
            }
            return sum;
        }

        public static double SumForeach(List<double> numbers)
        {
            double sum = 0;
            foreach (var number in numbers)
            {
                sum += number;
            }
            return sum;
        }


        public static double SumWithThreads(List<double> numbers, int threadsCount)
        {
            int batchCount = numbers.Count / threadsCount + 1;
            
            double totalSum = 0;
            object lockObject = new object();
            Barrier barrier = new Barrier(threadsCount + 1);
            for (int i = 0; i < threadsCount; i++)
            {
                Thread thread = new Thread(() =>
                {
                    int index = i;
                    double batchSum = GetSumFromBatch(batchCount, index, numbers);
                    lock (lockObject)
                    {
                        totalSum += batchSum;
                    }
                    barrier.SignalAndWait();
                });
                thread.Start();
            }
            
            barrier.SignalAndWait();
            return totalSum;
        }

        public static double SumWithTPL(List<double> numbers, int batchesCount)
        {
            int batchCount = numbers.Count / batchesCount + 1;

            double totalSum = 0;
            object lockObject = new object();
            Parallel.For(0, batchesCount, (index) =>
            {
                double batchSum = GetSumFromBatch(batchCount, index, numbers);
                lock (lockObject)
                {
                    totalSum += batchSum;
                }
            });
            return totalSum;
        }

        public static double SumWithPLINQ(List<double> numbers)
        {
            double totalSum = numbers.AsParallel().Sum();
            return totalSum;
        }

        private static double GetSumFromBatch(int batchCount, int batchIndex, List<double> numbers) 
        {
            int startIndex = batchIndex * batchCount;
            int endIndex = startIndex + batchCount;
            if (endIndex > numbers.Count)
                endIndex = numbers.Count;

            double batchSum = 0;
            for (int j = startIndex; j < endIndex; j++)
            {
                batchSum += numbers[j];
            }

            return batchSum;
        }
    }
}
