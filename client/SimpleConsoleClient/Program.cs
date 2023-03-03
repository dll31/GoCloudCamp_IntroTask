using Grpc.Net.Client;
using MusicPlayerSimpleConsoleClient;


using var channel = GrpcChannel.ForAddress("http://localhost:5172");


var client = new PlayerApp.PlayerAppClient(channel);


var response = await client.TransferTracklistAsync(request: new SendTracklistRequest());
Console.WriteLine($"Ответ сервера: {response.Tracklist}");