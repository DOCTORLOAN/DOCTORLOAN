# ---------- STAGE 1: BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy file project và restore
COPY DOCTORLOAN.csproj ./
RUN dotnet restore

# Copy toàn bộ mã nguồn
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ---------- STAGE 2: RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Mở port cho ứng dụng ASP.NET Core
EXPOSE 80

# Khởi động app
ENTRYPOINT ["dotnet", "DOCTORLOAN.dll"]