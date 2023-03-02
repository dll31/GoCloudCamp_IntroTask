

namespace MusicTrack;

public class Track
{
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Duration { get; set; } = 0;
    public string Path { get; set; } = string.Empty;

    Track() { }

    public Track(string name, string author, int duration, string path)
    {
        Name = name;
        Author = author;
        Duration = duration;
        Path = path;
    }
}

