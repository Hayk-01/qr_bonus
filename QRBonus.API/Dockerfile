FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore QRBonus/QRBonus.API/QRBonus.API.csproj
RUN dotnet publish QRBonus/QRBonus.API/QRBonus.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create required media directory
RUN mkdir -p /var/www/website/QR_Bonus/QrBonusAdminMedia && \
    chmod 777 /var/www/website/QR_Bonus/QrBonusAdminMedia

ENV ASPNETCORE_HTTP_PORTS=8080
ENV Kestrel__Endpoints__Http__Url=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
COPY --from=build /out .
ENTRYPOINT ["dotnet", "QRBonus.API.dll"]
