using System;
using System.Runtime.CompilerServices;
using Inside.InAllocator;


namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Test2();
            
            // var Before = Test1();
            //
            // var After = GC.GetTotalMemory(true);
            //
            // Console.WriteLine($"After De-allocation | {After}");
            //
            // Console.WriteLine($"Memory Freed - {Before - After}");
        }

        private static long Test1()
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

        private class Foo
        {
            private int Num;

            public Foo(int num)
            {
                Num = num;
            }

            public override string ToString()
            {
                return Num.ToString();
            }
        }
        
        private static void Test2()
        {
            var OArr = new object[10];

            var SArr = Unsafe.As<string[]>(OArr);

            SArr[0] = "Yes";

            Console.WriteLine(SArr[0]);

            var FArr = Unsafe.As<Foo[]>(OArr);

            FArr[0] = new Foo(69);
            
            Console.WriteLine(OArr[0]);
            
            //This can break, since its no longer a string
            
            Console.WriteLine(SArr[0]);
        }
    }
}