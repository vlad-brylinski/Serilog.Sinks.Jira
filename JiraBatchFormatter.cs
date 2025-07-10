using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Serilog.Sinks.Http;

namespace Serilog.Sinks.Jira
{
    public class JiraBatchFormatter : IBatchFormatter
    {
        private readonly string _projectKey;
        private readonly string _issueType;

        public JiraBatchFormatter(string projectKey, string issueType)
        {
            if (string.IsNullOrEmpty(projectKey)) throw new ArgumentNullException(nameof(projectKey));
            if (string.IsNullOrEmpty(issueType)) throw new ArgumentNullException(nameof(issueType));

            _projectKey = projectKey;
            _issueType = issueType;
        }

        public void Format(IEnumerable<string> logEvents, TextWriter output)
        {
            if (logEvents == null) throw new ArgumentNullException(nameof(logEvents));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (string.IsNullOrEmpty(_projectKey)) throw new ArgumentNullException(nameof(_projectKey));
            if (string.IsNullOrEmpty(_issueType)) throw new ArgumentNullException(nameof(_issueType));

            // Abort if sequence of log events is empty
            if (!logEvents.Any())
            {
                return;
            }

            var buffer = new StringWriter();

            buffer.Write("[");

            var delimStart = string.Empty;

            foreach (var logEvent in logEvents)
            {
                if (string.IsNullOrWhiteSpace(logEvent))
                {
                    continue;
                }

                buffer.Write(delimStart);
                buffer.Write(logEvent);
                delimStart = ",";
            }

            buffer.Write("]");

            var jiraPayload = new
            {
                fields = new
                {
                    project = new { key = _projectKey },
                    summary = $"New {_issueType}",
                    description = new
                    {
                        type = "doc",
                        version = 1,
                        content = new[]
                        {
                            new
                            {
                                type = "paragraph",
                                content = new[]
                                {
                                    new
                                    {
                                        type = "text",
                                        text = buffer.ToString()
                                    }
                                }
                            }
                        }
                    },
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