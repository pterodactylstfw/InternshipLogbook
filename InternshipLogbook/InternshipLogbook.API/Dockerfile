FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["InternshipLogbook.API.csproj", "./"]
RUN dotnet restore "InternshipLogbook.API.csproj"

COPY . .
RUN dotnet publish "InternshipLogbook.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "InternshipLogbook.API.dll"]