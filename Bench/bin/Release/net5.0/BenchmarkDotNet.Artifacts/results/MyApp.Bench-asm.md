## .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
```assembly
; MyApp.Bench.IntMem_Write()
       mov       rax,1C41A9072A0
       mov       rax,[rax]
       mov       rax,[rax+8]
       mov       dword ptr [rax+114],45
       ret
; Total bytes of code 28
```

## .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
```assembly
; MyApp.Bench.IntArr_Write()
       sub       rsp,28
       mov       rax,1D3180072B8
       mov       rax,[rax]
       cmp       dword ptr [rax+8],45
       jbe       short M00_L00
       mov       dword ptr [rax+124],45
       add       rsp,28
       ret
M00_L00:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 44
```

## .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
```assembly
; MyApp.Bench.InsideAllocator()
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,40
       vxorps    xmm4,xmm4,xmm4
       vmovdqu   xmmword ptr [rsp+28],xmm4
       xor       eax,eax
       mov       [rsp+38],rax
       mov       rcx,1AEF03072B0
       mov       rsi,[rcx]
       mov       ecx,1F
       lzcnt     ecx,ecx
       mov       edi,ecx
       neg       edi
       add       edi,20
       mov       rcx,[rsi+8]
       cmp       edi,[rcx+8]
       jae       near ptr M00_L02
       movsxd    rdx,edi
       lea       rdx,[rdx+rdx*4]
       lea       rbx,[rcx+rdx*8+10]
       mov       rcx,[rbx]
       lea       rdx,[rsp+28]
       cmp       [rcx],ecx
       call      System.Collections.Generic.Queue`1[[System.UIntPtr, System.Private.CoreLib]].TryDequeue(UIntPtr ByRef)
       test      eax,eax
       je        short M00_L00
       mov       rcx,[rsp+28]
       mov       [rsp+30],rcx
       mov       [rsp+38],edi
       jmp       short M00_L01
M00_L00:
       lea       rcx,[rsp+30]
       mov       [rsp+20],rcx
       cmp       [rbx],ebx
       lea       rcx,[rbx+8]
       mov       edx,edi
       mov       r9,rsi
       mov       r8d,20
       call      Inside.InAllocator.InAllocator+MemoryBlock.Allocate[[System.Int32, System.Private.CoreLib]](Int32, Int32, Inside.InAllocator.InAllocator, InsideMemory`1<Int32> ByRef)
M00_L01:
       mov       rcx,1AEF03072B0
       mov       rcx,[rcx]
       mov       rcx,[rcx+8]
       mov       edx,[rcx+8]
       cmp       [rsp+38],edx
       jae       short M00_L02
       mov       edx,[rsp+38]
       movsxd    rdx,edx
       lea       rdx,[rdx+rdx*4]
       lea       rcx,[rcx+rdx*8+10]
       mov       rcx,[rcx]
       mov       rdx,[rsp+30]
       cmp       [rcx],ecx
       call      System.Collections.Generic.Queue`1[[System.UIntPtr, System.Private.CoreLib]].Enqueue(UIntPtr)
       nop
       add       rsp,40
       pop       rbx
       pop       rsi
       pop       rdi
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 217
```
```assembly
; System.Collections.Generic.Queue`1[[System.UIntPtr, System.Private.CoreLib]].TryDequeue(UIntPtr ByRef)
       sub       rsp,28
       mov       r8d,[rcx+10]
       mov       r9,[rcx+8]
       cmp       dword ptr [rcx+18],0
       jne       short M01_L00
       xor       eax,eax
       mov       [rdx],rax
       add       rsp,28
       ret
M01_L00:
       cmp       r8d,[r9+8]
       jae       short M01_L02
       movsxd    rax,r8d
       mov       rax,[r9+rax*8+10]
       mov       [rdx],rax
       lea       rax,[rcx+10]
       mov       edx,[rax]
       inc       edx
       mov       r8,[rcx+8]
       cmp       [r8+8],edx
       jne       short M01_L01
       xor       edx,edx
M01_L01:
       mov       [rax],edx
       dec       dword ptr [rcx+18]
       inc       dword ptr [rcx+1C]
       mov       eax,1
       add       rsp,28
       ret
M01_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 89
```
```assembly
; Inside.InAllocator.InAllocator+MemoryBlock.Allocate[[System.Int32, System.Private.CoreLib]](Int32, Int32, Inside.InAllocator.InAllocator, InsideMemory`1<Int32> ByRef)
       push      rbp
       sub       rsp,30
       lea       rbp,[rsp+30]
       mov       [rbp+10],rcx
       mov       [rbp+18],edx
       mov       [rbp+20],r8d
       mov       [rbp+28],r9
       mov       rcx,[rbp+10]
       mov       ecx,[rcx+14]
       add       ecx,[rbp+20]
       mov       rdx,[rbp+10]
       cmp       ecx,[rdx+10]
       jge       short M02_L00
       mov       rcx,[rbp+10]
       mov       edx,[rbp+18]
       mov       r8d,[rbp+20]
       mov       r9,[rbp+30]
       call      Inside.InAllocator.InAllocator+MemoryBlock.UnsafeAllocate[[System.Int32, System.Private.CoreLib]](Int32, Int32, InsideMemory`1<Int32> ByRef)
       nop
       lea       rsp,[rbp]
       pop       rbp
       ret
M02_L00:
       mov       rcx,[rbp+30]
       mov       [rsp+20],rcx
       mov       rcx,[rbp+10]
       mov       edx,[rbp+18]
       mov       r8d,[rbp+20]
       mov       r9,[rbp+28]
       call      Inside.InAllocator.InAllocator+MemoryBlock.AllocateSlow[[System.Int32, System.Private.CoreLib]](Int32, Int32, Inside.InAllocator.InAllocator, InsideMemory`1<Int32> ByRef)
       nop
       lea       rsp,[rbp]
       pop       rbp
       ret
; Total bytes of code 107
```
```assembly
; System.Collections.Generic.Queue`1[[System.UIntPtr, System.Private.CoreLib]].Enqueue(UIntPtr)
       push      rdi
       push      rsi
       sub       rsp,28
       mov       rsi,rcx
       mov       rdi,rdx
       mov       eax,[rsi+18]
       mov       rcx,[rsi+8]
       cmp       eax,[rcx+8]
       jne       short M03_L01
       mov       eax,[rcx+8]
       movsxd    rax,eax
       imul      r8,rax,0C8
       mov       rdx,0A3D70A3D70B
       mov       rax,rdx
       imul      r8
       add       rdx,r8
       mov       rax,rdx
       shr       rax,3F
       sar       rdx,6
       add       rdx,rax
       mov       eax,[rcx+8]
       add       eax,4
       cmp       eax,edx
       jle       short M03_L00
       mov       edx,[rcx+8]
       add       edx,4
M03_L00:
       mov       rcx,rsi
       call      System.Collections.Generic.Queue`1[[System.UIntPtr, System.Private.CoreLib]].SetCapacity(Int32)
M03_L01:
       mov       rax,[rsi+8]
       mov       rdx,rax
       mov       ecx,[rsi+14]
       cmp       ecx,[rdx+8]
       jae       short M03_L03
       movsxd    rcx,ecx
       mov       [rdx+rcx*8+10],rdi
       lea       rdx,[rsi+14]
       mov       ecx,[rdx]
       inc       ecx
       cmp       [rax+8],ecx
       jne       short M03_L02
       xor       ecx,ecx
M03_L02:
       mov       [rdx],ecx
       inc       dword ptr [rsi+18]
       inc       dword ptr [rsi+1C]
       add       rsp,28
       pop       rsi
       pop       rdi
       ret
M03_L03:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 153
```

## .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
```assembly
; MyApp.Bench.ArrayPool()
       push      rsi
       sub       rsp,20
       mov       rcx,116A1C972A8
       mov       rcx,[rcx]
       mov       rsi,rcx
       mov       edx,20
       call      qword ptr [7FF9E2127510]
       mov       rdx,rax
       mov       rcx,rsi
       xor       r8d,r8d
       mov       rax,[7FF9E2127518]
       cmp       [rcx],ecx
       add       rsp,20
       pop       rsi
       jmp       rax
; Total bytes of code 58
```
```assembly
; System.Buffers.TlsOverPerCoreLockedStacksArrayPool`1[[System.Byte, System.Private.CoreLib]].Rent(Int32)
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,48
       xor       eax,eax
       mov       [rsp+40],rax
       mov       [rsp+38],rax
       mov       rsi,rcx
       mov       edi,edx
       test      edi,edi
       jl        near ptr M01_L21
       test      edi,edi
       jne       short M01_L00
       mov       rax,116A1C92D38
       mov       rax,[rax]
       add       rsp,48
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       ret
M01_L00:
       mov       rcx,116A1C91718
       mov       rbx,[rcx]
       lea       ecx,[rdi+0FFFF]
       or        ecx,0F
       or        ecx,1
       xor       ebp,ebp
       lzcnt     ebp,ecx
       xor       ebp,1F
       add       ebp,0FFFFFFFD
       mov       rcx,[rsi+10]
       cmp       [rcx+8],ebp
       jle       near ptr M01_L15
       mov       rcx,7FF9E1E80020
       mov       edx,0D
       call      CORINFO_HELP_GETSHARED_GCTHREADSTATIC_BASE_DYNAMICCLASS
       mov       rcx,[rax]
       test      rcx,rcx
       je        short M01_L02
       cmp       ebp,[rcx+8]
       jae       near ptr M01_L22
       movsxd    rax,ebp
       mov       r14,[rcx+rax*8+10]
       test      r14,r14
       je        short M01_L02
       movsxd    rax,ebp
       xor       edx,edx
       mov       [rcx+rax*8+10],rdx
       cmp       byte ptr [rbx+9D],0
       je        short M01_L01
       mov       rcx,r14
       call      00007FFA4192B980
       mov       edi,eax
       mov       r15d,[r14+8]
       mov       rcx,rsi
       call      00007FFA4192B980
       mov       r9d,eax
       mov       [rsp+20],ebp
       mov       edx,edi
       mov       r8d,r15d
       mov       rcx,rbx
       call      System.Buffers.ArrayPoolEventSource.BufferRented(Int32, Int32, Int32, Int32)
M01_L01:
       mov       rax,r14
       add       rsp,48
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       ret
M01_L02:
       mov       rcx,[rsi+10]
       cmp       ebp,[rcx+8]
       jae       near ptr M01_L22
       movsxd    rdx,ebp
       mov       rcx,[rcx+rdx*8+10]
       test      rcx,rcx
       je        near ptr M01_L12
       mov       rdi,[rcx+8]
       mov       rcx,7FF9E1E80020
       mov       edx,26E
       call      CORINFO_HELP_GETSHARED_NONGCTHREADSTATIC_BASE
       mov       edx,[rax+874]
       lea       ecx,[rdx+0FFFF]
       mov       [rax+874],ecx
       test      edx,0FFFF
       jne       short M01_L03
       call      System.Threading.ProcessorIdCache.RefreshCurrentProcessorId()
       jmp       short M01_L04
M01_L03:
       mov       eax,edx
       sar       eax,10
M01_L04:
       mov       r14d,[rdi+8]
       cdq
       idiv      r14d
       mov       r15d,edx
       xor       r12d,r12d
       test      r14d,r14d
       jle       near ptr M01_L08
M01_L05:
       cmp       r15d,r14d
       jae       near ptr M01_L22
       movsxd    rcx,r15d
       mov       r13,[rdi+rcx*8+10]
       cmp       [r13],r13d
       xor       eax,eax
       mov       [rsp+30],rax
       mov       rcx,r13
       call      System.Threading.Monitor.Enter(System.Object)
       cmp       dword ptr [r13+10],0
       jle       short M01_L06
       mov       rcx,[r13+8]
       mov       rax,rcx
       mov       edx,[r13+10]
       dec       edx
       mov       r8d,edx
       mov       [r13+10],r8d
       mov       r9d,[rax+8]
       cmp       r8d,r9d
       jae       near ptr M01_L22
       movsxd    r8,r8d
       mov       rax,[rax+r8*8+10]
       mov       [rsp+30],rax
       cmp       edx,r9d
       jae       near ptr M01_L22
       xor       edx,edx
       mov       [rcx+r8*8+10],rdx
M01_L06:
       mov       rcx,r13
       call      00007FFA4196B1D0
       mov       r13,[rsp+30]
       test      r13,r13
       jne       short M01_L09
       inc       r15d
       cmp       r14d,r15d
       jne       short M01_L07
       xor       r15d,r15d
M01_L07:
       inc       r12d
       cmp       r14d,r12d
       jg        near ptr M01_L05
M01_L08:
       xor       r14d,r14d
       jmp       short M01_L10
M01_L09:
       mov       r14,r13
M01_L10:
       test      r14,r14
       je        short M01_L12
       cmp       byte ptr [rbx+9D],0
       je        short M01_L11
       mov       rcx,r14
       call      00007FFA4192B980
       mov       edi,eax
       mov       r15d,[r14+8]
       mov       rcx,rsi
       call      00007FFA4192B980
       mov       r9d,eax
       mov       [rsp+20],ebp
       mov       edx,edi
       mov       r8d,r15d
       mov       rcx,rbx
       call      System.Buffers.ArrayPoolEventSource.BufferRented(Int32, Int32, Int32, Int32)
M01_L11:
       mov       rax,r14
       add       rsp,48
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       ret
M01_L12:
       mov       rdx,[rsi+8]
       cmp       ebp,[rdx+8]
       jae       near ptr M01_L22
       movsxd    rcx,ebp
       mov       edi,[rdx+rcx*4+10]
       cmp       edi,800
       jge       short M01_L13
       movsxd    rdx,edi
       mov       rcx,offset MT_System.Byte[]
       call      CORINFO_HELP_NEWARR_1_VC
       mov       r14,rax
       jmp       short M01_L14
M01_L13:
       mov       rcx,offset MT_System.Byte[]
       call      CORINFO_HELP_TYPEHANDLE_TO_RUNTIMETYPE
       mov       [rsp+40],rax
       lea       rcx,[rsp+40]
       call      System.RuntimeTypeHandle.get_Value()
       mov       rcx,rax
       mov       edx,edi
       mov       r8d,10
       call      System.GC.AllocateNewArray(IntPtr, Int32, GC_ALLOC_FLAGS)
       mov       r14,rax
M01_L14:
       jmp       short M01_L17
M01_L15:
       cmp       edi,800
       jge       short M01_L16
       movsxd    rdx,edi
       mov       rcx,offset MT_System.Byte[]
       call      CORINFO_HELP_NEWARR_1_VC
       mov       r14,rax
       jmp       short M01_L17
M01_L16:
       mov       rcx,offset MT_System.Byte[]
       call      CORINFO_HELP_TYPEHANDLE_TO_RUNTIMETYPE
       mov       [rsp+38],rax
       lea       rcx,[rsp+38]
       call      System.RuntimeTypeHandle.get_Value()
       mov       rcx,rax
       mov       edx,edi
       mov       r8d,10
       call      System.GC.AllocateNewArray(IntPtr, Int32, GC_ALLOC_FLAGS)
       mov       r14,rax
M01_L17:
       cmp       byte ptr [rbx+9D],0
       je        short M01_L20
       cmp       [r14],r14d
       mov       rcx,r14
       call      00007FFA4192B980
       mov       edi,eax
       mov       r15d,[r14+8]
       mov       rcx,rsi
       call      00007FFA4192B980
       mov       r9d,eax
       mov       dword ptr [rsp+20],0FFFFFFFF
       mov       edx,edi
       mov       r8d,r15d
       mov       rcx,rbx
       call      System.Buffers.ArrayPoolEventSource.BufferRented(Int32, Int32, Int32, Int32)
       mov       r15d,[r14+8]
       mov       rcx,rsi
       call      00007FFA4192B980
       mov       rcx,rbx
       mov       edx,edi
       mov       r8d,r15d
       mov       r9d,eax
       mov       rax,[rsi+10]
       cmp       [rax+8],ebp
       jle       short M01_L18
       mov       eax,2
       jmp       short M01_L19
M01_L18:
       mov       eax,1
M01_L19:
       mov       dword ptr [rsp+20],0FFFFFFFF
       mov       [rsp+28],eax
       call      System.Buffers.ArrayPoolEventSource.BufferAllocated(Int32, Int32, Int32, Int32, BufferAllocatedReason)
M01_L20:
       mov       rax,r14
       add       rsp,48
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       ret
M01_L21:
       mov       rcx,offset MT_System.ArgumentOutOfRangeException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       ecx,19C0D
       mov       rdx,7FF9E1D84020
       call      CORINFO_HELP_STRCNS
       mov       rdx,rax
       mov       rcx,rsi
       call      System.ArgumentOutOfRangeException..ctor(System.String)
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
M01_L22:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 980
```
```assembly
; System.Buffers.TlsOverPerCoreLockedStacksArrayPool`1[[System.Byte, System.Private.CoreLib]].Return(Byte[], Boolean)
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,38
       mov       rdi,rcx
       mov       rsi,rdx
       test      rsi,rsi
       je        near ptr M02_L11
       mov       ecx,[rsi+8]
       dec       ecx
       or        ecx,0F
       or        ecx,1
       xor       ebx,ebx
       lzcnt     ebx,ecx
       xor       ebx,1F
       add       ebx,0FFFFFFFD
       mov       rcx,[rdi+10]
       cmp       [rcx+8],ebx
       jle       near ptr M02_L09
       test      r8b,r8b
       je        short M02_L00
       mov       r8d,[rsi+8]
       mov       rcx,rsi
       xor       edx,edx
       call      System.Array.Clear(System.Array, Int32, Int32)
M02_L00:
       mov       ecx,[rsi+8]
       mov       rdx,[rdi+8]
       cmp       ebx,[rdx+8]
       jae       near ptr M02_L13
       movsxd    rax,ebx
       cmp       ecx,[rdx+rax*4+10]
       jne       near ptr M02_L12
       mov       rcx,7FF9E1E80020
       mov       edx,0D
       call      CORINFO_HELP_GETSHARED_GCTHREADSTATIC_BASE_DYNAMICCLASS
       mov       rbp,rax
       mov       rcx,[rbp]
       test      rcx,rcx
       jne       near ptr M02_L01
       mov       rcx,offset MT_System.Byte[][]
       mov       edx,11
       call      CORINFO_HELP_NEWARR_1_OBJ
       mov       r14,rax
       mov       rcx,rbp
       mov       rdx,r14
       call      CORINFO_HELP_ASSIGN_REF
       mov       rcx,r14
       mov       edx,ebx
       mov       r8,rsi
       call      CORINFO_HELP_ARRADDR_ST
       mov       rcx,116A1C92D68
       mov       rcx,[rcx]
       mov       rdx,r14
       xor       r8d,r8d
       cmp       [rcx],ecx
       call      System.Runtime.CompilerServices.ConditionalWeakTable`2[[System.__Canon, System.Private.CoreLib],[System.__Canon, System.Private.CoreLib]].Add(System.__Canon, System.__Canon)
       lea       rcx,[rdi+18]
       mov       eax,1
       xchg      eax,[rcx]
       cmp       eax,1
       je        near ptr M02_L09
       mov       rcx,offset MT_System.Func`2[[System.Object, System.Private.CoreLib],[System.Boolean, System.Private.CoreLib]]
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       lea       rcx,[rbx+8]
       mov       rdx,rbx
       call      CORINFO_HELP_ASSIGN_REF
       mov       rcx,7FF9E1D8D150
       mov       [rbx+18],rcx
       mov       rcx,offset System.Buffers.TlsOverPerCoreLockedStacksArrayPool`1[[System.Byte, System.Private.CoreLib]].Gen2GcCallbackFunc(System.Object)
       mov       [rbx+20],rcx
       mov       rcx,offset MT_System.Gen2GcCallback
       call      CORINFO_HELP_NEWFAST
       mov       rbp,rax
       lea       rcx,[rbp+10]
       mov       rdx,rbx
       call      CORINFO_HELP_ASSIGN_REF
       xor       ecx,ecx
       mov       [rsp+30],rcx
       lea       rcx,[rsp+30]
       mov       rdx,rdi
       xor       r8d,r8d
       call      System.Runtime.InteropServices.GCHandle..ctor(System.Object, System.Runtime.InteropServices.GCHandleType)
       mov       rcx,[rsp+30]
       mov       [rbp+18],rcx
       jmp       near ptr M02_L09
M02_L01:
       cmp       ebx,[rcx+8]
       jae       near ptr M02_L13
       movsxd    rdx,ebx
       mov       rbp,[rcx+rdx*8+10]
       mov       edx,ebx
       mov       r8,rsi
       call      CORINFO_HELP_ARRADDR_ST
       test      rbp,rbp
       je        near ptr M02_L09
       mov       rcx,[rdi+10]
       cmp       ebx,[rcx+8]
       jae       near ptr M02_L13
       movsxd    rdx,ebx
       mov       rax,[rcx+rdx*8+10]
       test      rax,rax
       jne       short M02_L02
       mov       rcx,rdi
       mov       edx,ebx
       call      System.Buffers.TlsOverPerCoreLockedStacksArrayPool`1[[System.Byte, System.Private.CoreLib]].CreatePerCoreLockedStacks(Int32)
M02_L02:
       mov       rbx,[rax+8]
       mov       rcx,7FF9E1E80020
       mov       edx,26E
       call      CORINFO_HELP_GETSHARED_NONGCTHREADSTATIC_BASE
       mov       edx,[rax+874]
       lea       ecx,[rdx+0FFFF]
       mov       [rax+874],ecx
       test      edx,0FFFF
       jne       short M02_L03
       call      System.Threading.ProcessorIdCache.RefreshCurrentProcessorId()
       jmp       short M02_L04
M02_L03:
       mov       eax,edx
       sar       eax,10
M02_L04:
       mov       r14d,[rbx+8]
       cdq
       idiv      r14d
       mov       r15d,edx
       xor       r12d,r12d
       test      r14d,r14d
       jle       near ptr M02_L09
M02_L05:
       cmp       r15d,r14d
       jae       near ptr M02_L13
       movsxd    rcx,r15d
       mov       r13,[rbx+rcx*8+10]
       cmp       [r13],r13d
       xor       eax,eax
       mov       [rsp+2C],eax
       mov       rcx,r13
       call      System.Threading.Monitor.Enter(System.Object)
       cmp       dword ptr [r13+10],8
       jge       short M02_L07
       cmp       dword ptr [r13+10],0
       jne       short M02_L06
       call      System.Environment.get_TickCount()
       mov       [r13+14],eax
M02_L06:
       mov       rcx,[r13+8]
       mov       edx,[r13+10]
       lea       r8d,[rdx+1]
       mov       [r13+10],r8d
       mov       r8,rbp
       call      CORINFO_HELP_ARRADDR_ST
       mov       dword ptr [rsp+2C],1
M02_L07:
       mov       rcx,r13
       call      00007FFA4196B1D0
       cmp       dword ptr [rsp+2C],0
       jne       short M02_L09
       inc       r15d
       cmp       r14d,r15d
       jne       short M02_L08
       xor       r15d,r15d
M02_L08:
       inc       r12d
       cmp       r14d,r12d
       jg        short M02_L05
M02_L09:
       mov       rcx,116A1C91718
       mov       rbx,[rcx]
       cmp       byte ptr [rbx+9D],0
       je        short M02_L10
       mov       rcx,rsi
       call      00007FFA4192B980
       mov       ebp,eax
       mov       esi,[rsi+8]
       mov       rcx,rdi
       call      00007FFA4192B980
       mov       [rsp+20],eax
       mov       rcx,rbx
       mov       r8d,ebp
       mov       r9d,esi
       mov       edx,3
       call      System.Diagnostics.Tracing.EventSource.WriteEvent(Int32, Int32, Int32, Int32)
M02_L10:
       nop
       add       rsp,38
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       ret
M02_L11:
       mov       rcx,offset MT_System.ArgumentNullException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       ecx,1DF
       mov       rdx,7FF9E1D84020
       call      CORINFO_HELP_STRCNS
       mov       rdx,rax
       mov       rcx,rsi
       call      System.ArgumentNullException..ctor(System.String)
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
M02_L12:
       mov       rcx,offset MT_System.ArgumentException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       ecx,8E5F
       mov       rdx,7FF9E1D84020
       call      CORINFO_HELP_STRCNS
       mov       rcx,rax
       xor       edx,edx
       call      System.SR.GetResourceString(System.String, System.String)
       mov       rdi,rax
       mov       ecx,1DF
       mov       rdx,7FF9E1D84020
       call      CORINFO_HELP_STRCNS
       mov       r8,rax
       mov       rdx,rdi
       mov       rcx,rsi
       call      System.ArgumentException..ctor(System.String, System.String)
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
M02_L13:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 888
```

