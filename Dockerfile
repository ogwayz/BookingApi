# Используем официальный образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копируем файлы решения и проекты
COPY BookingApi.sln .
COPY BookingApi.Presentation/BookingApi.Presentation.csproj BookingApi.Presentation/
COPY BookingApi.Infrastructure/BookingApi.Infrastructure.csproj BookingApi.Infrastructure/
COPY BookingApi.Domain/BookingApi.Domain.csproj BookingApi.Domain/
COPY BookingApi.Aplication/BookingApi.Aplication.csproj BookingApi.Aplication/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем все остальные файлы
COPY . .

# Собираем и публикуем проект
WORKDIR /app/BookingApi.Presentation
RUN dotnet publish -c Release -o /app/publish 

# Финальный образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingApi.Presentation.dll"]
