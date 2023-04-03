FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY Account.Web/bin/Release/net6.0/publish .
ENTRYPOINT ["dotnet", "Account.Web.dll"]