# Application Health Checks of ASP.NET Core applications

### Overview
The examples of application health checks are presented using sample ASP.NET Core applications, targeting .NET framework 6.0:

The solution contains 4 applications:
- A frontend MVC .NET Core 6.0 application called Investment Manager coded originally by Pluralsight Author David Berry (https://app.pluralsight.com/profile/author/david-berry). This frontent application consists of 3 projects: InvestmentManager, InvestmentManager.Core and InvestmentManager.DataAccess.EF.
- A backend API, called by the frontend applicaton to retrieve stock index data: 1 project named StockIndexWebService
- HealthCheckAlerter is a .NET Core web application containing the RabbitMQ.Client NuGet package to process health check messages, display their status and send alert messages
- IdentityServer used to provide OAuth2 JWT access token for authorizing health check endpoint
<p align="left">
  <img src="documentation/01 solution in VS.png" alt="Solution in Visual Studio" title="Solution in Visual Studio" />
</p>
- The frontend application can display investment accounts with asset allocation and account history charts, along with stock index data in the top row received from the API:
<p align="left">
  <img src="documentation/02 investment account list.png" alt="Investment account list" title="Investment account list" />
</p>
<p align="left">
  <img src="documentation/03 investment account charts.png" alt="Investment account charts" title="Investment account charts" />
</p>

- Most code and health check examples are based on the Pluralsight course ASP.NET Core 3 Health Checks created by Rag Dhiman (https://app.pluralsight.com/profile/author/rag-dhiman), whereas all applications are updated to target .NET framework 6.0


&nbsp;  
### Application health check examples

#### Liveness health checks

Liveness health check using the MapHealthChecks extension method (framework: Microsoft.AspNetCore.App, assembly: Microsoft.Extensions.Diagnostics.HealthChecks)
Endpoint: /health
<p align="left">
  <img src="documentation/04a liveness health check - code.png" alt="Liveness health check - code" title="Liveness health check - code" />
</p>
<p align="left">
  <img src="documentation/04b liveness health check.png" alt="Liveness health check - response" title="Liveness health check - response" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/8cc5d1d55e2e55a70b1f8bd13588b0f2c0e93297

&nbsp;  
&nbsp;  
Liveness health check for specific host name(s) and port number(s), using RequireHost extension method (framework: Microsoft.AspNetCore.App, assembly: Microsoft.AspNetCore.Routing)	
Endpoint: /health-on-host
<p align="left">
  <img src="documentation/05a liveness health check for specific host and port - code.png" alt="Liveness health check for specific host and port - code" title="Liveness health check for specific host and port - code" />
</p>
<p align="left">
  <img src="documentation/05b liveness health check for specific host and port.png" alt="Liveness health check for specific host and port - response" title="Liveness health check for specific host and port - response" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/05571ebc06538efe697d9982a9f9c25a0432487a

&nbsp;  
&nbsp;  
#### Readiness health checks

Dependency health check of SQL server using the AddCheck extension method (framework: Microsoft.AspNetCore.App, assembly: Microsoft.Extensions.Diagnostics.HealthChecks)	
Health check name: "SQLServer in startup" 
<p align="left">
  <img src="documentation/06 SQL server health check in startup.png" alt="SQL server health check in startup" title="SQL server health check in startup" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/fcd93e3098a7dd38378a257775f0fee4726a0608

&nbsp;  
&nbsp;  
Add SQL server health check using the AddSqlServer extension method (NuGet package: AspNetCore.HealthChecks.SqlServer, assembly: HealthChecks.SqlServer)
Health check name: "SqlServer"
<p align="left">
  <img src="documentation/07 SqlServer health check with SqlServer package.png" alt="Add SqlServer health check with SqlServer package" title="Add SqlServer health check with SqlServer package" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/af69c0e76263e58cbbd6be96309b1261aa3fe3c7

&nbsp;  
&nbsp;  
Add endpoint health check, using the AddUrlGroup extension method (NuGet package AspNetCore.HealthChecks.Uris, assembly: HealthChecks.Uris)
Health check name: "Stock Index API Health Check"
<p align="left">
  <img src="documentation/08 endpoint health check with Uris package.png" alt="Add endpoint health check with Uris package" title="Add endpoint health check with Uris package" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/29ec863022a78844e51a35f66ee99981d713370f

&nbsp;  
&nbsp;  
Health check with customized status codes, using the property ResultStatusCodes of type IDictionary <HealthStatus, int> (framework Microsoft.AspNetCore.App, assembly: Microsoft. AspNetCore.Diagnostics.HealthChecks)
- for Degraded healtch status it changes the status code to 500, instead of default 200
Endpoint: /health-customized-status-code
<p align="left">
  <img src="documentation/09a health check with customized status code - code.png" alt="Health check with customized status code - code" title="Health check with customized status code - code" />
</p>
<p align="left">
  <img src="documentation/09b health check with customized status code.png" alt="Health check with customized status code - response" title="Health check with customized status code - response" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/bed27ce63d17b09104fd77eb5b349521780c723a

&nbsp;  
&nbsp;  
Create separate endpoints for differently tagged health checks
- /health/ready endpoint for health checks with "ready" tag
- /health/live endpoint for health checks without "ready" tag
10 create health check endpoints with and without ready tag.png
<p align="left">
  <img src="documentation/10 create health check endpoints with and without ready tag.png" alt="Create health check endpoints with and without ready tag" title="Create health check endpoints with and without ready tag" />
</p>

Create separate endpoints for differently tagged health checks:
- /health/live endpoint:
<p align="left">
  <img src="documentation/10a customize content of health-live endpoint - code.png" alt="10a customize content of /health-live endpoint - code" title="10a customize content of /health-live endpoint - code" />
</p>
<p align="left">
  <img src="documentation/10b customize content of health-live endpoint.png" alt="Customize content of /health/live endpoint - response" title="Customize content of /health/live endpoint - response" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/290fa2da9d5fe45249866771fad939eb25f6c31e

&nbsp;  
&nbsp;  
Customize the content and format of health check responses:
- /health/ready endpoint:
<p align="left">
  <img src="documentation/11a customize content of health-ready endpoint - code.png" alt="Customize content of /health/ready endpoint - code" title="Customize content of /health/ready endpoint - code" />
</p>
<p align="left">
  <img src="documentation/11b customize content of health-ready endpoint.png" alt="Customize content of /health/ready endpoint - response" title="Customize content of /health/ready endpoint - response" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/18a884e1f09f4dc5d2565724bd1d61e878d1a59d

&nbsp;  
&nbsp;  
Add file path write health check using a class instance
- health check name: "File Path Health Check class"
<p align="left">
  <img src="documentation/12a file path write check with class instance - code.png" alt="Add file path write check with class instance - code" title="Add file path write check with class instance - code" />
</p>
<p align="left">
  <img src="documentation/12b file path write check with class instance.png" alt="Add file path write check with class instance - response" title="Add file path write check with class instance - response" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/5bce80b32e1cd398e7cf599e2296e5a619cd7b62

&nbsp;  
&nbsp;  
Add file path write health check using the extension method AddFilePathWrite
- health check name: "File Path Health Check extension"
<p align="left">
  <img src="documentation/13a file path write check with extension method - code.png" alt="Add file path write check with extension method - code" title="Add file path write check with extension method - code" />
</p>
<p align="left">
  <img src="documentation/13b file path write check with extension method.png" alt="Add file path write check with extension method - response" title="Add file path write check with extension method - response" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/332abf318219f73d5f27a455f63be109e8599baf

&nbsp;  
&nbsp;  
Add authorization policy to the health/ready endpoint, using the RequireAuthorization extension method (framework: Microsoft.AspNetCore.App, assembly: Microsoft.AspNetCore.Authorization.Policy). It uses OAuth2 authorization type with JSON Web Token authentication provided by IdentityServer.
- add authorization policy to health check endpoint:
<p align="left">
  <img src="documentation/14a add authorization policy to health check endpoint.png" alt="Add authorization policy to health check endpoint" title="Add authorization policy to health check endpoint" />
</p>

- configure JWT token request in Postman:
<p align="left">
  <img src="documentation/14b configure token request in Postman.png" alt="Configure token request in Postman" title="Configure token request in Postman" />
</p>
- JWT token issued by IdentityServer:
<p align="left">
  <img src="documentation/14c token issued by IdentityServer.png" alt="JWT token issued by IdentityServer" title="JWT token issued by IdentityServer" />
</p>

- JWT token received in Postman:
<p align="left">
  <img src="documentation/14d token received in Postman.png" alt="JWT token received in Postman" title="JWT token received in Postman" />
</p>

- Request to health check endpoint successful with the JWT token:
<p align="left">
  <img src="documentation/14e request with token successful in Postman.png" alt="Request with token successful in Postman" title="Request with token successful in Postman" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/f70e7215c933d893f6563854c2da8557e5691c08

&nbsp;  
&nbsp;  
Add endpoint health checks as Controller action methods
- the /health/controller endpoint returns customized status values of the enum type HealhStatus (framework Microsoft.AspNetCore.App, assembly: Microsoft. AspNetCore.Diagnostics.HealthChecks.Abstractions), which represents the reported status of a health check result:
<p align="left">
  <img src="documentation/15a customized HealthStatus values.png" alt="Customized HealthStatus values" title="Customized HealthStatus values" />
</p>

- the /health/controller endpoint returns the customized property values of the HealthReport class (framework Microsoft.AspNetCore.App, assembly: Microsoft. AspNetCore.Diagnostics.HealthChecks.Abstractions), such as aggregate Status of health checks, Totalduration, as well as values of the Entries property such as Name, Status, Duration, Description and  Exception of any individual health check:
<p align="left">
  <img src="documentation/15b customized property values of the HealthReport class.png" alt="Customized property values of the HealthReport class" title="Customized property values of the HealthReport class" />
</p>

- the final result is formatted by TextWriter using the enum type Formating option Indented (package: NewtonSoft.Json, assembly: Newtonsoft.Json):
<p align="left">
  <img src="documentation/15c property values of the HealthReport class.png" alt="Property values of the HealthReport class" title="Property values of the HealthReport class" />
</p>

- as a result each child objects appears in a new line indented:
<p align="left">
  <img src="documentation/15d final results of health-controller endpoint.png" alt="Final results of /health/controller endpoint" title="Final results of /health/controller endpoint" />
</p>

- if the StockIndexWebService API is not available, the endpoint returns Degraded Status for this health check, along with Exception information:
<p align="left">
  <img src="documentation/15e final results with Degraded status.png" alt="Final results with Degraded status" title="Final results with Degraded status" />
</p>

- if the physical path for health check FilePathWrite is not available, the endpoint returns Faiure Status for this health check, along with Exception information and the information defined in the Data dictionary:
<p align="left">
  <img src="documentation/15f final results with Failure status.png" alt="Final results with Failure status" title="Final results with Failure status" />
</p>

- the /healthcheck endpoint redirects to the ./health/controller endpoint
<p align="left">
  <img src="documentation/15g healthcheck endpoint redirects to health-controller endpoint.png" alt="The /healthcheck endpoint redirects to ./health/controller endpoint" title="The /healthcheck endpoint redirects to ./health/controller endpoint" />
</p>

- the /health/ping endpoint returns 200 (OK) status code to indicate that the dependency app is reachable:
<p align="left">
  <img src="documentation/15h health-ping endpoint.png" alt="The /health/ping endpoint returns status code 200" title="The /health/ping endpoint returns status code 200" />
</p>

- the /health/assembly endpoint returns assembly-related information, such as Application name, BuildDate, ProductVerson, AssembyVersion
<p align="left">
  <img src="documentation/15i health-assembly endpoint - code.png" alt="The /health/assembly endpoint - code" title="The /health/assembly endpoint - code" />
</p>
<p align="left">
  <img src="documentation/15j health-assembly endpoint.png" alt="The /health/assembly endpoint - response" title="The /health/assembly endpoint - response" />
</p>

- the controller endpoints are by default mapped in the http pipeline, using the MapControllerRoute extension method; (if needed, you can use the endpoints.MapControllers extension method):
<p align="left">
  <img src="documentation/15k controller endpoints are mapped in Program.cs.png" alt="Controller endpoints are mapped in Program.cs" title="Controller endpoints are mapped in Program.cs" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/2020836690ba46138d9ca47aa686a7fd97f04b2c

&nbsp;  
&nbsp;  
Add endpoint health checks defined in extension method
- The /health/extension endpoint returns the same health check results in the same format as the above health/controller endpoint, only the implementation is different. The MapEndpointHealthChecks extension method contains the /health/extension, /healthcheck, /health/ping and health/assembly endpoints:
<p align="left">
  <img src="documentation/16a health check extension method.png" alt="Health check extension method" title="Health check extension method" />
</p>

- The extension method is added as endpoint to the http middleware pipeline:
<p align="left">
  <img src="documentation/16b add endpoint health checks as extension method.png" alt="Add health check extension method as endpoint to the http pipeline" title="Add health check extension method as endpoint to the http pipeline" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/8529747653e4cebb3771ed2b69591bae351046fe

&nbsp;  
&nbsp;  
Add SQL server health checks defined in extension method
- The health check in the AddSqlServerCheckThroughSqlCommand class uses SqlCommand to check the availability of the database server:
<p align="left">
  <img src="documentation/17a SqlServerHealthCheckThroughSqlCommand class.png" alt="SqlServerHealthCheckThroughSqlCommand class" title="SqlServerHealthCheckThroughSqlCommand class" />
</p>

- The health check in the  AddSqlServerCheckThroughDbContext class uses  DbContext of Entity Framework to check the availability of the database server.

- It implements the IHealthCheck interface with the CheckHealthAsync (framework Microsoft.AspNetCore.App, assembly: Microsoft. AspNetCore.Diagnostics.HealthChecks.Abstractions), which returns Task<HealthCheckResult>, which struct type represents the results of a health check: 
<p align="left">
  <img src="documentation/17b SqlServerHealthCheckThroughDbContext class.png" alt="SqlServerHealthCheckThroughDbContext class" title="SqlServerHealthCheckThroughDbContext class" />
</p>

- Finally, we create extension methods to add these health checks as endpoints, adding them to the IHealthCheckBuilder interface (framework: Microsoft.AspNetCore.App, assembly: Microsoft.Extensions.Diagnostics.HealthChecks), used to register health checks). The class must be added as a scoped service to the IserviceCollection, and added as a new health check using the AddCheck extension method:
<p align="left">
  <img src="documentation/17c create extension methods to add Sql server health checks.png" alt="Create extension methods to add Sql server health checks" title="Create extension methods to add Sql server health checks" />
</p>

- This way these extension methods can be added as health checks to the http pipeline, where the AddHealthChecks extension method (framework: Microsoft.AspNetCore.App, assembly: Microsoft.Extensions.Diagnostics.HealthChecks) is used to add it as HealthCheckService to the container:
<p align="left">
  <img src="documentation/17d add extension methods to pipeline.png" alt="Add extension methods to pipeline" title="Add extension methods to pipeline" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/e957185c0f51ea4f09aceadbfdd6274e7142ffb0

&nbsp;  
&nbsp;  
Add CORS policy to the health/live endpoint
- we specify origins (URLs) allowed to access this healtch check endpoint, using the RequireCors extension method (framework: Microsoft.AspNetCore.App, assembly: Microsoft.AspNetCore.Cors):
<p align="left">
  <img src="documentation/18a add CORS policy to the health-live endpoint.png" alt="Add CORS policy to the health-live endpoint" title="Add CORS policy to the health-live endpoint" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/6bf7bb0ce983c5e52a11e2b4806e0d465f9a1876

&nbsp;  
&nbsp;  
Add a health check User Interface at /healthchecks-ui endpoint
In the InvestmentManager project,
- add package AspNetCore.HealthChecks.UI (6.0.5) to display health check information in the browser,
- add package AspNetCore.HealthChecks.UI.InMemory.Storage (6.0.5) to store health check results in memory, and
- add package AspNetCore.HealthChecks.UI.Client (6.0.5) that supports writing the UI response
<p align="left">
  <img src="documentation/19a add packages for health check user interface.png" alt="Add packages for health check user interface" title="Add packages for health check user interface" />
</p>
Configure HealthChecksUI as a service in Program.cs:
- add HealthChecksUI as a service with in-memory storage:
- add health check endoint /healthui for HealthChecksUI:
- configure HealthChecksUI in appsettings.json
<p align="left">
  <img src="documentation/19b add HealthChecksUI with in memory storage.png" alt="Add HealthChecksUI with in memory storage" title="Add HealthChecksUI with in memory storage" />
</p>

- configure HealthChecksUI in appsettings.json:
<p align="left">
  <img src="documentation/19c configure HealthChecksUI in appsettings.json.png" alt="Configure HealthChecksUI in appsettings.json" title="Configure HealthChecksUI in appsettings.json" />
</p>

- as a result we can see the health check results at the specified /healthui endpoint:
<p align="left">
  <img src="documentation/19d health check results at the specified healthui endpoint.png" alt="Health check results at the specified /healthui endpoint" title="Health check results at the specified /healthui endpoint" />
</p>

- configure Health Checks UI with specific path:
<p align="left">
  <img src="documentation/19e configure HealthChecksUI with specific path.png" alt="Configure HealthChecksUI with specific path" title="Configure HealthChecksUI with specific path" />
</p>

- as a result, we can see the health check results on the user interface:
<p align="left">
  <img src="documentation/19f health check results on the user interface.png" alt="Health check results on the user interface" title="Health check results on the user interface" />
</p>

- if the API is not accessible, Degraded status is indicated:
<p align="left">
  <img src="documentation/19g health check results with Degraded status.png" alt="Health check results with Degraded status" title="Health check results with Degraded status" />
</p>

- if the file path is not accessible, Unhealthy status is indicated:
<p align="left">
  <img src="documentation/19h health check results with Unhealthy status.png" alt="Health check results with Unhealthy status" title="Health check results with Unhealthy status" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/6a7fc7a2680d6e7134ee1f28f08733a7781bd2fd

&nbsp;  
&nbsp;  
Limit the rate of calls made to the health/ready endpoint
- add package AspNetCoreRateLimit (5.0.0):
<p align="left">
  <img src="documentation/20a add AspNetCoreRateLimit package.png" alt="Add AspNetCoreRateLimit package" title="Add AspNetCoreRateLimit package" />
</p>

- create the RateLimit.ConfigureServices method to configure the package and the services it requires:
<p align="left">
  <img src="documentation/20b create the RateLimit.ConfigureServices method.png" alt="Create the RateLimit.ConfigureServices method" title="Create the RateLimit.ConfigureServices method" />
</p>

- add and configure services required for the AspNetCoreRateLimit package:
<p align="left">
  <img src="documentation/20c configure AspNetCoreRateLimit in Program.cs.png" alt="Configure AspNetCoreRateLimit in Program.cs" title="Configure AspNetCoreRateLimit in Program.cs" />
</p>

- configure IpRateLimining in appsettings.json:
<p align="left">
  <img src="documentation/20d configure IpRateLimining in appsettiong.json.png" alt="Configure IpRateLimining in appsettiong.json in Program.cs" title="Configure IpRateLimining in appsettiong.json" />
</p>

- as a result, when we try to access the health/ready endpoint within 10 seconds, our query will be rejected with this warning:
<p align="left">
  <img src="documentation/20e frequent query rejected.png" alt="Frequent query within 10 seconds rejected" title="Frequent query within 10 seconds rejected" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/2ed61287ff7f37c6ca8f92e949055eb9e1fbb8ea

&nbsp;  
&nbsp;  
Add functionality to publish health check information
- add package RabbitMQ.Client (5.1.1):
<p align="left">
  <img src="documentation/21a add package RabbitMQ.Client.png" alt="Add package RabbitMQ.Client" title="Add package RabbitMQ.Client" />
</p>

- add package component IQueueMessage interface for queuing health check information (into QueueMessage folder):
<p align="left">
  <img src="documentation/21b add IQueueMessage interface.png" alt="Add IQueueMessage interface" title="Add IQueueMessage interface" />
</p>

- add RabbitMQQueueMessage class, facilitating to send queue messages:
<p align="left">
  <img src="documentation/21c add RabbitMQQueueMessage class.png" alt="Add RabbitMQQueueMessage class" title="Add RabbitMQQueueMessage class" />
</p>
- add HealthCheckQueuePublisher class containing the publishing logic:
<p align="left">
  <img src="documentation/21d add HealthCheckQueuePublisher class.png" alt="Add HealthCheckQueuePublisher class" title="Add HealthCheckQueuePublisher class" />
</p>

- configure HealthCheckPublisher options using the Configure extension method (framework: Microsoft.AspNetCore.App, assembly: Microsoft.Extensions.Options), 
and add its components as a service to IServiceCollection in Program.cs:
<p align="left">
  <img src="documentation/21e configure HealthCheckPublisher and its dependencies.png" alt="Configure HealthCheckPublisher and its dependencies" title="Configure HealthCheckPublisher and its dependencies" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/91cd79583edefc5b81e77f876e0be0a57e01a314

&nbsp;  
&nbsp;  
Add HealthCheckAlerter project
- add package Microsoft.Extensions.Hosting (6.0.1)
- add package Newtonsoft.Json (13.0.3)
- add package RabbitMQ.Client  (5.1.0)
<p align="left">
  <img src="documentation/22a add packages to HealthCheckAlerter project.png" alt="Add packages to HealthCheckAlerter project" title="Add packages to HealthCheckAlerter project" />
</p>

- add Worker.cs to implement a long-running IHostedService (https://github.com/sztrelcsikzoltan/application-health-checks/blob/master/InvestmentManager/HealthCheckAlerter/Worker.cs)
- add Worker as a hosted service in Program.cs:
<p align="left">
  <img src="documentation/22b add Worker as a hosted service in Program.cs.png" alt="Add Worker as a hosted service in Program.cs" title="Add Worker as a hosted service in Program.cs" />
</p>

- install RabbitQM on the server from PowerShell (choco install rabbitmq) and allow local firewall access
- as a result, the HealthCheckAlerter app monitors the status of the health check messages:
<p align="left">
  <img src="documentation/22c HealthCheckAlerter monitors the health check messages.png" alt="HealthCheckAlerter monitors the health check messages" title="HealthCheckAlerter monitors the health check messages" />
</p>

- the application sends e-mail alert message if the healh status is not Healthy:
<p align="left">
  <img src="documentation/22d HealthCheckAlerter sends alert message.png" alt="HealthCheckAlerter sends alert message" title="HealthCheckAlerter sends alert message" />
</p>

- the health messages can be monitored in RabbitMQ Server UI:
<p align="left">
  <img src="documentation/22e health messages can be monitored in RabbitMQ server UI.png" alt="Health messages can be monitored in RabbitMQ server UI" title="Health messages can be monitored in RabbitMQ server UI" />
</p>

- the message content can be displayed on the RabbitMQ UI:
<p align="left">
  <img src="documentation/22f health messages can be displayed in RabbitMQ server UI.png" alt="Health messages can be displayed in RabbitMQ server UI" title="Health messages can be displayed in RabbitMQ server UI" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/9e424b667e6d323594f448ef52cc956292c1ed80

&nbsp;  
&nbsp;  
Add database storage for health check information
- add package AspNetCore.HealthChecks.UI.SqlServer.Storage (6.0.5) to the InvestmentManager project:
<p align="left">
  <img src="documentation/23a add health check database storage to the project.png" alt="Add health check database storage to the project" title="Add health check database storage to the project" />
</p>

- add connection string to HealthChecks database in appsettings.json (the third-party package will automatically create the database):
<p align="left">
  <img src="documentation/23b add connection string to HealthChecks database.png" alt="Add connection string to HealthChecks database" title="Add connection string to HealthChecks database" />
</p>

- add health check UI with db storage in Program.cs; it works only if AddInMemoryStorage is not added:
<p align="left">
  <img src="documentation/23c add database storage for HealthChecksUI.png" alt="Add database storage for HealthChecksUI" title="Add database storage for HealthChecksUI" />
</p>
- as a result, the HealthChecks database is created and the current health check information will be stored there:
<p align="left">
  <img src="documentation/23d HealthChecks database created.png" alt="HealthChecks database created" title="HealthChecks database created" />
</p>
<p align="left">
  <img src="documentation/23e health check history stored in database.png" alt="Health check history stored in database" title="Health check history stored in database" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/4def2954d1eddb1295054f3850c99a94c7b67bf6

&nbsp;  
&nbsp;  
Add build date information as a custom AssemblyAttribute
- define the custom attribute BuildDateAttribute with DateTime property and to retrieve its value, using the GetCustomAttribute extension method (defined in the System.Reflection.CustomAttributeExtension class):
<p align="left">
  <img src="documentation/24a define BuildDateAttribute.png" alt="Define BuildDateAttribute" title="Define BuildDateAttribute" />
</p>

- make InvestmentManager.HealthChecks as a global namespace in Program.cs, so that it will be available in the auto-generated AssembyInfo.cs file:
<p align="left">
  <img src="documentation/24b make namespace HealthChecks global.png" alt="Make namespace HealthChecks global" title="Make namespace HealthChecks global" />
</p>

- include BuildDateAttribute as AssemblyAttribute in the project file:
<p align="left">
  <img src="documentation/24c include BuildDateAttribute in the project.png" alt="Include BuildDateAttribute in the project" title="Include BuildDateAttribute in the project" />
</p>

- add build date of the assembly to the health/assembly endpoint in HealthCheckController.cs and EndPointHealthChecks.cs:
<p align="left">
  <img src="documentation/24d add BuildDate to health-assembly endpoint.png" alt="Add BuildDate to /health/assembly endpoint" title="Add BuildDate to /health/assembly endpoint" />
</p>

- as a result, the health/endpoint provides this information as well:
<p align="left">
  <img src="documentation/24e health-assembly endpoint with BuildDate.png" alt="The /health/assembly endpoint with BuildDate information" title="The /health/assembly endpoint with BuildDate information" />
</p>
Code: https://github.com/sztrelcsikzoltan/application-health-checks/commit/5b2f60b3bd6de7ad3834756fc93fe6d9b5aef487

