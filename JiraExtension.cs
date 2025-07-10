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
            if (applicationId == null) applicationId = "unknown";
            if (hostName == null) hostName = Dns.GetHostName().ToLower();
            
            var envName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
                          Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                          Environment.GetEnvironmentVariable("ASPNET_ENVIRONMENT");

            return sinkConfiguration.Http(
                requestUri: jiraApi,
                queueLimitBytes: null,
                batchFormatter: new JiraBatchFormatter(projectKey, issueType),
                textFormatter:  new JiraTextFormatter( applicationId, hostName, envName),
                httpClient: new JiraHttpClient(username, password));
        }
    } 
}