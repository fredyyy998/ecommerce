# E-Commerce
E-Commerce Project based on microservice architecture build with .net 6.0.16


## [Getting Started](https://github.com/fredyyy998/ecommerce/wiki)

## Status
[![Docker images](https://github.com/fredyyy998/ecommerce/actions/workflows/publish-docker-images.yml/badge.svg)](https://github.com/fredyyy998/ecommerce/actions/workflows/publish-docker-images.yml)

[![NuGet Package](https://github.com/fredyyy998/ecommerce/actions/workflows/publish-packages.yml/badge.svg)](https://github.com/fredyyy998/ecommerce/actions/workflows/publish-packages.yml)

[![UnitTest](https://github.com/fredyyy998/ecommerce/actions/workflows/unit-test.yml/badge.svg)](https://github.com/fredyyy998/ecommerce/actions/workflows/unit-test.yml)

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
