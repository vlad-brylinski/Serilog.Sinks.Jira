using System;
using System.Net;
using Serilog.Configuration;
using Serilog.Sinks.Jira;

namespace Serilog
{
    public static class JiraExtensions
    {
        public static LoggerConfiguration Jira(
            this LoggerSinkConfiguration sinkConfiguration,
            string jiraApi,
            string username,
            string password,
            string projectKey,
            string issueType,
            string applicationId = null,
            string hostName = null
            )
        {
            var envName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
                          Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                          Environment.GetEnvironmentVariable("ASPNET_ENVIRONMENT");
            
            return sinkConfiguration.Sink(new JiraSink(jiraApi, username, password, new JiraTextFormatter( applicationId, hostName, envName)));
        }
    } 
}