using MusicPlayer;
using MusicPlayerServer.Source.MusicPlaylist;
using MusicPlaylist;
using MusicTrack;
using Newtonsoft.Json;
using System.IO;

class Program
{

    public static async Task Main(string[] args)
    {

        Track t1 = new("t1", "author", 500, "");
        Track t2 = new("t1", "author", 1500, "");

        TrackLoader tl = new ();
        tl.SetSource("library.json");

        List<Track> tracks = tl.UploadTracksFromSource();

        Console.WriteLine(tracks.Count);

        //List<Track> tracks = new ();
        //tracks.Add(t1);
        //tracks.Add(t2);

        //using (StreamWriter file = File.CreateText("library.json"))
        //{
        //    JsonSerializer serializer = new ();
        //    serializer.Serialize(file, tracks);
        //}




        //Playlist playlist = new("Test1");
        //playlist.AddTrack(ref t1);
        //playlist.AddTrack(ref t2);


        //Player pl = new();
        //pl.SetPlaylist(ref playlist);

        //pl.Start();

        //int Dur = 200;
        //Console.WriteLine("Delay main thred for " + Dur.ToString() + " ms");
        //await Task.Delay(Dur);

        //pl.Next();

        //Console.WriteLine("Delay main thred for " + (Dur * 3).ToString() + " ms");
        //await Task.Delay(Dur * 3);

        //pl.Prev();

        //Console.WriteLine("Delay main thred for " + (Dur * 3).ToString() + " ms");
        //await Task.Delay(Dur * 3);

        //pl.Next();

        //Console.WriteLine("Delay main thred for " + (Dur * 10).ToString() + " ms");
        //await Task.Delay(Dur * 10);

        return;
    }
}


//class Program
//{
//    static async Task Main()
//    {
//        CancellationTokenSource cts = new CancellationTokenSource();

//        Task task = Task.Run(async () =>
//        {
//            try
//            {
//                Console.WriteLine("Task started...");
//                await Task.Delay(TimeSpan.FromSeconds(10), cts.Token);
//                Console.WriteLine("Task completed.");
//            }
//            catch (TaskCanceledException)
//            {
//                Console.WriteLine("Task was canceled.");
//            }
//        });

//        await Task.Delay(TimeSpan.FromSeconds(5));
//        cts.Cancel();

//        using (var ctr = cts.Token.Register(() =>
//        {
//            Console.WriteLine("Cancellation callback called.");
//        }))
//        {
//            await task;
//        }

//        await Task.Delay(TimeSpan.FromSeconds(7));
//    }
//}
