# 1. Folosim SDK-ul de .NET 10 pentru compilare
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiem csproj și restaurăm
COPY ["InternshipLogbook.API.csproj", "./"]
RUN dotnet restore "InternshipLogbook.API.csproj"

# Copiem restul fișierelor și publicăm
COPY . .
RUN dotnet publish "InternshipLogbook.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Folosim Runtime-ul de .NET 10 pentru rulare
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf

COPY --from=build /app/publish .

# Setări necesare pentru SQL Server și Rețea
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:5203

EXPOSE 5203

ENTRYPOINT ["dotnet", "InternshipLogbook.API.dll"]