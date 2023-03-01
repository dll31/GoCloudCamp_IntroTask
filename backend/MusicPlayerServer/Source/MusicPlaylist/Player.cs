using MusicEmulatorPlayer;
using MusicPlaylist;
using MusicTrack;


namespace MusicPlayer;

public enum CurrentAction
{
    Start,
    Pause,
    Next,
    Prew
}


public class Player
{

    public Playlist playlist = null;

    private LinkedListNode<Track> current = null;

    EmulatorPlayer EP { get; set; } = new();
    CurrentAction currentAction ;
    //ManualResetEventSlim pauseEvent { get; set; } = new(false);


    public Player() { }


    public void SetPlaylist(ref Playlist playlist)
    {
        this.playlist = playlist;
        current = playlist.GetBegin();
    }


    public void Start()
    {
        if (current == null) { return; }
        
        if (currentAction == CurrentAction.Pause)
        {
            //pauseEvent.Set();
            currentAction = CurrentAction.Start;
        }
        else
        {
            EP.RemakeCTokens();
            Console.WriteLine("Start");
            EP.Duration = playlist.GetDuration(ref current);
            currentAction = CurrentAction.Start;
            EP.RestartTaskAsync(EPcallback);
        }
    }


    private void EPcallback(EmulatorPlayerErrors error)
    {
        Console.WriteLine("Callback");
        if (currentAction == CurrentAction.Start)
            currentAction = CurrentAction.Next;

        if (currentAction == CurrentAction.Next)
        {
            Console.WriteLine("Make next");
            Next();
        }

    }


    public void Pause()
    {
        currentAction = CurrentAction.Pause;
        //pauseEvent.Reset();
    }


    public void Next()
    {
        if (current == null) { return; }

        current = playlist.Next(ref current);

        if (current == null) { return; }

        Console.WriteLine("Next");
        EP.CTokenSource.Cancel();

        EP.RemakeCTokens();
        EP.Duration = playlist.GetDuration(ref current);
        currentAction = CurrentAction.Next;
        EP.RestartTaskAsync(EPcallback);
    }
}
