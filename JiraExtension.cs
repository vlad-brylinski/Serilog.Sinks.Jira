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
            string issueType
            )
        {
            return sinkConfiguration.Http(
                requestUri: jiraApi,
                queueLimitBytes: null,
                textFormatter:  new JiraTextFormatter(projectKey, issueType),
                httpClient: new JiraHttpClient(username, password));
        }
    } 
}