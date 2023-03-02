using MusicTrack;
using System.Numerics;
using Newtonsoft.Json;
using System;


namespace MusicPlayerServer.Source.MusicPlaylist
{
    public class TrackLoader
    {
        private string Source = string.Empty;

        public void SetSource(string source) 
        {
            Source = source;    
        }

        
        public List<Track> UploadTracksFromSource()
        {
            List<Track>? tracklist = new();

            using (StreamReader file = File.OpenText(Source))
            {
                JsonSerializer serializer = new ();
                tracklist = serializer.Deserialize(file, typeof(List<Track>)) as List<Track>;
            }

            return tracklist ?? new List<Track>();
        }

    }
}
