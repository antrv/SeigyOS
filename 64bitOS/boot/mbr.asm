                use16
                org 0x0600

mbr:
                xor ax, ax
                mov ss, ax
                mov bx, 0x7C00
                mov sp, bx
                mov es, ax
                mov ds, ax
                mov si, bx
                mov di, $$
                mov cx, 0x0200
                cld
                rep movsb

                push ax
                push .continue
                retf

.continue:    
                sti
                xor cx, cx
                mov cl, 4
                mov bp, partition_table
.check_entry:   cmp byte [bp], 0
                jl .found_entry
                jnz .error_invalid_table
                add bp, 0x10
                loop .check_entry

                int 0x18    ; Start ROM-BASIC (only available on some IBM machines!) 
                            ; Many BIOS simply display: "PRESS A KEY TO REBOOT"

.found_entry:   mov [bp], dl
                push bp
                mov byte [bp + 0x11], 5 ; reading repeat times
                mov byte [bp + 0x10], 0

                mov ah, 0x41
                mov bx, 0x55AA
                int 0x13    ; INT13, Function 41h (with BX=55AAh) - Check for Int 13 Extensions in BIOS.

                pop bp
                jb .read_bootsect
                cmp bx, 0xAA55
                jnz .read_bootsect
                test cx, 1
                jz .read_bootsect

                inc byte [bp + 0x10]

.read_bootsect: pushad
                cmp byte [bp + 0x10], 0
                jz .no_ext_read

                push dword 0        ; VBR's Starting Sector
                push dword [bp + 8] ; Location of VBR Sector
                push word 0         ; Offset part of address in memory
                push 0x7C00         ; Segment part of address in memory
                push word 1         ; Copy only 1 sector
                push word 0x10      ; Reserved and DAP Size (16 bytes)

                mov ah, 0x42
                mov dl, [bp] ; Drive Number
                mov si, sp   ; DS:SI must point to -> "Disk Address Packet (DAP)" on Stack
                int 0x13     ; INT 13, Function 42h ("Extended Read")

                lahf
                add sp, 0x10
                sahf
                jmp .restore_regs

.no_ext_read:   mov ax, 0x0201   ; Function 02h, read only 1 sector
                mov bx, 0x7C00   ; Buffer for read starts at 7C00
                mov dl, [bp]     ; DL = Disk Drive
                mov dh, [bp + 1] ; DH = Head number (never use FFh)
                mov cl, [bp + 2] ; Bits 0-5 of CL (max. value 3Fh) make up the Sector number
                mov ch, [bp + 3] ; Bits 6-7 of CL become highest two bits (8-9) with bits 0-7 of CH to make Cylinder number (max. 3FFh)
                int 0x13         ; INT13, Function 02h: READ SECTORS into Memory at ES:BX (0000:7C00)

.restore_regs:  popad

                jnb .check_sign
                dec byte [bp + 0x11]
                jnz .drive_reset
                cmp byte [bp], 0x80
                jz .error_loading_os ; Error loading operating system
                mov dl, 0x80
                jmp .found_entry
                push bp

.drive_reset:   xor ah, ah   ; INT13, Function 00h (Reset Disk Drive)
                mov dl, [bp] ; Disk drive
                int 0x13

                pop bp
                jmp .read_bootsect
.check_sign:    cmp word [0x7DFE], 0xAA55
                jnz .error_missing_os ; Missing operating system
                push word [bp]

.enable_a20:    call a20_check
                jnz .tcg_check

                cli
                mov al, 0xD1
                out 0x64, al
                call a20_check

                mov al, 0xDF
                out 0x60, al
                mov al, 0xFF
                call a20_check

.tcg_check:     mov ax, 0xBB00 ; With AH = BBh and AL = 00h
                int 0x1A       ; TCG_StatusCheck

                and eax, eax       ; If EAX does not equal zero,
                jnz .goto_bootsect ; then no BIOS support for TCG.

                cmp ebx, 0x41504354 ; EBX must also return the numerical equivalent of the ASCII character string "TCPA" ("54 43 50 41")
                jnz .goto_bootsect  ; exit TCG code
                cmp cx, 0x0102      ; version 1.2 or higher?
                jb .goto_bootsect

                push dword 0x0000BB07 ; Setup for INT 1Ah AH = BB, AL = 07h command (p.94 f)
                push dword 0x00000200
                push dword 0x00000008
                push dword ebx
                push dword ebx
                push dword ebp
                push dword 0x00000000
                push dword 0x00007C00
                popad
                push word 0x0000
                pop es
                int 0x1A ; TCG_CompactHashLogExtendEvent

.goto_bootsect: pop dx
                xor dh, dh
                jmp 0x0000:0x7C00 ; Jump to Volume Boot Record code
                int 0x18

.error_missing_os:
                mov ax, msg_missing_os
                jmp .write_msg
.error_loading_os:
                mov ax, msg_loading_os
                jmp .write_msg
.error_invalid_table:
                mov ax, msg_invalid_table

.write_msg:
                mov si, ax
.load_char:     lodsb
                cmp al, 0
                jz .halt
                mov bx, 0x0007
                mov ah, 0x0E
                int 0x10
                jmp .load_char
.halt:          hlt
                jmp .halt

a20_check:      sub cx, cx ; Part of A20 Line Enablement code. This routine checks/waits for access to KB controller.
.check_port:    in al, 0x64
                jmp .next
.next:          and al, 2
                loopne .check_port
                and al, 2
                ret

msg_missing_os:
                db 'Missing operating system...', 0
msg_loading_os:
                db 'Error loading operating system...', 0
msg_invalid_table:
                db 'Invalid partition table...', 0

repeat          0x1B8 - ($ - $$)
                nop
end repeat

disk_signature: dd 0
                dw 0

partition_table:
                dd 0, 0, 0, 0
                dd 0, 0, 0, 0
                dd 0, 0, 0, 0
                dd 0, 0, 0, 0

signature:      dw 0x55AA
