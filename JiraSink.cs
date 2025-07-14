using System;
using System.IO;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.Jira
{
    public class JiraSink: ILogEventSink, IDisposable
    {
        private readonly JiraTextFormatter _formatter;
        private readonly JiraHttpClient _httpClient;

        public JiraSink(string username, string password, string jiraUrl, JiraTextFormatter formatter)
        {
            _formatter = formatter;
            _httpClient = new JiraHttpClient(username, password, jiraUrl);
        }

        public void Emit(LogEvent logEvent)
        {
            var output = new StringWriter();
            _formatter.Format(logEvent, output);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}