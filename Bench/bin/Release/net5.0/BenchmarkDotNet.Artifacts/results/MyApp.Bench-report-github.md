``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=6.0.100-preview.7.21377.35
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|          Method |       Mean |     Error |    StdDev |     Median | Code Size |
|---------------- |-----------:|----------:|----------:|-----------:|----------:|
|    IntMem_Write |  0.0002 ns | 0.0007 ns | 0.0006 ns |  0.0000 ns |      28 B |
|    IntArr_Write |  0.1704 ns | 0.0061 ns | 0.0057 ns |  0.1703 ns |      44 B |
| InsideAllocator |  7.7690 ns | 0.1304 ns | 0.1156 ns |  7.8041 ns |     566 B |
|       ArrayPool | 16.7067 ns | 0.1604 ns | 0.1500 ns | 16.6691 ns |    1926 B |
