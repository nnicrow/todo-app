FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TODOApp/TODOApp.csproj", "TODOApp/"]
RUN dotnet restore "TODOApp/TODOApp.csproj"
COPY . .
WORKDIR "/src/TODOApp"
RUN dotnet build "TODOApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TODOApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TODOApp.dll"]
