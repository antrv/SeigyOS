                        use16
                        org 0x7C00

bootsector:             jmp continue

                        ; parameters
                        start_lba = 0x11
                        reserved_sectors = 0x20
                        fat_size = 0x3A2
                        fat_table_lba = start_lba + reserved_sectors
                        second_fat_lba = fat_table_lba + fat_size
                        cluster_data_start = second_fat_lba + fat_size

repeat                  3 - ($ - $$)
                        nop
end repeat                

oem_name:                db 8 dup 0x20

bpb:                    ; bios parameters block
                        ; DOS 2.0 BPB
.bytes_per_sector:      dw 0 ; Bytes per logical sector in powers of two; the most common value is 512.
.sectors_per_cluster:   db 0 ; Logical sectors per cluster. Allowed values are 1, 2, 4, 8, 16, 32, 64, and 128.
.reserved_sectors:      dw 0 ; Count of reserved logical sectors. The number of logical sectors before the first FAT in the file system image. 
                             ; At least 1 for this sector, usually 32 for FAT32 (to hold the extended boot sector, FS info sector and backup boot sectors).
.fat_table_count:       db 0 ; Number of File Allocation Tables. Almost always 2; RAM disks might use 1. Most versions of MS-DOS/PC DOS do not support more than 2 FATs.
.root_elem_count:       dw 0 ; Maximum number of FAT12 or FAT16 root directory entries. 0 for FAT32, where the root directory is stored in ordinary data clusters
                             ; see offset 0x02C in FAT32 EBPBs.
.logical_sectors16:     dw 0 ; Total logical sectors (if zero, use 4 byte value at offset 0x020 in FAT32 EBPBs).
.drive_type:            db 0 ; Media descriptor
.fat_size16:            dw 0 ; Logical sectors per File Allocation Table for FAT12/FAT16. FAT32 sets this to 0 and uses the 32-bit value at .fat_size32.
                        ; DOS 3.31 BPB
.phys_sectors_on_track: dw 0 ; Physical sectors per track for disks with INT 13h CHS geometry, e.g., 18 for a “1.44 MB” (1440 KB) floppy. 
                             ; Unused for drives, which don't support CHS access any more.
.heads_count:           dw 0 ; Number of heads for disks with INT 13h CHS geometry, e.g., 2 for a double sided floppy. Unused for drives, which don't support CHS access any more.
.hidden_sectors:        dd 0 ; Count of hidden sectors preceding the partition that contains this FAT volume. This field should always be zero on media that are not partitioned.
.logical_sectors32:     dd 0 ; Total logical sectors (if greater than 65535; otherwise, see .logical_sectors16).
                        ; DOS 7.1 EBPB (FAT32)
.fat_size32:            dd 0 ; Logical sectors per file allocation table (corresponds with the old entry at .fat_size16 in the DOS 2.0 BPB).
.flags_doubled:         dw 0 ; Drive description / mirroring flags (bits 3-0: zero-based number of active FAT, if bit 7 set. 
                             ; If bit 7 is clear, all FATs are mirrored as usual. Other bits reserved and should be 0.)
.version:               dw 0 ; Version (defined as 0.0). The high byte of the version number is stored at offset 0x02B, and the low byte at offset 0x02A. 
                             ; FAT32 implementations should refuse to mount volumes with version numbers unknown by them.
.root_dir:              dd 0 ; Cluster number of root directory start, typically 2 if it contains no bad sector.
.fsi_cluster:           dw 0 ; Logical sector number of FS Information Sector, typically 1, i.e., the second of the three FAT32 boot sectors.
.reserved_sectors_addr: dw 0 ; First logical sector number of a copy of the three FAT32 boot sectors, typically 6.
.reserved:              db 12 dup 0 ; Reserved (may be changed to format filler byte 0xF6 as an artifact by MS-DOS FDISK, must be initialized to 0 by formatting tools,
                                    ; but must not be changed by file system implementations or disk tools later on.)
.drive_number:          db 0 ; Physical drive number (0x00 for (first) removable media, 0x80 for (first) fixed disk as per INT 13h). 
                             ; Allowed values for possible physical drives depending on BIOS are 0x00-0x7E and 0x80-0xFE. 
                             ; Values 0x7F and 0xFF are reserved for internal purposes such as remote or ROM boot and should never occur on disk.
.flags:                 db 0 ; Reserved
.extended_boot_record:  db 0 ; Extended boot signature (0x28 or 0x29)

.serial_number:         dd 0 ; Volume ID (serial number)
.volume_label:          db 11 dup 0 ; Partition Volume Label, padded with blanks (0x20)
.fs_type:               db 8 dup 0 ; File system type, padded with blanks (0x20)

fat_read_addr_ofs       = bpb.reserved - bpb
cluster_read_addr_ofs   = bpb.reserved - bpb + 4
cluster_index_ofs       = bpb.reserved - bpb + 8

bytes_per_cluster_ofs   = bpb.volume_label - bpb

dap_block_ofs           = bpb.volume_label - bpb + 2
dap_block_sectors_ofs   = dap_block_ofs + 2
dap_block_offset_ofs    = dap_block_ofs + 4
dap_block_segment_ofs   = dap_block_ofs + 6
dap_block_lba_ofs       = dap_block_ofs + 8

repeat                  0x5A - ($ - $$)
                        db 0
end repeat                

continue:               xor ax, ax
                        mov ds, ax
                        mov es, ax
                        mov ss, ax
                        mov ax, 0x7C00
                        mov sp, ax

                        ; check BIOS Int 13h extension
                        mov ah, 0x41
                        mov bx, 0x55AA
                        int 0x13 ; INT13, Function 41h (with BX=55AAh) - Check for Int 13 Extensions in BIOS.
                        cmp bx, 0xAA55
                        jnz error_loading_os
                        test cx, 1
                        jz error_loading_os

                        ; calculate BPB address
                        mov bp, bpb
                        mov word [bp + fat_read_addr_ofs], 0x7E00
                        mov word [bp + cluster_read_addr_ofs], 0x8000

                        ; dap block initialization
                        xor eax, eax
                        mov word [bp + dap_block_ofs], 0x10
                        mov [bp + dap_block_segment_ofs], ax
                        mov [bp + dap_block_lba_ofs], eax
                        mov [bp + dap_block_lba_ofs + 4], eax

                        ; validate BPB
                        cmp word [bp + bpb.bytes_per_sector - bpb], 0x0200
                        jnz error_loading_os
                        mov ah, [bp + bpb.sectors_per_cluster - bpb]
                        cmp ah, 0x40
                        jg error_loading_os

                        ; calculate bytes per cluster 
                        ; eax = 256 * sectors per cluster (al = 0, ah = sectors per cluster)
                        shl ax, 1
                        mov [bp + bytes_per_cluster_ofs], ax

                        ; find kernel file
                        mov eax, [bp + bpb.root_dir - bpb]
                        mov [bp + cluster_index_ofs], eax

.scan_dir_cluster:      call read_cluster ; read root directory

                        mov dx, [bp + bytes_per_cluster_ofs]
                        mov bx, [bp + cluster_read_addr_ofs]
                        shr dx, 4

.check_file_entry:      test byte [bx + 0xB], 0x18 ; test file attributes for volume label and directory bits
                        jnz .next_file_entry

                        mov di, bx
                        mov cx, 11 ; file name 8 chars + extension 3 chars
                        mov si, kernel_name
                        cld
                        repz cmpsb
                        jz .load_kernel

.next_file_entry:       add bx, 0x10 ; dir entry size
                        and dx, dx
                        jnz .check_file_entry ; loop for entries in cluster

                        call verify_cluster_index
                        jc error_loading_os ; end of chain

                        jmp .scan_dir_cluster

.load_kernel:            ; file found
                        mov ax, [bx + 0x14] ; high 2 bytes of cluster number
                        shl eax, 16
                        mov ax, [bx + 0x1A] ; low 2 bytes of cluster number
                        
                        mov [bp + cluster_index_ofs], eax

.next_file_cluster:                        
                        call read_cluster ; read file cluster
                        call verify_cluster_index
                        jc .jump_to_kernel ; end of chain
                        mov ax, [bp + bytes_per_cluster_ofs]
                        add [bp + cluster_read_addr_ofs], ax
                        jmp .next_file_cluster 

.jump_to_kernel:        mov ax, msg_loading_os
                        call write_msg

                        mov ebx, 0x7A53489D ; magic number
                        jmp 0x8000

error_loading_os:       mov ax, msg_error_loading_os

write_msg_and_halt:     call write_msg
halt:                   hlt
                        jmp halt

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
                        
verify_cluster_index:   mov eax, [bp + cluster_index_ofs] ; fat entry number
                        and eax, 0x0FFFFFFF
                        cmp eax, 2
                        jl error_loading_os
                        cmp eax, 0x0FFFFFF8
                        jl .next_check
                        stc ; end of chain
                        ret
.next_check:            cmp eax, 0x0FFFFFEF
                        jg error_loading_os
                        clc
                        ret

read_cluster:           mov eax, [bp + cluster_index_ofs] ; fat entry number
                        sub eax, 2
                        movzx ecx, byte [bp + bpb.sectors_per_cluster - bpb]
                        mov word [bp + dap_block_sectors_ofs], cx
                        mul ecx
                        add eax, cluster_data_start
                        mov bx, [bp + cluster_read_addr_ofs]
                        mov [bp + dap_block_offset_ofs], bx
                        call read_sectors
                        call read_fat_sector
                        mov bx, [bp + cluster_index_ofs] ; fat entry number
                        and bl, 127
                        shl bx, 2
                        add bh, 0x7E
                        mov eax, [bx]
                        mov [bp + cluster_index_ofs], eax ; next fat entry number
                        ret

read_fat_sector:        mov word [bp + dap_block_sectors_ofs], 1 ; read 1 sector
                        mov word [bp + dap_block_offset_ofs], 0x7E00 ; into memory 0x7E00

                        mov eax, [bp + cluster_index_ofs] ; fat entry number
                        shr eax, 7 ; 512=2^11 bytes per sector / 4=2^2 bytes per entry = 2^7
                        add eax, fat_table_lba ; ecx - fat sector

read_sectors:           mov [bp + dap_block_lba_ofs], eax
                        push bp
                        mov ah, 0x42
                        mov dl, [drive_number]
                        mov si, bpb + dap_block_ofs
                        int 0x13
                        pop bp
                        ret

msg_loading_os:         db 'Loading OS...', 13, 10, 0
msg_error_loading_os:   db 'Error loading OS...', 0
kernel_name:            db 'KLOADER BIN'

repeat                  0x200 - 3 - ($ - $$)
                        nop
end repeat

drive_number:           db 0x80
signature:              dw 0xAA55    ; Boot Sector Signature
