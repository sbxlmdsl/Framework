ECHO Starting PreBuild.bat
REM Note: Beware that .bat files in VS have junk characters at beginning that must be removed via Binary Editor.
REM PreBuildEvent: Call $(ProjectDir)PreBuild.Bat $(ProjectDir)

ECHO ** PreBuild.bat **
ECHO Executing %1EFPartial.ps1 -Parameter1 %1

%WINDIR%\system32\attrib.exe %1*.cs -r
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope CurrentUser -Force
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File %1EFPartial.ps1 -Parameter1 %1
%WINDIR%\system32\attrib.exe %1*.cs +r