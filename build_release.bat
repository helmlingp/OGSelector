dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true

copy /Y OGSelector\bin\Release\net10.0\win-x64\publish\OGSelector.exe %USERPROFILE%\Desktop\OGSelector.exe