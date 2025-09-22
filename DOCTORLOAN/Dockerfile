# ---------- STAGE 1: BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY DOCTORLOAN.csproj ./
RUN dotnet restore

# Copy the full source and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ---------- STAGE 2: RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "DOCTORLOAN.dll"]