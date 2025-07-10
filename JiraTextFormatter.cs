using System;
using System.Collections.Generic;
using Serilog.Events;
using Serilog.Formatting;
using System.IO;
using System.Linq;
using Serilog.Debugging;
using Serilog.Formatting.Json;

namespace Serilog.Sinks.Jira
{
    public class JiraTextFormatter: ITextFormatter
    {
        private readonly string _applicationId;
        private readonly string _hostName;
        private readonly string _envName;

        public JiraTextFormatter(string applicationId, string hostName, string envName)
        {
            _applicationId = applicationId;
            _hostName = hostName;
            _envName = envName;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            try
            {
                var buffer = new StringWriter();
                FormatContent(logEvent, buffer);
                output.WriteLine(buffer.ToString());
            }
            catch (Exception ex)
            {
                LogNonFormattableEvent(logEvent, ex);
            }
        }
        
        private void FormatContent(LogEvent logEvent, TextWriter output)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.Write("{\"timestamp\":\"");
            output.Write(logEvent.Timestamp.ToUnixTimeMilliseconds());

            output.Write("\",\"level\":\"");
            output.Write(logEvent.Level);

            output.Write("\",\"application.id\":\"");
            output.Write(_applicationId);

            output.Write("\",\"host.name\":\"");
            output.Write(_hostName);

            if (_envName != null) 
            {
                output.Write("\",\"environment\":\"");
                output.Write(_envName);                
            }

            output.Write("\",\"content\":");
            var message = logEvent.RenderMessage();
            JsonValueFormatter.WriteQuotedJsonString(message, output);

            if (logEvent.Exception != null)
            {
                output.Write(",\"exception\":");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }

            if (logEvent.Properties.Count != 0)
            {
                WriteProperties(logEvent.Properties, output);
            }

            output.Write('}');
        }

        private static void WriteProperties(
            IReadOnlyDictionary<string, LogEventPropertyValue> properties,
            TextWriter output, string prefixKey = "")
        {
            foreach (var property in properties)
            {
                var flatKey = prefixKey + property.Key;
                switch (property.Value) 
                {
                    case ScalarValue scalar:
                        output.Write(",");
                        JsonValueFormatter.WriteQuotedJsonString(flatKey, output);
                        output.Write(':');
                        JsonValueFormatter.WriteQuotedJsonString(Convert.ToString(scalar.Value), output); // Only values of the String type are supported
                        break;
                    case SequenceValue sequence:
                        int seq = 0;
                        WriteProperties(sequence.Elements.ToDictionary(e => (seq++).ToString(), e => e), output, flatKey + ".");
                        break;
                    case StructureValue structure:
                        WriteProperties(structure.Properties.ToDictionary(p => p.Name, p => p.Value), output, flatKey + ".");
                        break;
                    case DictionaryValue dictionary:
                        WriteProperties(dictionary.Elements.ToDictionary(e => e.Key.Value.ToString(), e => e.Value), output, flatKey + ".");
                        break;
                }
            }
        }

        private static void LogNonFormattableEvent(LogEvent logEvent, Exception e)
        {
            SelfLog.WriteLine(
                "Event at {0} with message template {1} could not be formatted into JSON and will be dropped: {2}",
                logEvent.Timestamp.ToString("o"),
                logEvent.MessageTemplate.Text,
                e);
        }

    }
}