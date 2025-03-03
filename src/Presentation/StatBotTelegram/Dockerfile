FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY [".StatBotTelegram.csproj", "StatBotTelegram/"]
RUN dotnet restore "./StatBotTelegram/StatBotTelegram.csproj"
COPY . .
WORKDIR "/src/StatBotTelegram"
RUN dotnet build "./StatBotTelegram.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./StatBotTelegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
ENTRYPOINT ["dotnet", "/app/publish/StatBotTelegram.dll"]