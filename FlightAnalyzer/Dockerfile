# Use the official Microsoft .NET SDK image to build your app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FlightAnalyzer.csproj", "./"]
RUN dotnet restore "./FlightAnalyzer.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FlightAnalyzer.csproj" -c Release -o /app/build

# Publish your app
FROM build AS publish
RUN dotnet publish "FlightAnalyzer.csproj" -c Release -o /app/publish

# Use the official Microsoft .NET runtime image to run your app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy your CSV file into the Docker image
COPY ./Data/flights.csv ./Data/
ENTRYPOINT ["dotnet", "FlightAnalyzer.dll"]
