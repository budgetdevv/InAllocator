using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Inside.InAllocator
{
    public sealed class InAllocator: IDisposable
    {
        private struct Slab
        {
            internal readonly Queue<nuint> FreeMemory;

            private MemoryBlock Block;

            public Slab(int AllocationSize)
            {
                FreeMemory = new Queue<nuint>(25);

                Block = new MemoryBlock(AllocationSize);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Allocate<T>(int Exp, int AllocationSize, InAllocator Allocator, out InsideMemory<T> Memory)
                //where T: class
            {
                if (FreeMemory.TryDequeue(out var FreeAddress))
                {
                    Memory = new InsideMemory<T>(FreeAddress, Exp);
                }

                else
                {
                    Block.Allocate(Exp, AllocationSize, Allocator, out Memory);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe void Recycle<T>(in InsideMemory<T> Memory)
                //where T: class
            {
                FreeMemory.Enqueue((nuint) Memory.Allocation);
            }
        }
        
        private readonly Slab[] Slabs;
        
        private readonly List<MemoryBlock> AdditionalMemoryBlocks;

        public InAllocator(int AllocationSize = 85_000)
        {
            if (AllocationSize >= 85_000)
            {
                Slabs = new Slab[31];

                var memoryBlocks = Slabs; //Local var to skip bound checks
            
                AdditionalMemoryBlocks = new List<MemoryBlock>(5);

                for (int Exp = 1; Exp < memoryBlocks.Length; Exp++)
                {
                    memoryBlocks[Exp] = new Slab(AllocationSize);
                }
            }

            else
            {
                throw new Exception("AllocationSize must at least be 85_000!");
            }
        }
        
        private unsafe struct MemoryBlock
        {
            public readonly object[] Memory;

            public readonly void* MemoryPtr;

            public readonly int MemoryBlockSize;
            
            public int AllocationIndex, NextBlockIndex;

            public MemoryBlock(int allocationSize)
            {
                MemoryBlockSize = allocationSize;
                
                Memory = new object[MemoryBlockSize];

                MemoryPtr = Unsafe.AsPointer(ref Memory[0]);

                AllocationIndex = 0;

                NextBlockIndex = -1;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public void Allocate<T>(int Exp, int allocationSize, InAllocator Allocator, out InsideMemory<T> Memory)
                //where T : class
            {
                if (unchecked(AllocationIndex + allocationSize) < MemoryBlockSize)
                {
                    UnsafeAllocate(Exp, allocationSize, out Memory);
                }

                else
                {
                    AllocateSlow(Exp, allocationSize, Allocator, out Memory);
                }
            }

            private void AllocateSlow<T>(int Exp, int allocationSize, InAllocator Allocator, out InsideMemory<T> Memory)
                //where T : class
            {
                if (NextBlockIndex != -1)
                {
                    //TODO: Write custom List<T> that can return ref
                    
                    ref var Next = ref Unsafe.Add(ref MemoryMarshal.GetReference(CollectionsMarshal.AsSpan(Allocator.AdditionalMemoryBlocks)), NextBlockIndex);

                    Next.Allocate(Exp, allocationSize, Allocator, out Memory);
                }

                else
                {
                    var Blocks = Allocator.AdditionalMemoryBlocks;
                    
                    NextBlockIndex = Blocks.Count;

                    var NewBlock = new MemoryBlock(MemoryBlockSize);
                    
                    NewBlock.UnsafeAllocate(Exp, allocationSize, out Memory);

                    Blocks.Add(NewBlock);
                }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void UnsafeAllocate<T>(int Exp, int allocationSize, out InsideMemory<T> Memory)
                //where T : class
            {
                Memory = new InsideMemory<T>(MemoryPtr, Exp);

                AllocationIndex += allocationSize;
            }
        }
        
        public readonly unsafe struct InsideMemory<T> //where T: class
        {
            internal readonly void* Allocation;

            internal readonly int Exp;
            
            public int Size
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => 1 << Exp;
            }

            public ref T this[int Index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.Add(ref Unsafe.AsRef<T>(Allocation), Index);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public InsideMemory(void* allocation, int exp)
            {
                Allocation = allocation;

                Exp = exp;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public InsideMemory(nuint Address, int exp)
            {
                Allocation = (void*) Address;

                Exp = exp;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Clear()
            {
                ref var Current = ref Unsafe.As<T, object>(ref this[0]);

                ref var LastOffsetByOne = ref Unsafe.As<T, object>(ref this[Size]);

                while (!Unsafe.AreSame(ref Current, ref LastOffsetByOne))
                {
                    Current = null;

                    Current = ref Unsafe.Add(ref Current, 1);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void As<F>(out InsideMemory<F> Memory)
                //where F: class
            {
                Clear();
                
                UnsafeAs(out Memory);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void UnsafeAs<F>(out InsideMemory<F> Memory)
            {
                Memory = new InsideMemory<F>(Allocation, Exp);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Allocate<T>(int Size, out InsideMemory<T> Memory)
            //where T : class
        {
            AllocateByExp(GetExpByAllocationSize(Size), Size, out Memory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AllocateByExp<T>(int Exp, int Size, out InsideMemory<T> Memory)
            //where T: class
        {
            //Console.WriteLine(Exp);
            
            ref var Slab = ref Slabs[Exp];
            
            Slab.Allocate(Exp, Size, this, out Memory);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Recycle<T>(in InsideMemory<T> Memory)
            //where T: class
        {
            Memory.Clear();
            
            UnsafeRecycle(in Memory);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeRecycle<T>(in InsideMemory<T> Memory)
            //where T: class
        {
            ref var Slab = ref Slabs[Memory.Exp];
            
            Slab.Recycle(in Memory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetExpByAllocationSize(int AllocationSize)
        {
            return GetNextSuperiorExpOf2(AllocationSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetNextSuperiorExpOf2(int Num)
        {
            return unchecked(32 - BitOperations.LeadingZeroCount((uint) (Num - 1))); //Note that the subtraction should take place before the cast to uint
        }

        public void Dispose()
        {
            Slabs.AsSpan().Fill(default);
            
            AdditionalMemoryBlocks.Clear();
        }
    }
}