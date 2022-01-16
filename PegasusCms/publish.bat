set configuration=Debug
set source=C:\Projects\PegasusCms\PegasusCms
set dest=C:\inetpub\wwwroot\pegasus-cms

xcopy %source%\bin\%configuration%\net6.0\PegasusCms* %dest% /s /d /y
xcopy %source%\Views\*.cshtml %dest%\Views /s /d /y
xcopy %source%\wwwroot %dest%\wwwroot /e /d /y