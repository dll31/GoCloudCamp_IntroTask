using Grpc.Core;
using MusicPlayerServer;
using MusicTrack;

namespace MusicPlayerServer.Services
{

    public class TrackSenderService : TransferTracklist.TransferTracklistBase
    {
        private List<TransferTrack> tracks = new();

        private readonly ILogger<TrackSenderService> _logger;
        public TrackSenderService(ILogger<TrackSenderService> logger, ref List<Track> tracklist)
        {
            _logger = logger;

            SetTracklist(ref tracklist);
        }

        public void SetTracklist(ref List<Track> tracklist)
        {
            tracks.Clear();

            foreach (var track in tracklist)
            {
                tracks.Add(new TransferTrack { Name = track.Name, Author = track.Author, Duration = track.Duration });
            }
        }

        
        public override Task<SendTracklistResponse> Transfer(SendTracklistRequest request, ServerCallContext context)
        {
            SendTracklistResponse response = new ();
            response.Tracklist.AddRange(tracks);
            
            return Task.FromResult(response);
        }
    }
}