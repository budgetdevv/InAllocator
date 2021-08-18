using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Inside.InAllocator;


namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Test1();
            
            Test2();
        }

        private static void Test1()
        {
            var Before = Test1_Sub();
            
            var After = GC.GetTotalMemory(true);
            
            Console.WriteLine($"After De-allocation | {After}");
            
            Console.WriteLine($"Memory Freed - {Before - After}");
        }
        
        private static long Test1_Sub()
        {
            var Allocator = new InAllocator();

            Allocator.Allocate<string>(1, out var Memory);

            ref var String = ref Memory[0];

            String = "Trump";

            Console.WriteLine(Memory[0]);

            String = "McDonaldz";

            Console.WriteLine(Memory[0]);

            Memory.UnsafeAs<ulong>(out var NMem);

            ref var Num = ref NMem[0];

            Num = 69;

            Console.WriteLine(NMem[0]);

            Num = 1258;

            Console.WriteLine(NMem[0]);

            Allocator.UnsafeRecycle(in Memory);

            Memory = default;

            NMem = default;

            Allocator.Allocate<ulong>(1, out NMem);

            Console.WriteLine(NMem[0]);

            NMem = default;

            var Before = GC.GetTotalMemory(false);

            Console.WriteLine($"Before De-allocation | {Before}");

            //Control

            Console.WriteLine($"Control | {GC.GetTotalMemory(false)}");

            //Force it to collect the Allocator ( And its underlying memory )

            Allocator = null;

            return Before;
        }

        public struct Yes
        {
            public string x;
            
            public int y;
        }

        public struct No
        {
            public int A, B, C, D, E, F, G;
        }
        
        private static void Test2()
        {
            var Allocator = new InAllocator();

            Allocator.Allocate<Yes>(1, out var Memory);

            ref var x = ref Memory[0];

            x.x = "TrumpMcDonaldz";

            x.y = 1258;
            
            Allocator.UnsafeRecycle(in Memory);
            
            Allocator.Allocate<Yes>(1, out var Memory2);
            
            ref var y = ref Memory2[0];
            
            Console.WriteLine(y.x);
            
            Console.WriteLine(y.y);
            
            Memory2.UnsafeAs<No>(out var Memory3);

            //Memory2.As<No>(out var Memory3);
            
            ref var z = ref Memory3[0];
            
            z.F = 69;

            z.G = 69;
            
            Console.WriteLine(z.A);
            
            Console.WriteLine(z.B);

            Console.WriteLine(z.C);

            Console.WriteLine(z.D);
            
            Console.WriteLine(z.E);

            Console.WriteLine(z.F);

            Console.WriteLine(z.G);

            Console.WriteLine(y.x);
        }
    }
}