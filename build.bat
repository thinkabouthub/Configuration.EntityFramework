
@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%BuildCounter%" == "" (
   set version=--version-suffix ci-%BuildCounter%
)

REM (optional) build.bat is in the root of our repo, cd to the correct folder where sources/projects are
echo %cd%
echo %cd src\Configuration.EntityFramework%
REM Restore
call dotnet restore
if not "%errorlevel%"=="0" goto failure

REM Build
REM - Option 1: Run dotnet build for every source folder in the project
REM   e.g. call dotnet build <path> --configuration %config%
REM - Option 2: Let msbuild handle things and build the solution
"%programfiles(x86)%\MSBuild\15.0\Bin\MSBuild.exe" src\Configuration.EntityFramework\Configuration.EntityFramework.csproj /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
REM call dotnet build --configuration %config%
if not "%errorlevel%"=="0" goto failure

:success
exit 0

:failure
exit -1
