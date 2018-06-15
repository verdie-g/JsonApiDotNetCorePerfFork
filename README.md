# JsonApiDotNetCore Performance Tests

Hed to the [issues](https://github.com/json-api-dotnet/PerformanceReports/issues) 
to see the latest test results.

## What is this?

This repository provides tooling for running load tests against a sample JADNC application.
It uses [vegeta](https://github.com/tsenart/vegeta) to run tests against a sample application (located under `./app`).
If you want to see the details of the tests, take a look at `./load-test/test.sh`

### The App

```
ASP.Net Core → JsonApiDotNetCore → Entity Framework Core → Npgsql → PostgreSQL
```

### Execution Environment

The app and database run inside separate docker containers using docker-compose for orchestration. 
The test can be executed locally or on a Digital Ocean VM. 
docker-machine is used to create the VM on demand using the token specified in variables.env.

## Running The Test

### Pre-Requisites

* Docker
* Docker Compose
* Docker Machine

### Running Locally

- Set configuration in `variables.env` and source them into your shell

```sh
cp ./variables.env.sample ./variables.env
open ./variables.env
# edit the config, GH and DO values are not required for local testing
```

- Run the test

```sh
./run-local.sh
```

- The test results will be dumped to `./load-test/results`

### Running On Digital Ocean Droplet

- Set configuration in `variables.env`
- Run the test 

```
./run-on-droplet.sh
```

- Delete the droplet when done

```
./remove-droplet.sh
```

### Uploading Results to GitHub

- Ensure `UPLOAD_RESULTS` is set to `true` in variables.env file
