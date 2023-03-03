using Google.Protobuf;
using Grpc.Core;
using MusicPlayer;
using MusicPlayerServer.Source.MusicPlaylist;
using MusicPlaylist;
using MusicTrack;
using System.Collections.Generic;

namespace MusicPlayerServer.Services
{
    public class PlayerService : PlayerApp.PlayerAppBase
    {
        private Player player = new();
        private List<TransferTrack> transferTracklist = new();

        private TrackLoader trackLoader = new();
        private List<Playlist> playlistCollection = new();
        private List<Track> trackLibrary = new();
        


        public PlayerService() 
        {
            trackLoader.SetSource("library.json");
            trackLibrary = trackLoader.UploadTracksFromSource();
            SetTracklist(ref trackLibrary);
        }


        public override Task<SendTracklistResponse> TransferTracklist (SendTracklistRequest request, ServerCallContext context)
        {
            SendTracklistResponse response = new();
            response.Tracklist.AddRange(transferTracklist);

            return Task.FromResult(response);
        }
        

        public override Task<CreatePlaylistResponse> CreatePlaylist (CreatePlaylistRequest request, ServerCallContext context)
        {
            CreatePlaylistResponse response = new();

            playlistCollection.Add(new Playlist(request.Name));

            response.Msg = "Create playlist with name " + request.Name;
            response.Playlist = new TransferPlaylistHeader { Name = request.Name, Id = playlistCollection[^1].Id };

            return Task.FromResult(response);
        }


        public override Task<AddTrackToPlaylistResponse> AddTrackToPlaylist (AddTrackToPlaylistRequest request, ServerCallContext context)
        {
            AddTrackToPlaylistResponse response = new();
            response.Resp.ErC = ErrorCode.Ok;
            TypicalPlaylistResponse resp = response.Resp;

            Playlist? pl = TypicalCheckPlaylistExist(request.Req, ref resp);
            response.Resp = resp;
            if (response.Resp.ErC != ErrorCode.Ok) return Task.FromResult(response);


            Track? tr = TypicalCheckTrackExist(request.Req, ref resp, request.Tr);
            response.Resp = resp;
            if (response.Resp.ErC != ErrorCode.Ok) return Task.FromResult(response);


            playlistCollection[playlistCollection.IndexOf(pl)].AddTrack(trackLibrary[trackLibrary.IndexOf(tr)]);

            response.Resp.ErC = ErrorCode.Ok;
            response.Resp.Msg = "Add track " + request.Tr.Name + " to playlist " + pl.Name;

            List<TransferTrack> temp = FillTransferPlaylistContent(pl.playlist.ToList());

            response.NewPlaylistContent = new();
            response.NewPlaylistContent.Tracks.AddRange(temp);

            return Task.FromResult(response);
        }


        public override Task<StartPlaylistResponse> StartPlaylist(StartPlaylistRequest request, ServerCallContext context)
        {
            StartPlaylistResponse response = new();
            response.Resp.ErC = ErrorCode.Ok;
            TypicalPlaylistResponse resp = response.Resp;

            Playlist? pl = TypicalCheckPlaylistExist(request.Req, ref resp);
            response.Resp = resp;
            if (response.Resp.ErC != ErrorCode.Ok) return Task.FromResult(response); 

            player.SetPlaylist(ref pl);
            player.Start();

            response.Resp.ErC = ErrorCode.Ok;
            response.Resp.Msg = "Start player with playlist name " + pl.Name;

            return Task.FromResult(response);
        }


        public override Task<OperationResponse> ResolveOperation(OperationRequest request, ServerCallContext context)
        {
            OperationResponse response = new();
            MusicPlayerErrors rc = MusicPlayerErrors.AllOk;


            switch (request.Op)
            {
                case Operation.Play:
                    rc = player.Start();
                    if (rc == MusicPlayerErrors.NoCurrentSong)
                    {
                        response.Msg = "Add songs before playing playlist";
                        response.PlayerEr = ErrorCode.NoCurrentSong;
                    }
                    else if (rc == MusicPlayerErrors.AllOk)
                    {
                        response.Msg = "Start playing";
                        response.PlayerEr = ErrorCode.Ok;
                    }

                    break;

                case Operation.Pause:
                    rc = player.Pause();

                    if (rc == MusicPlayerErrors.Pause)
                    {
                        response.Msg = "Playlist is paused";
                        response.PlayerEr = ErrorCode.Pause;
                    }
                    
                    break;

                case Operation.Next:
                    rc = player.Next();

                    if (rc == MusicPlayerErrors.NoCurrentSong)
                    {
                        response.Msg = "Somethimg wrong. Playlist has no current song";
                        response.PlayerEr = ErrorCode.NoCurrentSong;
                    }
                    else if (rc == MusicPlayerErrors.EndOfPlaylist)
                    {
                        response.Msg = "Playlist is ended";
                        response.PlayerEr = ErrorCode.EndOfPlaylist;
                    }
                    else if (rc == MusicPlayerErrors.StartNextSong)
                    {
                        response.Msg = "Next song is started";
                        response.PlayerEr = ErrorCode.StartNextSong;
                    }

                    break;

                case Operation.Prev:
                    rc = player.Prev();

                    if (rc == MusicPlayerErrors.NoCurrentSong)
                    {
                        response.Msg = "Somethimg wrong. Playlist has no current song";
                        response.PlayerEr = ErrorCode.NoCurrentSong;
                    }
                    else if (rc == MusicPlayerErrors.NoSongsInPlaylist)
                    {
                        response.Msg = "Add songs before playing playlist";
                        response.PlayerEr = ErrorCode.NoCurrentSong;
                    }
                    else if (rc == MusicPlayerErrors.StartPrevSong)
                    {
                        response.Msg = "Start prev song";
                        response.PlayerEr = ErrorCode.StartPrevSong;
                    }

                    break;
            }

            return Task.FromResult(response);
        }


        private Playlist TypicalCheckPlaylistExist(TypicalPlaylistRequest request, ref TypicalPlaylistResponse response)
        {
            Playlist? pl = playlistCollection.Find(i => i.Id == request.Id);

            if (pl == null)
            {
                response.ErC = ErrorCode.NoSuchPlaylist;
                response.Msg = "No such playlist with id= " + request.Id;
            }

            return pl;
        }


        private Track TypicalCheckTrackExist(TypicalPlaylistRequest request, ref TypicalPlaylistResponse response, TransferTrack reqTrack)
        {
            Track? tr = trackLibrary.Find(i => (i.Name == reqTrack.Name && i.Author == reqTrack.Author && i.Duration == reqTrack.Duration));

            if (tr == null)
            {
                response.ErC = ErrorCode.NoSuchTrack;
                response.Msg = "No such track in library";
            }

            return tr;
        }


        private List<TransferTrack> FillTransferPlaylistContent(List<Track> tracklist)
        {
            List<TransferTrack> temp = new();

            foreach (var track in tracklist)
            {
                temp.Add(new TransferTrack { Name = track.Name, Author = track.Author, Duration = track.Duration });
            }

            return temp;
        }


        private void SetTracklist(ref List<Track> tracklist)
        {
            transferTracklist.Clear();

            transferTracklist = FillTransferPlaylistContent(tracklist);
            
        }


    }
}
