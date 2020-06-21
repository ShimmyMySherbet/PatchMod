@echo off
call Patch.bat noexit
pushd D:\Servers\DevServers\Unturned\unturned\
Unturned.exe -nographics -batchmode +secureserver/CleanServer
echo Unturned Client closed.
Timeout /T 4