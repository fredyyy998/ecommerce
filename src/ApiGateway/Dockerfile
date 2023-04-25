FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY OcelotApiGw/bin/Release/net6.0/publish .
ENTRYPOINT ["dotnet", "OcelotApiGw.dll"]