FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Backend/Backend.csproj ./
RUN dotnet restore "Backend.csproj"

COPY Backend/. .

RUN dotnet publish "Backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=7151
EXPOSE 7151
ENTRYPOINT ["dotnet", "Backend.dll"]