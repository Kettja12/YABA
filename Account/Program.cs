using Account.Services;
using ServicesShared;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInterface(builder.Configuration);

var app = builder.Build();
app.AddAccountApi();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();
