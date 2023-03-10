using Microsoft.AspNetCore.Hosting.Server;
using MusicPlayerServer.Services;


class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpc();

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        app.MapGrpcService<PlayerService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run("http://localhost:50051");
    }
}
