using Proxy;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<InjectionMiddleware>();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseMiddleware<InjectionMiddleware>();
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapReverseProxy();
app.MapFallbackToFile("index.html");
app.Run();
