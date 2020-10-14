FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Starkit/Starkit.csproj", "Starkit/"]
RUN dotnet restore "Starkit/Starkit.csproj"
COPY . .
WORKDIR "/src/Starkit"
RUN dotnet build "Starkit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Starkit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Starkit.dll"]