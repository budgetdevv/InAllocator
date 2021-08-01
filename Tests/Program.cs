using System;
using Inside.InAllocator;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Allocator = new InAllocator();
            
            Allocator.Allocate<string>(10_000, out var Memory);
            
            for (int I = 0; I < Memory.Size; I++)
            {
                Memory[I] = "Trump";
            }
            
            Memory[Memory.Size - 1] = "McDonaldz";
            
            Console.WriteLine(Memory.Size);
            
            for (int I = 0; I < Memory.Size; I++)
            {
                Console.WriteLine(Memory[I]);
            }
            
            Memory.UnsafeAs<int>(out var MemN);
            
            //Memory.As<int>(out var MemN);
            
            MemN[0] = 1;
            
            MemN[1] = 2;
            
            MemN[2] = 3;
            
            for (int I = 0; I <= 2; I++)
            {
                Console.WriteLine(MemN[I]);
            }
            
            Allocator.UnsafeRecycle<int>(in MemN);
            
            MemN = default;
            
            Allocator.Allocate<int>(10_000, out MemN);
            
            for (int I = 0; I <= 2; I++)
            {
                Console.WriteLine(MemN[I]);
            }
        }
    }
}