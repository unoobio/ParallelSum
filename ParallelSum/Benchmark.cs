using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ParallelSum
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Method)]
    [RankColumn]
    [SimpleJob(RunStrategy.Monitoring, warmupCount: 1, launchCount: 1, iterationCount: 5)]
    public class Benchmark
    {
        private List<double> NumbersCollection { get; set; }

        [Params(100000, 1000000, 10000000)]
        public int CollectionSize { get; set; }


        [GlobalSetup]
        public void Setup()
        {
            NumbersCollection = new List<double>();

            // Initialize the arrays with random values
            var rand = new Random();
            for (int i = 0; i < CollectionSize; i++)
            {
                NumbersCollection.Add(rand.Next(0, 1000));
            }
        }

        [Benchmark]
        public void SimpleSum()
        {
            SumCalculator.Sum(NumbersCollection);
        }

        [Benchmark]
        public void SumManualy()
        {
            SumCalculator.SumManualy(NumbersCollection);
        }

        [Benchmark]
        public void SumForeach()
        {
            SumCalculator.SumForeach(NumbersCollection);
        }

        [Benchmark]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(4)]
        [Arguments(8)]
        public void SumWithParallelLINQ(int batchesCount)
        {
            SumCalculator.SumWithParallelLINQ(NumbersCollection, batchesCount);
        }

        [Benchmark]
        [Arguments(2)]
        [Arguments(4)]
        [Arguments(8)]
        public void SumWithThread(int threadsCount)
        {
            SumCalculator.SumWithThreads(NumbersCollection, threadsCount);
        }
    }
}
