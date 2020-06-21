@echo off
taskkill /f /t /im Unturned.exe
copy "C:\Users\User\source\repos\PatchMod\PatchMod\bin\Debug\PatchMod.dll" "D:\Servers\DevServers\Unturned\unturned\Modules\PatchMod\PatchMod.dll"
if NOT [%1]==[noexit] exit