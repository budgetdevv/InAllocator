# InAllocator
Experimental allocator that shares memory across all types!

# WARNING!
It HAS the potential to BREAK, as it relies on the fact that the LOH ( Large Object Heap ) doesn't compact stuff unless explicitly told to.
Compacting the LOH will cause corruptions here and there! This allocator is also NOT production-ready, so use it at your own risk!

# APIs

Allocator.Allocate<T>(int Size, out InsideMemory<T> Memory) -> Allocates memory capable of containing Size amount of elements.

ref var Element = ref Memory[int Index] -> Gets a reference to element at Index.

Memory.Clear() -> Zeroes the Memory

Memory.As<F>(out InsideMemory<F> Memory) -> Zeroes the Memory, and reinterprets it to type F.

Memory.UnsafeAs<F>(out InsideMemory<F> Memory) -> Reinterprets Memory to type F without Zeroing. Can cause issues if there's an attempt to
de-reference underlying type that is NOT F. The obvious solution is to overwrite with content of type F.

Allocator.Recycle<T>(in InsideMemory<T> Memory) -> Recycles the Memory back into the Allocator. The Memory is zeroed beforehand.

Allocator.UnsafeRecycle<T>(in InsideMemory<T> Memory) -> Recycles the Memory back into the Allocator without zeroing its contents.

# Notes

- Nulling the Allocator will cause it to go out of scope. That means, the GC may reclaim ALL memory allocated by the Allocator.

- GCSettings.LargeObjectHeapCompactionMode.CompactOnce @ https://docs.microsoft.com/en-us/dotnet/api/system.runtime.gclargeobjectheapcompactionmode?view=net-5.0#System_Runtime_GCLargeObjectHeapCompactionMode_Default can cause undefined behavior.

- You shouldn't be using this shit at all because its only a day old
