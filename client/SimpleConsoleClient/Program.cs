using Grpc.Core;
using Grpc.Net.Client;
using MusicPlayerSimpleConsoleClient;

class Program
{
    static private List<TransferPlaylist> playlistLibrary = new();  
    static private List<TransferTrack> trackLibrary = new();


    static private async void MakeOperation(PlayerApp.PlayerAppClient client, Operation op)
    {
        var response = await client.ResolveOperationAsync(new OperationRequest { Op = op });
        Console.WriteLine($"Response:\n \tPlayer error: {response.PlayerEr} \n\t Message: {response.Msg}");
    }


    static private async void CreatePlaylist(PlayerApp.PlayerAppClient client)
    {
        Console.WriteLine("Enter playlist name: ");
        string name = Console.ReadLine();

        var response = await client.CreatePlaylistAsync(new CreatePlaylistRequest { Name = name });
        Console.WriteLine($"Response:\n\t {response.Msg}");

        playlistLibrary.Add(new TransferPlaylist { Header = response.Playlist });

    }


    static private async Task LoadTrackLibrary(PlayerApp.PlayerAppClient client)
    {
        var response = await client.TransferTracklistAsync(new SendTracklistRequest { });

        trackLibrary = response.Tracklist.ToList();
    }


    static private async void TransferTracklist(PlayerApp.PlayerAppClient client)
    {
        await LoadTrackLibrary(client);

        PrintTrackLibrary();
    }


    static private void PrintTrackLibrary()
    {
        Console.WriteLine("Track library :");
        foreach (var track in trackLibrary)
        {
            Console.WriteLine($"\tTrack name : {track.Name}");
            Console.WriteLine($"\tTrack author : {track.Author}\n");
        }
    }


    static private void PrintPlaylistLibrary()
    {
        Console.WriteLine("Playlist library:");
        foreach (var playlist in playlistLibrary)
        {
            Console.WriteLine($"Playlist name : {playlist.Header.Name}");
            if (playlist.Content == null)
                continue;
            
            foreach (var track in playlist.Content.Tracks.ToList())
            {
                Console.WriteLine($"Track : {track.Name} -- {track.Author}");
            }
            Console.WriteLine();
        }

    }


    static private async void AddTrack(PlayerApp.PlayerAppClient client)
    {
        PrintTrackLibrary();

        TransferTrack neededTrack = new();
        bool existFlag = false;

        // excess check. server do this
        do
        {
            Console.WriteLine("Enter added track name: ");
            string trackName = Console.ReadLine();

            TransferTrack? tr = trackLibrary.Find(i => i.Name == trackName);
            if (trackName == "0")
                return;

            if (tr != null)
            {
                existFlag = true;
                neededTrack = tr;
            }
            else Console.WriteLine($"No track with name : {trackName} in library. Try again");

        } while (!existFlag);


        PrintPlaylistLibrary();

        existFlag = false;
        TransferPlaylist neededPlaylist = new();

        // excess check. server do this
        do
        {
            Console.WriteLine("Enter playlist name for added track: ");
            string playlistName = Console.ReadLine();

            TransferPlaylist? pl = playlistLibrary.Find(i => i.Header.Name == playlistName);
            if (playlistName == "0")
                return;

            if (pl != null)
            {
                existFlag = true;
                neededPlaylist = pl;
            }
            else Console.WriteLine($"No playlist with name : {playlistName} in library. Try again");


        } while (!existFlag);

        var response = await client.AddTrackToPlaylistAsync(new AddTrackToPlaylistRequest { 
            Req = new TypicalPlaylistRequest { Id = neededPlaylist.Header.Id}, Tr = new TransferTrack(neededTrack)});

        if (response.Resp.ErC != ErrorCode.Ok)
        {
            Console.WriteLine($"Response : {response.Resp.Msg}");
        }
        else
        {
            Console.WriteLine($"Response : {response.Resp.Msg}");
            playlistLibrary[playlistLibrary.IndexOf(neededPlaylist)].Content = response.NewPlaylistContent;
        }


    }


    static private async void StartPlaylist(PlayerApp.PlayerAppClient client)
    {
        PrintPlaylistLibrary();

        bool existFlag = false;
        TransferPlaylist neededPlaylist = new();

        // excess check. server do this
        do
        {
            Console.WriteLine("Enter playlist name for added track: ");
            string playlistName = Console.ReadLine();

            TransferPlaylist? pl = playlistLibrary.Find(i => i.Header.Name == playlistName);

            if (pl != null)
            {
                existFlag = true;
                neededPlaylist = pl;
            }
            else Console.WriteLine($"No playlist with name : {playlistName} in library. Try again");


        } while (!existFlag);

        var response = await client.StartPlaylistAsync(new StartPlaylistRequest { Req = new TypicalPlaylistRequest { Id = neededPlaylist.Header.Id } });

        Console.WriteLine($"Response: {response}");
    }


    static async Task Main()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:50051");


        PlayerApp.PlayerAppClient client = new (channel);

        await LoadTrackLibrary(client);


        static void mainMenuShow()
        {
            Console.WriteLine("Main menu:" +
                "\n1. Play" +
                "\n2. Pause" +
                "\n3. Next" +
                "\n4. Prev" +
                "\n5. Create playlist" +
                "\n6. Add track to playlist" +
                "\n7. Start playlist" +
                "\n8. Get track library" +
                "\n0. Exit" +
                "\nShoose action: ");
        }


        while (true)
        {
            mainMenuShow();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D0:
                    return;

                case ConsoleKey.D1:
                    MakeOperation(client, Operation.Play);
                    break;

                case ConsoleKey.D2:
                    MakeOperation(client, Operation.Pause);
                    break;

                case ConsoleKey.D3:
                    MakeOperation(client, Operation.Next);
                    break;

                case ConsoleKey.D4:
                    MakeOperation(client, Operation.Prev);
                    break;

                case ConsoleKey.D5:
                    CreatePlaylist(client);
                    break;

                case ConsoleKey.D6:
                    AddTrack(client);
                    break;

                case ConsoleKey.D7:
                    StartPlaylist(client);
                    break;

                case ConsoleKey.D8:
                    TransferTracklist(client);
                    break;
                default : break;

            }
        }
    }
}

