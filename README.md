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
Api documentation generated via OpenApi can be viewed in a browser at *baseurl/swagger* e.g. https://localhost:5001/swagger

## Data Store
An Sqlite database will be automatically created at Api/accpod.db when the website is started.

## Architecture
The architecture is in a 3-tier system with each tier in its own project:
1. Api - Hosts Http/s endpoints and would also handle authentication.
2. Business - Business rules and processing. This separates read (queries) and write (command) operations which may allow read operations to be cached or operate on a read-replica store should performance demand that in the future.
3. Data - Database models and data access.

At this stage only Integration tests have been created as there is no business or api logic of significance to unit test.

Test Coverage:
1. Download and extract OpenCover https://github.com/OpenCover/opencover/releases/download/4.7.922/opencover.4.7.922.zip
2. Download and extract ReportGenerator https://github.com/danielpalme/ReportGenerator/releases/download/v4.1.5/ReportGenerator_4.1.5.zip
3. Change the paths to the above at the top of test-coverage.ps1
4. Run test-coverage.ps1 and view the results by opening ./Coverage/index.htm

Please note that OpenCover shows reduced branch coverage on await statements as described here: https://github.com/OpenCover/opencover/issues/881




