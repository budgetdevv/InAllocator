using System.Buffers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using Inside.InAllocator;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Bench>();

            while (true)
            {
                
            }
        }
    }

    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true, exportDiff: true)]
    public class Bench
    {
        private static readonly ArrayPool<byte> AP;

        private static readonly InAllocator IA;

        private static readonly int[] IntArr;
        
        private static readonly InAllocator.InsideMemory<int> IntMem;

        static Bench()
        {
            AP = ArrayPool<byte>.Shared;

            IA = new InAllocator();

            var APArr = AP.Rent(32);

            IA.Allocate<byte>(32, out var Mem);

            AP.Return(APArr);

            IA.UnsafeRecycle(in Mem);

            IntArr = new int[100];
            
            IA.Allocate<int>(100, out IntMem);
        }

        [Benchmark]
        public void ArrayPool()
        {
            AP.Return(AP.Rent(32));
        }

        [Benchmark]
        public void InsideAllocator()
        {
            IA.Allocate<int>(32, out var Mem);

            IA.UnsafeRecycle(in Mem);
        }
        
        [Benchmark]
        public void IntArr_Write()
        {
            IntArr[69] = 69;

        }
        
        [Benchmark]
        public void IntMem_Write()
        {
            IntMem[69] = 69;
        }
    }
}