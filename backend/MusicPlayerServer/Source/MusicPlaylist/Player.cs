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


    public void Start()
    {
        if (current == null) { return; }
        
        if (currentAction == CurrentAction.Pause)
        {
            Console.WriteLine("Resume");
            currentAction = CurrentAction.Start;
            EP.RestartTaskAsync(EPcallback, true);
        }
        else
        {
            Console.WriteLine("Start");
            EP.Duration = playlist.GetDuration(ref current);
            currentAction = CurrentAction.Start;
            EP.RestartTaskAsync(EPcallback);
        }
    }


    private void EPcallback(EmulatorPlayerErrors error)
    {
        Console.WriteLine("Callback. CurrentAction is " + currentAction);
        if (currentAction == CurrentAction.Start)
            currentAction = CurrentAction.Next;

        if (currentAction == CurrentAction.Pause)
            return;

        if (currentAction == CurrentAction.Next)
        {
            Console.WriteLine("Make next");
            Next();
        }

    }


    public void Pause()
    {
        Console.WriteLine("Pause");
        currentAction = CurrentAction.Pause;
        EP.CTokenSource.Cancel();
    }


    public void Next()
    {
        if (current == null) { return; }

        current = playlist.Next(ref current);

        if (current == null) { return; }

        Console.WriteLine("Next");
        

        EP.Duration = playlist.GetDuration(ref current);
        currentAction = CurrentAction.Next;
        EP.RestartTaskAsync(EPcallback);
    }
}
