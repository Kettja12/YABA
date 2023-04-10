using Master;
using ServicesShared;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddPresentation()
    .AddInterface(builder.Configuration);
var app = builder.Build();
app.AddMasterApi();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.Run();

