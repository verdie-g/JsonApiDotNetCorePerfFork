# JsonApiDotNetCore Performance Tests

[![Build status](https://ci.appveyor.com/api/projects/status/0qxgxdu8inpyp491/branch/master?svg=true)](https://ci.appveyor.com/project/json-api-dotnet/performancereports/branch/master)

Head to the [issues](https://github.com/json-api-dotnet/PerformanceReports/issues)
to see the latest test results.

## What is this?

This repository provides tooling for running load tests against a JsonApiDotNetCore application.
It uses [vegeta](https://github.com/tsenart/vegeta) to run tests against a sample application (located under `./app`).
If you want to see the details of the tests, take a look at `./load-test/test.sh`

### The App

```
ASP.Net Core → JsonApiDotNetCore → Entity Framework Core → Npgsql → PostgreSQL
```

### Execution Environment

The PostgreSQL database as well as a Grafana stack are run inside docker and the vegeta load testing tool are run
in docker. The app itself is run outside of Docker so it's easier to profile it. The app emits many different metrics
to Grafana to be able to easily find potential bottlenecks.

### Pre-Requisites

* Docker
* Docker Compose
* .NET 8.0

### Running The Test

Run the database and Grafana
```
docker-compose -f infra/infra.yml build --pull
docker-compose -f infra/infra.yml up -d
```
Run the app in a first terminal
```
dotnet run -c Release --project app
```
and the load test in a second terminal
```
./load-test/run.sh
```

Then you can observe the real-time performance on Grafana (http://localhost:3000).
![image](https://github.com/json-api-dotnet/PerformanceReports/assets/9092290/c93e764f-0593-4c6f-960d-600adb397099)

Once you're done you can run that command to stop the infra
```
docker-compose -f infra/infra.yml down
```
