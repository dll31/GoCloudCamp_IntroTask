using MusicPlayer;
using MusicPlaylist;
using MusicTrack;

class Program
{

    public static async Task Main(string[] args)
    {

        Track t1 = new ("t1", "author", 500, "");
        Track t2 = new("t1", "author", 1500, "");


        Playlist playlist = new ("Test1");
        playlist.AddTrack(ref t1);
        playlist.AddTrack(ref t2);


        Player pl = new();
        pl.SetPlaylist(ref playlist);

        pl.Start();

        int Dur = 3000;
        Console.WriteLine("Delay main thred for " + Dur.ToString() + " ms");
        await Task.Delay(Dur);

        pl.Next();

        Console.WriteLine("Make next");
        Console.WriteLine("Delay main thred for " + (Dur * 2).ToString() + " ms");
        await Task.Delay(Dur * 2);

        return;
    }
}




