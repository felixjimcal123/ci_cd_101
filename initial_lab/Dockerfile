# Lección 9: imagen independiente del runner; .NET 10 LTS.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY src/Integration.Api/Integration.Api.csproj src/Integration.Api/
RUN dotnet restore src/Integration.Api/Integration.Api.csproj
COPY src/Integration.Api/ src/Integration.Api/
RUN dotnet publish src/Integration.Api/Integration.Api.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /out .
USER $APP_UID
ENTRYPOINT ["dotnet", "Integration.Api.dll"]
