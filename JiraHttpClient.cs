using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Http;

namespace Serilog.Sinks.Jira
{
    public class JiraHttpClient: IHttpClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        public JiraHttpClient(string username, string password, string jiraUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(jiraUrl);
            
            var credentials = $"{username}:{password}";
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
        }
        
        public async Task<HttpResponseMessage> createTicket(Stream contentStream, CancellationToken cancellationToken = default)
        {
            using (var content = new StreamContent(contentStream))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json") { CharSet = Encoding.UTF8.WebName };
                return await _httpClient.PostAsync() PostAsync(content, cancellationToken).ConfigureAwait(false);
            }
        }

        public void Configure(IConfiguration configuration) { }

        public void Dispose() => _httpClient?.Dispose();
    }
}