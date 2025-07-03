using System;
using Serilog.Events;
using Serilog.Formatting;
using System.IO;
using System.Text.Json;

namespace Serilog.Sinks.Jira
{
    public class JiraTextFormatter: ITextFormatter
    {
        private readonly string _projectKey;
        private readonly string _issueType;

        public JiraTextFormatter(string projectKey, string issueType)
        {
            if (string.IsNullOrEmpty(projectKey)) throw new ArgumentNullException(nameof(projectKey));
            if (string.IsNullOrEmpty(issueType)) throw new ArgumentNullException(nameof(issueType));
            
            _projectKey = projectKey;
            _issueType = issueType;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (string.IsNullOrEmpty(_projectKey)) throw new ArgumentNullException(nameof(_projectKey));
            if (string.IsNullOrEmpty(_issueType)) throw new ArgumentNullException(nameof(_issueType));

            var jiraPayload = new
            {
                fields = new
                {
                    project = new { key = _projectKey },
                    summary = logEvent.RenderMessage(),
                    issuetype = new { name = _issueType }
                }
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(jiraPayload, options);
            output.WriteLine(json);
        }
    }
}