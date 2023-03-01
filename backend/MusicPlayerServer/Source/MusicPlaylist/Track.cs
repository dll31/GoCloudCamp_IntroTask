

namespace MusicTrack;

public class Track
{
    public string Name { get; set; }
    public string Author { get; set; }
    public int Duration { get; set; }
    public string Path { get; set; }

    public Track(string name, string author, int duration, string path)
    {
        Name = name;
        Author = author;
        Duration = duration;
        Path = path;
    }
}

