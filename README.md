# Serilog.Sinks.Jira #

Serilog Sink that sends log events to Jira

**Package** - [Serilog.Sinks.Jira](http://nuget.org/packages/serilog.sinks.jira) | **Platforms** - netstandard2.0, .NET Framework 4.6.1+

## Getting started ##

Enable the sink and log:

```csharp
var log = new LoggerConfiguration()
    .WriteTo.Jira(
        jiraApi:"https://{your-company-id}.atlassian.net/rest/api/3/issue",
        username: "username",
        password: "password",
        projectKey: "TEST",
        issueType: "Bug")
    .CreateLogger();

var position = new { Latitude = 25, Longitude = 134 };
var elapsedMs = 34;
log.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
```
## Log from ASP.NET Core & appsettings.json ##

Extra packages:

```shell
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Settings.Configuration
```

Add `UseSerilog` to the Generic Host:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration))
        .ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
        });
```

Add to `appsettings.json` configuration:

```json
{
    "Serilog": {
        "Using": [ "Serilog.Sinks.Jira" ],
        "MinimumLevel": "Error",
        "WriteTo": [{
            "Name": "Jira",
            "Args": {
                "jiraApiUrl": "https://{your-company-id}.atlassian.net/rest/api/3/issue",
                "username": "username",
                "password": "password",
                "projectKey": "TEST",
                "issueType": "Bug"
            }
        }]
    }
}
```