using MusicTrack;


namespace MusicPlaylist;

public class Playlist
{
    private static long nextID = 1;

    public long Id { get; private set; }
    public string Name { get; set; }
    public LinkedList<Track> playlist { get; private set; }  = new();


    public Playlist(string name) 
    {
        Name = name;
        Id = nextID++;
    }


    public LinkedListNode<Track> Next(ref LinkedListNode<Track> current)
    {
        return current.Next;
    }


    public LinkedListNode<Track> Prev(ref LinkedListNode<Track> current)
    {
        return current.Previous;
    }
    

    public void Delete(ref LinkedListNode<Track> track)
    {
        playlist.Remove(track);
    }


    public void AddTrack(Track track)
    {
        playlist.AddLast(track);
    }


    public LinkedListNode<Track> GetBegin()
    {
        return playlist.First;
    }


    public int GetDuration(ref LinkedListNode<Track> current)
    {
        return current.Value.Duration;
    }

}