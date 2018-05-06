FROM blindstriker/aspnetcore-angular-universal:latest AS base
WORKDIR /app
EXPOSE 80

FROM blindstriker/aspnetcore-build-angular-universal:latest AS build
WORKDIR /src
COPY Codefiction.CodefictionTech.sln ./
COPY CodefictionApi/CodefictionApi.csproj CodefictionApi/
COPY CodefictionApi.Core/CodefictionApi.Core.csproj CodefictionApi.Core/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/CodefictionApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CodefictionApi.dll"]