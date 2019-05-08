# accpod-api

This requires .NET Core 2.2 (https://dotnet.microsoft.com/download/dotnet-core/2.2)

## Building
Either:
1. Open in Visual Studio (2017 or later), change launch profile to *Api*, and run (F5).
2. Open the command-line, *cd* to the /Api folder, then *dotnet run --launch-profile Api*

You can then access the Api at https://localhost:5001/course using a tool like Postman.

To use in different environments change the launch profile argument to match the profile name in Api/Properties/launchSettings.json 
e.g. for Stage call *dotnet run --launch-profile Stage* which will make the api available at https://localhost:51997/ as well as reducing the console logging verbosity.

## Api Documentation
TODO - OpenApi endpoint.

## Data Store
TODO

## Architecture
The architecture is in a 3-tier system with each tier in its own project:
1. Api - Hosts Http/s endpoints and would also handle authentication.
2. Business - Business rules and processing.
3. Data - Database models. Entity Framework should be considered as part of this layer, and hence is directly accessed in the business layer.

At this stage only Integration tests are required as there is no business logic of significance to test. My philosophy on testing is outlined here: https://winterlimelight.com/2017/12/17/automated-testing-priorities/, 
and in the case of an API I consider Integraton tests paramount as they represent the consistent contract with callers (whereas unit tests represent implentation details).



