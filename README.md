# TFNValidation

This API was built for a coding challenge. TFNValidation - A web based Australian Tax File Number (TFN) validation tool.

## Getting Started

Run the UI app on visual studio or dotnet console.
Go to folder TFNValidation/ 
and run 
```
dotnet run .
```
and alter the config.js in src/ folder accordingly. URL for the API is driven from the config.js file. Please update it accordingly.

Run the API app on visual studio or dotnet console.
Go to folder TFNValidationAPI/ 
and run 
```
dotnet run .
```
After both of them are up and running, you could start playing around with it.

### Features

* Simple App to showcase my tech skills and my adaptability to any skill.
* UI to validate TFN.
* Separation of Concerns. UI, API and tests are separate.
* Dependency Injection
* SOLID principles
* Async Web API calls
* Cors enable allow all

### Assumptions

* API not Authenticated/Authorised.
* In memory caching.
* HTTP return codes are returned along with error/success response.
* 2 Tier Architecture - UI Layer and Business/API Layer
* SOLID principles followed as much as possible.
* Sufficient Unit Tests

### Improvements

* UI can be improved big time.
* Authentication/Authorization for the APIs
* Web API documentation using swagger
* Docker containers for the apps. Was not necessary for this challenge.

### Prerequisites

* Install node.js
* .NET Core 2.2 to be installed

## Running the tests

I have created a few unit tests which are found in the folder TFNValidationAPITests. This could be run manually. 

Go to Folder TFNValidationAPITests/ and run
```
dotnet test
```
There are 2 separate tests. One is the actual validation test which follws the correct algorithm. All tests should pass.
The other one tests the mock algorithm which has small changes. This test would fail everything.

### Deisgn Architrecture

2 Tier Architecture - UI Layer, Business/API Layer 

### Deisgn TEchnique

* Agile - split tasks into mini sprints
* TDD

## Built With

* C#, .NET Core 2.2 MVC/WebAPI
* React.js, css, bootstrap, javascript
* In Memory Caching
* Dependency Injection
* XUnit
