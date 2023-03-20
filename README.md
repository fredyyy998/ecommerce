# E-Commerce
E-Commerce Project based on microservice architecture build with .net


## Getting Started

Run `docker-compose up` in the project root to start the docker container providing a postgres database and the infrastructure for Apache Kafka.

The services share code via the Common project. Currently that is provided locally as a nuget package. To do that follow these steps:

1. Pack the project with `dotnet pack -o {direcotry path}`
2. Reference the local package directory

// tbd


## Project Structure

The soruce code is structured as following:

- Every service has a directory inside src/Services
- each service is a .net solution
- each service is structured in different projects
  - .Core has the Domain Model
  - .Test has all xUnit Tests
  - .Web has the exposed api logic
  - .Application has the application logic
  - .Infrastructure has the database implemention and inter service communication