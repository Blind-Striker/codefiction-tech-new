dotnet --info

echo The installed .NET Core SDKs are:
dir $env:ProgramFiles"\dotnet\sdk" | findstr /l "."

dotnet restore ./src/Codefiction.CodefictionTech.sln

dotnet build ./src/Tests/CodefictionApi.Core.Tests/CodefictionApi.Core.Tests.csproj -c Release
dotnet build ./src/Tests/CodefictionApi.IntegrationTests/CodefictionApi.IntegrationTests.csproj -c Release
dotnet build ./src/Tests/CodefictionApi.Core.Tests/CodefictionApi.Core.Tests.csproj -c Release

echo --------------------
echo Running NET461 Tests
echo --------------------

dotnet test -c Release ./src/Tests/CodefictionApi.Core.Tests -f net461

echo --------------------
echo Running NETCORE2 Tests
echo --------------------

dotnet test -c Release ./src/Tests/CodefictionApi.Core.Tests -f netcoreapp2.0
dotnet test -c Release ./src/Tests/CodefictionApi.IntegrationTests -f netcoreapp2.0
dotnet test -c Release ./src/Tests/CodefictionApi.Core.Tests -f netcoreapp2.0