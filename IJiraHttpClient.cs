using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Sinks.Jira
{
    public interface IJiraHttpClient
    {
        Task<HttpResponseMessage>  CreateTicket(string projectKey, string issueType, CancellationToken cancellationToken);
        Task<HttpResponseMessage>  UploadAttachment(string issueId, Stream content, CancellationToken cancellationToken);
    }
}