using DrasticMedia.Core.Model.Metadata;
using SpotifyAPI.Web;

namespace DrasticMedia.Metadata
{
    public static class MetadataExtensions
    {
        public static AlbumSpotifyMetadata FromSimpleAlbum(this SimpleAlbum spotifyAlbum, int albumId)
        {
            var album = new AlbumSpotifyMetadata(albumId);
            album.AlbumType = spotifyAlbum.AlbumType;
            album.TotalTracks = spotifyAlbum.TotalTracks;
            album.Name = spotifyAlbum.Name;
            album.Uri = spotifyAlbum.Uri;
            album.Artists = string.Join(", ", spotifyAlbum.Artists.Select(n => n.Name));
            album.Image = spotifyAlbum.Images.FirstOrDefault()?.Url;
            album.ReleaseDate = spotifyAlbum.ReleaseDate;
            album.ReleaseDatePrecision = spotifyAlbum.ReleaseDatePrecision;
            album.SpotifyId = spotifyAlbum.Id;
            return album;
        }

        public static ArtistSpotifyMetadata FromFullArtist(this FullArtist spotifyArtist, int artistId)
        {
            var artist = new ArtistSpotifyMetadata(artistId);
            artist.SpotifyId = spotifyArtist.Id;
            artist.Genres = string.Join(",", spotifyArtist.Genres);
            artist.Name = spotifyArtist.Name;
            artist.Popularity = spotifyArtist.Popularity;
            artist.Image = spotifyArtist.Images.FirstOrDefault()?.Url;
            artist.LastUpdated = DateTime.UtcNow;
            artist.Uri = spotifyArtist.Uri;
            return artist;
        }
    }
}
