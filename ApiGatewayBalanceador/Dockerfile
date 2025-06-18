# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o .csproj e restaura os pacotes
COPY ApiGatewayBalanceador.csproj ./
RUN dotnet restore ApiGatewayBalanceador.csproj

# Copia tudo e publica
COPY . . 
RUN dotnet publish Server1.csproj -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Server1.dll"]
