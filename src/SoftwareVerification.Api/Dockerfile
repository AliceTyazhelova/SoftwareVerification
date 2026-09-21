# Стейдж сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем .csproj всех проектов, чтобы восстановить зависимости слоями
COPY ["src/SoftwareVerification.Domain/SoftwareVerification.Domain.csproj", "src/SoftwareVerification.Domain/"]
COPY ["src/SoftwareVerification.Infrastructure/SoftwareVerification.Infrastructure.csproj", "src/SoftwareVerification.Infrastructure/"]
COPY ["src/SoftwareVerification.Api/SoftwareVerification.Api.csproj", "src/SoftwareVerification.Api/"]

RUN dotnet restore "src/SoftwareVerification.Api/SoftwareVerification.Api.csproj"

# Копируем весь исходный код
COPY . .

# Собираем и публикуем
WORKDIR /src/src/SoftwareVerification.Api
RUN dotnet publish "SoftwareVerification.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Стейдж запуска
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SoftwareVerification.Api.dll"]