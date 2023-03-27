using Account.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddPresentation()
    .AddInterface(builder.Configuration);
var app = builder.Build();
app.UseCors();
app.AddAccountApi();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<InjectionMiddleware>();

app.Run();
