using System;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.Jira
{
    public class JiraSink: ILogEventSink, IDisposable
    {
        private readonly string username;
        private readonly string username;
        private readonly string jiraUrl;
        private readonly JiraTextFormatter _formatter;

        public JiraSink(JiraTextFormatter formatter)
        {
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}