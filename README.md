# E-Commerce
E-Commerce Project based on microservice architecture build with .net


## Getting Started 
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