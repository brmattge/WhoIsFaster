using WhoIsFaster.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowLocalhost", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhost");

app.MapHub<GameHub>("/gamehub");

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
