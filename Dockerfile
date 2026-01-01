# ETAPA 1: Build (Construcción)
# Usamos la imagen con el SDK completo para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el csproj y restauramos dependencias (optimización de caché)
COPY ["TareaAPI.csproj", "./"]
RUN dotnet restore "TareaAPI.csproj"

# Copiamos todo el código fuente y compilamos
COPY . .
RUN dotnet publish "TareaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ETAPA 2: Runtime (Ejecución)
# Usamos una imagen ligera solo con el runtime de ASP.NET (más segura y rápida)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiamos los archivos compilados de la etapa anterior
COPY --from=build /app/publish .

# Punto de entrada
ENTRYPOINT ["dotnet", "TareaAPI.dll"]
