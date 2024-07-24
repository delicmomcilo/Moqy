FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/Moqy.Api
COPY ["./Moqy.Api.csproj", "."]
RUN dotnet restore "./Moqy.Api.csproj"
COPY . .
WORKDIR "/src/Moqy.Api/."
RUN dotnet build "Moqy.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Moqy.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Moqy.Api.dll"]