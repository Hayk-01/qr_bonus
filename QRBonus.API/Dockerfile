FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore QRBonus/QRBonus.API/QRBonus.API.csproj
RUN dotnet publish QRBonus/QRBonus.API/QRBonus.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

EXPOSE 80

COPY --from=build /out .

ENTRYPOINT ["dotnet", "QRBonus.API.dll"]
