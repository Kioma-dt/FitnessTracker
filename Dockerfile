FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY *.slnx .
COPY FitnessTracker.API/*.csproj ./FitnessTracker.API/
COPY FirnessTracker.API.Tests/*.csproj ./FirnessTracker.API.Tests/
COPY FitnessTracker.Application/*.csproj ./FitnessTracker.Application/
COPY FitnessTracker.Infrastructure/*.csproj ./FitnessTracker.Infrastructure/
COPY FitnessTracker.Application.Tests/*.csproj ./FitnessTracker.Application.Tests/
COPY FitnessTracker.DataAccess/*.csproj ./FitnessTracker.DataAccess/
COPY FitnessTracker.DataAccess.Tests/*.csproj ./FitnessTracker.DataAccess.Tests/
COPY FitnessTracker.Entities/*.csproj ./FitnessTracker.Entities/
COPY FitnessTracker.Shared/*.csproj ./FitnessTracker.Shared/
COPY FitnessTracker.Mapping/*.csproj ./FitnessTracker.Mapping/

RUN dotnet restore

COPY . .
WORKDIR /source/FitnessTracker.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update \
    && apt-get install -y libfontconfig
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "FitnessTracker.API.dll"]