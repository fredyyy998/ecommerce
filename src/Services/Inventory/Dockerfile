FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY Inventory.Web/bin/Release/net6.0/publish .
ENTRYPOINT ["dotnet", "Inventory.Web.dll"]