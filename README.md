# InAllocator
Experimental allocator that shares memory across all types!

# WARNING!
It HAS the potential to BREAK, as it relies on the fact that the LOH ( Large Object Heap ) doesn't compact stuff unless explicitly told to.
Compacting the LOH will cause corruptions here and there! This allocator is also NOT production-ready, so use it at your own risk!
