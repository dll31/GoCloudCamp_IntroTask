using MusicTrack;


namespace MusicPlaylist;

public class Playlist
{
    public long Id { get; }
    public string Name { get; set; }
    private LinkedList<Track> playlist = new();


    public Playlist(string name) 
    {
        Name = name;  
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


    public void AddTrack(ref Track track)
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