                        format MS64 COFF
                        section '.start' code readable executable
                        extrn kloader_start
                        public kloader_entry

                        use16
                        org 0x8000

kloader_entry:
                        xor ax, ax
                        mov ds, ax
                        mov es, ax
                        mov ss, ax
                        mov ax, 0x8000
                        mov sp, ax

                        cmp ebx, 0x7A53489D
                        jz .continue
                        mov ax, msg_error_loading_os
                        jmp write_msg_and_halt

.continue:                
                        call check_cpu

                        mov ax, message
                        call write_msg

                        ; switch to long mode

                        ; prepare page directory for 2MB paging

                        ; initialize PML4
                        mov bp, 0x1000
                        xor eax, eax
                        mov dword [bp], 0x2003 ; PML4[0] - low 32-bits (PDP address = 0x2000 and flags: writeable (bit 1) and present (bit 0))
                        mov [bp + 4], eax ; PML4[0] - high 32-bits

                        ; initialize PDP
                        mov bp, 0x2000
                        mov dword [bp], 0x3003 ; PDP[0] - low 32-bits (PD address = 0x3000 and flags: writeable (bit 1) and present (bit 0))
                        mov [bp + 4], eax ; PDP[0] - high 32-bits

                        ; initialize PD - first 8 MB of memory
                        mov bp, 0x3000
                        mov dword [bp], 0x83 ; PD[0] - low 32-bits (memory address = 0 MB, flags: writeable (bit 1) and present (bit 0) and page size (bit 7))
                        mov [bp + 4], eax ; PD[0] - high 32-bits
                        add bp, 8
                        mov dword [bp], 0x200000 or 0x83 ; PD[1] - low 32-bits (memory address = 2 MB, flags: writeable (bit 1) and present (bit 0) and page size (bit 7))
                        mov [bp + 4], eax ; PD[0] - high 32-bits
                        add bp, 8
                        mov dword [bp], 0x400000 or 0x83 ; PD[1] - low 32-bits (memory address = 4 MB, flags: writeable (bit 1) and present (bit 0) and page size (bit 7))
                        mov [bp + 4], eax ; PD[0] - high 32-bits
                        add bp, 8
                        mov dword [bp], 0x600000 or 0x83 ; PD[1] - low 32-bits (memory address = 6 MB, flags: writeable (bit 1) and present (bit 0) and page size (bit 7))
                        mov [bp + 4], eax ; PD[0] - high 32-bits

                        ; enable A20 line
                        call enable_A20

                        ; disable interrupts
                        cli

                        ; disable IRQs
                        mov al, 0xFF
                        out 0xA1, al
                        out 0x21, al
                        nop
                        nop

                        ; initialize IDT
                        lidt [IDT]

                        ; enter long mode
                        mov eax, 10100000b ; set the PAE and PGE bit
                        mov cr4, eax

                        mov edx, 0x1000 ; point CR3 at the PML4
                        mov cr3, edx

                        mov ecx, 0xC0000080 ; read from the EFER MSR
                        rdmsr
 
                        or eax, 0x00000100 ; set the LME bit
                        wrmsr

                        mov ebx, cr0 ; activate long mode -
                        or ebx, 0x80000001 ; - by enabling paging and protection simultaneously
                        mov cr0, ebx
                    
                        lgdt [GDT_pointer] ; load GDT_pointer defined below

                        code_segment = 8 ; code segment
                        jmp code_segment:long_mode_entry

; long mode structures
                        align 4
IDT:                    dw 0 ; length
                        dw 0 ; base
GDT:                    dq 0 ; null descriptor
                        dq 0x00209A0000000000 ; 64-bit code descriptor
                        dq 0x0000920000000000 ; 64-bit data descriptor
GDT_pointer:            dw $ - GDT - 1
                        dd GDT

; 16-bit mode routines
enable_A20:             ; enable A20
                        call check_A20
                        test ax, ax
                        jnz .exit

                        mov ax, msg1
                        call write_msg_and_halt

.exit:                  ret

msg1:                   db 'A20 line is disabled', 0

check_A20:              ; test if A20 is already enabled
                        ; 0000:0500 and FFFF:0510
                        mov ax, 0xFFFF
                        mov fs, ax
                        mov bp, 0x0500
                        mov bx, 0x0510
                        mov cl, [bp]
                        mov ch, [fs:bx]
                        mov byte [bp], 0
                        mov byte [fs:bx], 0xFF
                        
                        xor ax, ax
                        cmp byte [bp], 0xFF
                        mov [bp], cl
                        mov [fs:bx], ch

                        je .disabled
                        inc ax
.disabled:              ret                        

check_cpu:                ; check whether CPUID is supported or not.
                        pushfd
                        pop eax
                        mov ecx, eax
                        xor eax, 0x200000
                        push eax
                        popfd
                        pushfd
                        pop eax
                        xor eax, ecx
                        shr eax, 21
                        and eax, 1 ; Check whether bit 21 is set or not. If EAX now contains 0, CPUID isn't supported.
                        push ecx
                        popfd
                        test eax, eax
                        jz .no_long_mode

                        mov eax, 0x80000000
                        cpuid

                        cmp eax, 0x80000001 ; check whether extended function 0x80000001 is available are not.
                        jb .no_long_mode ; If not, long mode not supported.

                        mov eax, 0x80000001
                        cpuid                 
                        test edx, 1 shl 29 ; Test if the LM-bit, is set or not.
                        jz .no_long_mode ; If not Long mode not supported.
                        ret

.no_long_mode:          mov ax, msg_error_cpu_not_supp

write_msg_and_halt:     call write_msg
.halt:                  hlt
                        jmp .halt

write_msg:
                        mov si, ax
.load_char:             lodsb
                        and al, al
                        jnz .write_char
                        ret
.write_char:            mov bx, 0x0007
                        mov ah, 0x0E
                        int 0x10
                        jmp .load_char

message:                db 'Stage 2 loader starts!', 13, 10, 0
msg_error_loading_os:   db 'Error loading OS...', 0
msg_error_cpu_not_supp: db 'Error: CPU has no support for 64-bit mode...', 0

                        use64
long_mode_entry:        mov ax, 0x10 ; data segment
                        mov ds, ax
                        mov es, ax
                        mov fs, ax
                        mov gs, ax
                        mov ss, ax

                         ; blank out the screen to a black color
                        mov edi, 0xB8000
                        mov rcx, 500 ; since we are clearing uint64_t over here, we put the count as Count/4
                        mov rax, 0x0720072007200720 ; set the value to set the screen to: black background, white foreground, blank spaces
                        rep stosq ; clear the entire screen

                        ; display "Hello World!"
                        mov edi, 0xB8000
                        mov rax, 0x076C076C07650748
                        mov [rdi], rax
                        mov rax, 0x076F07570720076F
                        mov [rdi + 8], rax
                        mov rax, 0x07210764076C0772
                        mov [rdi + 16], rax

                        mov rax, kloader_start
                        jmp rax
