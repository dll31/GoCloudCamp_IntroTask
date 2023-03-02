using MusicEmulatorPlayer;
using MusicPlaylist;
using MusicTrack;


namespace MusicPlayer;

public enum CurrentAction
{
    Start,
    Pause,
    Next,
    Prev
}


public class Player
{
    private Playlist playlist;
    private LinkedListNode<Track> current;
    
    EmulatorPlayer EP { get; set; } = new();
    CurrentAction currentAction ;


    public Player() { }


    public void SetPlaylist(ref Playlist playlist)
    {
        this.playlist = playlist;
        current = playlist.GetBegin();
    }


    private void EPcallback(EmulatorPlayerErrors error)
    {
        Console.WriteLine("Callback. CurrentAction is " + currentAction);
        if (currentAction == CurrentAction.Start)
            currentAction = CurrentAction.Next;

        if (currentAction == CurrentAction.Pause)
            return;

        if (error == EmulatorPlayerErrors.Timeout && (currentAction == CurrentAction.Next || currentAction == CurrentAction.Prev))
        {
            Console.WriteLine("Make next");
            // How to rework StartNext in this case to see errors?
            StartNext();
        }

    }


    private MusicPlayerErrors TryToGetBegin()
    {
        if (current == null)
        {
            if (playlist.GetBegin() == null)
                return MusicPlayerErrors.NoSongsInPlaylist;
            else
                current = playlist.GetBegin();
        }
        return MusicPlayerErrors.AllOk;
    }


    public MusicPlayerErrors Start()
    {
        if (current == null) 
        {
            if (TryToGetBegin() == MusicPlayerErrors.NoSongsInPlaylist) 
                return MusicPlayerErrors.NoSongsInPlaylist;
        }

        if (currentAction == CurrentAction.Pause)
        {
            Console.WriteLine("Resume");
            currentAction = CurrentAction.Start;
            EP.RestartTaskAsync(EPcallback, true);
        }
        else
        {
            Console.WriteLine("Start");

            currentAction = CurrentAction.Start;
            PlayCurrentSong();
        }

        return MusicPlayerErrors.AllOk;
    }


    public MusicPlayerErrors Pause()
    {
        Console.WriteLine("Pause");
        currentAction = CurrentAction.Pause;
        EP.CTokenSource.Cancel();

        return MusicPlayerErrors.Pause;
    }


    public MusicPlayerErrors Next()
    {
        Console.WriteLine("Next");

        currentAction = CurrentAction.Next;
        EP.CTokenSource.Cancel();

        return StartNext();
    }


    public MusicPlayerErrors StartNext()
    {
        if (current == null) { return MusicPlayerErrors.NoCurrentSong; }

        current = playlist.Next(ref current);

        if (current == null) { return MusicPlayerErrors.EndOfPlaylist; }

        PlayCurrentSong();

        return MusicPlayerErrors.StartNextSong;
    }


    public MusicPlayerErrors Prev()
    {
        if (current == null) { return MusicPlayerErrors.NoCurrentSong; }

        current = playlist.Prev(ref current);

        if (TryToGetBegin() == MusicPlayerErrors.NoSongsInPlaylist) return MusicPlayerErrors.NoSongsInPlaylist;

        Console.WriteLine("Prev");

        currentAction = CurrentAction.Prev;
        PlayCurrentSong();

        return MusicPlayerErrors.StartPrevSong;
    }

    private void PlayCurrentSong()
    {
        EP.Duration = playlist.GetDuration(ref current);
        EP.RestartTaskAsync(EPcallback);
    }

}

public enum MusicPlayerErrors
{
    AllOk,
    
    NoCurrentSong,
    NoPrevSong,
    EndOfPlaylist,
    NoSongsInPlaylist,

    StartNextSong,
    StartPrevSong,

    Pause
}
