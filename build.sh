#!/usr/bin/env bash

#exit if any command fails
set -e

export FrameworkPathOverride=$(dirname $(which mono))/../lib/mono/4.6.1-api/

dotnet restore ./src/Codefiction.CodefictionTech.sln

# Linux/Darwin
OSNAME=$(uname -s)
echo "OSNAME: $OSNAME"

dotnet build ./src/Tests/CodefictionApi.Core.Tests/CodefictionApi.Core.Tests.csproj /p:Configuration=Release || exit 1
dotnet build ./src/Tests/CodefictionApi.IntegrationTests/CodefictionApi.IntegrationTests.csproj /p:Configuration=Release || exit 1
dotnet build ./src/Tests/CodefictionApi.Core.Tests/CodefictionApi.Core.Tests.csproj /p:Configuration=Release || exit 1

echo --------------------
echo Running NETCORE2 Tests
echo --------------------

dotnet test -c Release ./src/Tests/CodefictionApi.Core.Tests -f netcoreapp2.0
dotnet test -c Release ./src/Tests/CodefictionApi.IntegrationTests -f netcoreapp2.0
dotnet test -c Release ./src/Tests/CodefictionApi.Core.Tests -f netcoreapp2.0