@echo off
echo =======================================================
echo Calorie Tracker (ASP.NET Core MVC) Baslatiliyor...
echo =======================================================
echo.
echo UYARI: Bu uygulamayi calistirmak icin sisteminizde .NET SDK 8.0 
echo veya uzeri bir surumun kurulu olmasi gerekmektedir.
echo.
echo Tarayicinizdan http://localhost:5000 veya http://localhost:5001 (HTTPS)
echo adresine giderek uygulamaya erisebilirsiniz.
echo.
echo API Dokumantasyonu (Swagger) icin: http://localhost:5000/swagger
echo.
dotnet run --urls "http://localhost:5000;https://localhost:5001"
pause
