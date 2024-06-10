using System.Diagnostics; 
using System.Data.Common;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// app.MapGet("/user", async (string name) => {
//     using var connection = new MySqlConnection("Server=SQL01;User ID=Admin;Password=SuperS3cureP@ss;Database=app");
//     using var command = connection.CreateCommand();
//     command.CommandText = $"SELECT name FROM users WHERE name='{name}';";
//     var users = await command.ExecuteReaderAsync();
//     return users;
// });

app.MapGet("/users", async (int limit) => {
    using var connection = new MySqlConnection("Server=SQL01;User ID=Admin;Password=SuperS3cureP@ss;Database=app");
    using var command = connection.CreateCommand();
    if (limit == 0) limit = 10;
    command.CommandText = $"SELECT name FROM users limit {limit};";
    var users = await command.ExecuteReaderAsync();
    return users;
});



app.MapGet("/weatherforecast", (string city) =>
{
    // Start the child process.
    Process p = new Process();
    // Redirect the output stream of the child process.
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.RedirectStandardOutput = true;
    p.StartInfo.FileName = "curl.exe";
    p.StartInfo.Arguments = "-H 'X-Api-Key: d984c043-d893-4a76-94fe-bac14948fe5d' https://api.api-ninjas.com/v1/weather?city="+city;
    p.Start();
    // Do not wait for the child process to exit before
    // reading to the end of its redirected stream.
    // p.WaitForExit();
    // Read the output stream first and then wait.
    string output = p.StandardOutput.ReadToEnd();
    p.WaitForExit();
    return output;
})
.WithName("GetWeatherForecast");

app.Run();
