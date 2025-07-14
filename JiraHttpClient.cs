using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Sinks.Jira
{
    public class JiraHttpClient: IJiraHttpClient, IDisposable
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
        
        public async Task<HttpResponseMessage> CreateTicket(string projectId, string issueType, CancellationToken cancellationToken = default)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose() => _httpClient?.Dispose();
    }
}