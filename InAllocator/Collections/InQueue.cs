using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Inside.InCollections
{
    public struct InQueue<T>
    {
        private T[] Arr;

        private int ReadPos;

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => unchecked(ReadPos + 1);
        }

        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Arr.Length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InQueue(int InitialSize)
        {
            Arr = AllocateNew(InitialSize);

            ReadPos = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[] AllocateNew(int Size)
        {
            #if NET5_0_OR_GREATER

            return GC.AllocateUninitializedArray<T>(Size);

            #else
            
            return new T[Size];
            
            #endif
        }

        public ref T this[int Index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(Arr), Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(T Item)
        {
            Enqueue(ref Item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(ref T Item) //Improvement: https://shorturl.at/jwI35
        {
            var arr = Arr;
            
            var WritePos = unchecked(++ReadPos);
        
            if ((uint) WritePos < (uint) arr.Length)
            {
                arr[WritePos] = Item;
            }

            else
            {
                ResizeAndAdd(ref Item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeEnqueue(ref T Item)
        {
            this[unchecked(++ReadPos)] = Item;
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ResizeAndAdd(ref T Item)
        {
            var NewArr = AllocateNew(unchecked(Arr.Length * 2));
            
            Arr.AsSpan().CopyTo(NewArr);

            Arr = NewArr;
            
            this[ReadPos] = Item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDequeue(out T Item)
        {
            if (ReadPos != -1)
            {
                Item = this[unchecked(ReadPos--)];

                return true;
            }

            Unsafe.SkipInit(out Item);
            
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            ReadPos = -1;
        }
    }
}