# ETAPA 1: CONSTRUCCIÓN (El Chef) 🍳
# Usamos una imagen que tiene todas las herramientas (SDK) para compilar
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos el archivo de proyecto y restauramos dependencias (los nuget)
COPY ["ProyectoPrueba.csproj", "./"]
RUN dotnet restore "ProyectoPrueba.csproj"

# Copiamos todo el resto del código
COPY . .

# Compilamos y publicamos una versión optimizada (Release)
RUN dotnet publish "ProyectoPrueba.csproj" -c Release -o /app/publish

# ETAPA 2: EJECUCIÓN (El Mesero) 🚀
# Usamos una imagen más liviana, solo para correr el programa (sin herramientas de compilación)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Le decimos que escuche en el puerto 8080
ENV ASPNETCORE_HTTP_PORTS=8080

# Comando de arranque
ENTRYPOINT ["dotnet", "ProyectoPrueba.dll"]