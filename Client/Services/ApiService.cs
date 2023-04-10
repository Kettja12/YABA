using Client.Responses;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Services;

public class ApiService
{
    private HttpClient client;
    private StateContainer stateContainer;
    private BaseResponse? errorResponse;
    public ApiService(HttpClient client, StateContainer stateContainer)
    {
        this.client = client;
        this.stateContainer = stateContainer;
    }

    public async Task<T?> PostAsync<T>(string service, dynamic data)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(60));
        if (service.StartsWith("(db)"))
        {
            service = service.Replace("(db)", stateContainer.LoginUser.CurrentDatabase);
        }
        var request = new HttpRequestMessage(HttpMethod.Post, service);

        if (stateContainer.IsAuthenticated)
        {
            Type type = data.GetType();
            if (type.Name == "ExpandoObject")
            {
                data.AuthToken = stateContainer.LoginUser.AuthToken;
            }
            else
            {
                PropertyInfo? property = type.GetProperty("AuthToken");
                property?.SetValue(data, stateContainer.LoginUser.AuthToken);
            }
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
        return  await response.Content.ReadFromJsonAsync<T>();
    }
    //public async Task<T?> GetAsync<T>(string service)
    //{
    //    var cts = new CancellationTokenSource();
    //    cts.CancelAfter(TimeSpan.FromSeconds(30));
    //    var request = new HttpRequestMessage(HttpMethod.Get, service);

    //    if (stateContainer.LoginUser != null && string.IsNullOrEmpty(stateContainer.LoginUser.AuthToken) == false)
    //    {
    //        //Type type = data.GetType();
    //        //PropertyInfo? property = type.GetProperty("AuthToken");
    //        //if (property != null)
    //        //{
    //        //    property.SetValue(data, stateContainer.LoginUser.AuthToken);
    //        //}
    //    }
    //    var response = await client.SendAsync(request);

    //    if (!response.IsSuccessStatusCode)
    //    {

    //        if (response.StatusCode == HttpStatusCode.Unauthorized)
    //        {
    //            throw new Exception("Unauthorized request.");
    //        }
    //        throw new Exception("Service failed.");
    //    }
    //    var message = await response.Content.ReadFromJsonAsync<T>();
    //    return message;
    //}

}
