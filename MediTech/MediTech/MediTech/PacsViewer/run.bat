@echo off

if defined ProgramFiles(x86) (
    ::64-bit
    start /B x64\mdicom.exe /scan .
) else (
    ::32-bit
    start /B win32\mdicom.exe /scan .
)

