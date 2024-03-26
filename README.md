# FlightAnalyzer
Flight quality analysis

## Background
An airline wants us to create a solution to analyse airline aircraft
flight data quality tod find inconsistencies from flight records
realtime. Currently airline has dedicated persons doing work
manually by using Excel. Your task is to help airline to automate
data quality analysis.

## Deployment
- docker build -t danisaghy/flightanalyzer:latest .
- docker push danisaghy/flightanalyzer:latest
- flyctl auth login
- flyctl apps create flightanalyzer
- flyctl launch --no-deploy //creates fly.toml file
- flyctl deploy

Check deployed version at: https://flightanalyzer.fly.dev/swagger/index.html
