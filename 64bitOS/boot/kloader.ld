SECTIONS
{
	. = 0x8000;
	.text : { 
		*(.start)
		*(.text)
		*(.rdata)
		*(.rodata)
		*(.data)
	}
}
