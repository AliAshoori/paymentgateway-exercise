## Dev Tools 
   This solution is built using Visual Studio 2019, .NET 5. This also uses SQL Server v18.6 to store data on.
## How to run on a local machine
   1. Make sure you have the above tools already installed on your machine
   2. Simply clone the repository on your machine
   3. Build the solution 
   4. Hit F5 to run your solution, you will see a console coming up logging out events using Serilog. Also, you shall see a browser displaying the Swagger documentation
   
 Note: For the first run, it may take a few seconds as it needs to provision the database on your `local` server. 
 ## Bank simulator
 The `infrastructure` layer contains a set of mock services that pretend to be a bank API and mocks out various responses each time according to some confitions. Read more on this in the assumption section below.
 ## Assumptions
 1. There a set of services in the `infrastructure` pretending to be the acquiring bank mocking out different responses according to the value of the amount property. Three different conditions have been considered for this: 
    - Normal Scenrario - wherein the `amount` value must be less than `1000` 
    - Service unavailable - wherein the `amount` value must begreater than or equal to `1,000,000` and finally  
    - Slow processing - wherein the `amount` should be greater than or equale to `1000` and less than `1,000,000`
 3. The `Payment` table contains a property named `MerchantId`, this references no other table just for the sake of simpllicty and in favour of reducing development time, especially that database design was not a mandatory criteria
 ## Some principles exercies in the codebase
 1. Using `mediatr` nuget package and notion of `commands` and `queries` 
 2. Trying to practice `onion architecture`
 3. Trying to respect the `RESTful` principles from unified resource identifier to content negotation and HATEOS
 4. Unit testing and integration testing
 5. Code coverage using `Fine Code Coverage`
## Extra miles
1. Adding GitHub actions to have a build script that builds all the projects and runs the test projects too - the details can be accessed via the  `Actions` tab from the top menu
2. Database design - using entity framework core code first approach and its encryption feature to mask the sensitive data even on the rest
3. Used fixed URLs for the APIs configured in the `Program.cs`
4. Using helpful external libraries such as `FluentAssertions` and `FluentValidations` in test project and application services,respectively
## Potential improvement 
1. Taking advantage of an identity server to exercise OAtuh2 Client Credentials flow when it comes to calling the APIs
2. Re-considering the test coverage for any potential improovements
3. Building a separate project to act as a bank simulator which also carries some sort of logic that could make the whole solutino more realistic
4. Using Docker to containerize the solution 
5. Although the code coverage is displayed and reported through the logs when a build runs, that would have been more helpful to export it to `coveralls.io` for visualizations
6. We use Serilog and log everything to console, that would have been a more complete solution to use ElasticSearch and Kibana to visualize the logs making it easier to search and filter and to also implement some monitoring tools 
7. To have an API gateway sitting in front of the APIs and it could carry out the security logics as well as acting like a load balaner
## Code coverage on local - screenshot
<img width="927" alt="code-coverage-local" src="https://user-images.githubusercontent.com/7995157/155894150-31b4e7f5-53a9-4871-bbd8-f767155a350c.PNG">


## API response on local - showing off the HATEOS
<img width="815" alt="api" src="https://user-images.githubusercontent.com/7995157/155894130-566ae4e0-6d1b-414b-907f-1cf19232d1d5.PNG">


## Test coverage report from within the build logs
<img width="717" alt="code-coverage-sample" src="https://user-images.githubusercontent.com/7995157/155894169-508f0140-cbd5-4b49-abcf-10fc597f6896.PNG">

   
