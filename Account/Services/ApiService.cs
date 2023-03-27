using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client.Services;

public class ApiService
{
    
    private HttpClient client;
    public ApiService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<T?> PostAsync<T>(string authToken,string service, object data)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        var request = new HttpRequestMessage(HttpMethod.Post, service);

        if (string.IsNullOrEmpty(authToken) == false)
        {
            request.Headers.Add("X-Api-Key", authToken);
        }
        string jsonData = JsonSerializer.Serialize(data);
        request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.SendAsync(request, cts.Token);
        if (response.StatusCode != HttpStatusCode.OK)
        {

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Unauthorized request.");
            }
            throw new Exception("Service failed.");
        }
        var message = await response.Content.ReadFromJsonAsync<T>();
        return message;
    }
    public async Task<T?> GetAsync<T>(string authToken,string service)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        var request = new HttpRequestMessage(HttpMethod.Get, service);

        if (string.IsNullOrEmpty(authToken) == false)
        {
            request.Headers.Add("X-Api-Key", authToken);
        }
        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Unauthorized request.");
            }
            throw new Exception("Service failed.");
        }
        var message = await response.Content.ReadFromJsonAsync<T>();
        return message;
    }

}
