using Server.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppDefinitions();


var app = builder.Build();

app.UseAppDefinitions();
app.Run();
