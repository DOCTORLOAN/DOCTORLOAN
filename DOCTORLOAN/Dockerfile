# ---------- STAGE 1: BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["DOCTORLOAN/DOCTORLOAN.csproj", "DOCTORLOAN/"]
RUN dotnet restore "./DOCTORLOAN/DOCTORLOAN.csproj"

# Copy the full source and build
COPY . .
WORKDIR "/src/DOCTORLOAN"
RUN dotnet build "DOCTORLOAN.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DOCTORLOAN.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------- STAGE 2: RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .

# Start the app
ENTRYPOINT ["dotnet", "DOCTORLOAN.dll"]