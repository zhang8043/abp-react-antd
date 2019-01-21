cls 
@ECHO OFF 
SET NET_CORE_64=https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.102-windows-x64-installer
SET NET_CORE_32=https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.102-windows-x86-installer
SET REACT=Precise-Antd
SET Core=Precise-Core
SET Web=Precise.Web.Host
color 0a 
TITLE 项目启动程序 by:zyp
GOTO MENU 
:MENU 
CLS 
ECHO. 
ECHO. * * * * ASP.NET Boilerplate + React + Ant Design Pro 快速启动 * * *  
ECHO. 
ECHO. 0 下载.NET Core 2.2.102
ECHO. 
ECHO. 1 启动API
ECHO. 
ECHO. 2 启动React
ECHO. 
ECHO. 3 启动API+React
ECHO. 
ECHO. 4 删除Bin和Obj文件夹
ECHO. 
ECHO. 5 查看当前.NET Core版本
ECHO. 
ECHO. 6 退 出
ECHO.  
ECHO. * * * * * * * * * * * * * ** * * * * * * ** * * * * * * ** * * * * * 
ECHO. 
ECHO.请输入需要操作的编号： 
set /p ID= 
IF "%id%"=="0" GOTO download 
IF "%id%"=="1" GOTO startapi 
IF "%id%"=="2" GOTO startreact 
IF "%id%"=="3" GOTO startapireact
IF "%id%"=="4" GOTO deletebinobj
IF "%id%"=="5" GOTO netcoreversion
IF "%id%"=="6" EXIT 
PAUSE 
:download 
ECHO. 
ECHO.启动Nginx...... 
ECHO. 1 下载64位
ECHO. 
ECHO. 2 下载32位
ECHO.
ECHO. 3 退 出
ECHO.
set /p ID= 
IF "%id%"=="1" GOTO NETCORE64 
IF "%id%"=="2" GOTO NETCORE32 
IF "%id%"=="3" GOTO MENU
ECHO. 
PAUSE 
GOTO MENU
:NETCORE64 
ECHO. 
ECHO.打开下载网站...... 
start %NET_CORE_64%
ECHO. 
ECHO.下载中......
ECHO.下载完毕请自行安装......
ECHO. 
PAUSE 
GOTO MENU
:NETCORE32 
ECHO. 
start %NET_CORE_32%
ECHO. 
PAUSE 
GOTO MENU
:startapi 
ECHO. 
ECHO.正在启动API......
START cmd /k "CD %Core%&&dotnet restore&&dotnet build&&cd %Web%&&dotnet run"
ECHO.启动中......
ECHO. 
PAUSE 
GOTO MENU
:startreact 
ECHO. 
ECHO.正在启动React......
if exist %REACT%/node_modules (
	START cmd /k "CD %REACT%&&npm start"
) else (
	START cmd /k "CD %REACT%&&npm install&&npm start"
)
ECHO.启动中......
ECHO. 
PAUSE 
GOTO MENU
:startapireact 
ECHO. 
ECHO.正在启动......
START cmd /k "CD %Core%&&dotnet restore&&dotnet build&&cd %Web%&&dotnet run"
if exist %REACT%/node_modules (
	START cmd /k "CD %REACT%&&npm start"
) else (
	START cmd /k "CD %REACT%&&npm install&&npm start"
)
ECHO.启动中......
ECHO. 
:deletebinobj 
ECHO. 
ECHO.正在删除......
FOR /d /r . %%d in (bin,obj) DO (
	IF EXIST "%%d" (		 	 
		ECHO %%d | FIND /I "\node_modules\" > Nul && ( 
			ECHO.Skipping: %%d
		) || (
			ECHO.Deleting: %%d
			rd /s/q "%%d"
		)
	)
)
ECHO.删除完成......
ECHO.
PAUSE
GOTO MENU
:netcoreversion
ECHO. 
ECHO.当前使用的版本是：
dotnet --version
ECHO.
PAUSE
GOTO MENU






