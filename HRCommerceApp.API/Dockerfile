FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HRCommerceApp.API/HRCommerceApp.API.csproj", "HRCommerceApp.API/"]
COPY ["HRCommerceApp.Core/HRCommerceApp.Core.csproj", "HRCommerceApp.Core/"]
COPY ["HRCommerceApp.Infrastructure/HRCommerceApp.Infrastructure.csproj", "HRCommerceApp.Infrastructure/"]
COPY ["HRCommerceApp.Services/HRCommerceApp.Services.csproj", "HRCommerceApp.Services/"]


RUN dotnet restore "HRCommerceApp.API/HRCommerceApp.API.csproj"
COPY . .
WORKDIR "/src/HRCommerceApp.API"
RUN dotnet build "HRCommerceApp.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "HRCommerceApp.API.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRCommerceApp.API.dll"]