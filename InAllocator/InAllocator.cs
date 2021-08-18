using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Inside.InCollections;

// ReSharper disable once CheckNamespace
namespace Inside.Allocators
{
    public readonly unsafe struct InMemory<T>
    {
        public readonly void* Allocation;

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
        public InMemory(void* allocation, int exp)
        {
            Allocation = allocation;

            Exp = exp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InMemory(nuint Address, int exp) : this((void*)Address, exp)
        {
            //Nothing here!
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
        public void As<F>(out InMemory<F> Memory)
        {
            Clear();

            UnsafeAs(out Memory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeAs<F>(out InMemory<F> Memory)
        {
            Memory = new InMemory<F>(Allocation, Exp);
        }

        public interface IIterateJob
        {
            public void Execute(ref T Item);
        }

        public void UnsafeIterateJob<TJob>(ref TJob Job, int StartIndex, int Count) where TJob : struct, IIterateJob
        {
            ref var Current = ref this[StartIndex];

            ref var LastOffsetByOne = ref this[Count];

            while (!Unsafe.AreSame(ref Current, ref LastOffsetByOne))
            {
                Job.Execute(ref Current);

                Current = ref Unsafe.Add(ref Current, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return AsSpan(Size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan(int Count)
        {
            return AsSpan(0, Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan(int StartIndex, int Count)
        {
            return MemoryMarshal.CreateSpan(ref this[StartIndex], Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return AsReadOnlySpan(Size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> AsReadOnlySpan(int Count)
        {
            return AsReadOnlySpan(0, Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> AsReadOnlySpan(int StartIndex, int Count)
        {
            return MemoryMarshal.CreateReadOnlySpan(ref this[StartIndex], Count);
        }
    }
    
    public sealed class InAllocator: IDisposable
    {
        private struct Slab
        {
            private InQueue<nuint> FreeMemory;

            private MemoryBlock Block;

            public Slab(int AllocationSize)
            {
                FreeMemory = new InQueue<nuint>(25);

                Block = new MemoryBlock(AllocationSize);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Allocate<T>(int Exp, int AllocationSize, InAllocator Allocator, out InMemory<T> Memory)
                
            {
                if (FreeMemory.TryDequeue(out var FreeAddress))
                {
                    Memory = new InMemory<T>(FreeAddress, Exp);
                }

                else
                {
                    Block.Allocate(Exp, AllocationSize, Allocator, out Memory);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AllocateNew<T>(int Exp, int AllocationSize, InAllocator Allocator, out InMemory<T> Memory)
            {
                Block.Allocate(Exp, AllocationSize, Allocator, out Memory);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe void Recycle<T>(in InMemory<T> Memory)
                
            {
                FreeMemory.Enqueue((nuint) Memory.Allocation);
            }
        }
        
        private readonly Slab[] Slabs;
        
        private InQueue<MemoryBlock> AdditionalMemoryBlocks;
        
        public InAllocator(int AllocationSizeInBytes = 85_000)
        {
            if (AllocationSizeInBytes >= 85_000)
            {
                Slabs = new Slab[31];

                var memoryBlocks = Slabs; //Local var to skip bound checks
                
                AdditionalMemoryBlocks = new InQueue<MemoryBlock>(5);
                
                for (int Exp = 0; Exp < memoryBlocks.Length; Exp++)
                {
                    memoryBlocks[Exp] = new Slab(AllocationSizeInBytes);
                }
            }

            else
            {
                throw new Exception("AllocationSize must at least be 85_000!");
            }
        }
        
        private unsafe struct MemoryBlock
        {
            public readonly object[] AllocatedMemory;

            public readonly void* MemoryPtr;

            public readonly int MemoryBlockSize;
            
            public int AllocationIndex, NextBlockIndex;
            
            public MemoryBlock(int allocationSize)
            {
                MemoryBlockSize = allocationSize;
                
                AllocatedMemory = new object[MemoryBlockSize];

                MemoryPtr = Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(AllocatedMemory));
                
                AllocationIndex = 0;

                NextBlockIndex = -1;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public void Allocate<T>(int Exp, int allocationSize, InAllocator Allocator, out InMemory<T> Memory)
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

            private void AllocateSlow<T>(int Exp, int allocationSize, InAllocator Allocator, out InMemory<T> Memory)
            {
                if (NextBlockIndex != -1)
                {
                    ref var Next = ref Allocator.AdditionalMemoryBlocks[NextBlockIndex];
                    
                    Next.Allocate(Exp, allocationSize, Allocator, out Memory);
                }

                else
                {
                    ref var Blocks = ref Allocator.AdditionalMemoryBlocks;

                    NextBlockIndex = Blocks.Count;

                    var NewBlock = new MemoryBlock(MemoryBlockSize);
                    
                    NewBlock.UnsafeAllocate(Exp, allocationSize, out Memory);
                    
                    Blocks.Enqueue(ref NewBlock);
                }
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void UnsafeAllocate<T>(int Exp, int allocationSize, out InMemory<T> Memory)
            {
                Memory = new InMemory<T>(MemoryPtr, Exp);

                AllocationIndex += allocationSize;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Allocate<T>(int Size, out InMemory<T> Memory)
        {
            AllocateByExp(GetExpByAllocationSize(Size), Size, out Memory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AllocateByExp<T>(int Exp, int Size, out InMemory<T> Memory)
        {
            ref var Slab = ref GetSlabByExp(Exp);
            
            Slab.Allocate(Exp, Size, this, out Memory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Slab GetSlabByExp(int Exp)
        {
            return ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(Slabs), Exp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Recycle<T>(in InMemory<T> Memory)
        {
            Memory.Clear();
            
            UnsafeRecycle(in Memory);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeRecycle<T>(in InMemory<T> Memory)
        {
            ref var Slab = ref GetSlabByExp(Memory.Exp);
            
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