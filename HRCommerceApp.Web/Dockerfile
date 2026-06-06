FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HRCommerceApp.Web/HRCommerceApp.Web.csproj", "HRCommerceApp.Web/"]
COPY ["HRCommerceApp.Core/HRCommerceApp.Core.csproj", "HRCommerceApp.Core/"]
COPY ["HRCommerceApp.Services/HRCommerceApp.Services.csproj", "HRCommerceApp.Services/"]
COPY ["HRCommerceApp.Infrastructure/HRCommerceApp.Infrastructure.csproj", "HRCommerceApp.Infrastructure/"]


RUN dotnet restore "HRCommerceApp.Web/HRCommerceApp.Web.csproj"
COPY . .
WORKDIR "/src/HRCommerceApp.Web"
RUN dotnet build "HRCommerceApp.Web.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "HRCommerceApp.Web.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRCommerceApp.Web.dll"]