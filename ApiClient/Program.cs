using System.Net;
using System.Reflection;
using ApiClient.Policies;
using ApiClient.Services;
using MediatR;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<TokenHandler>();


var logger = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();

builder.Services.AddHttpClient<ISayHelloHttpService, SayHelloHttpService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["EndpointSayHello"]);
}).AddPolicyHandlers("PolicyConfig", logger,builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
