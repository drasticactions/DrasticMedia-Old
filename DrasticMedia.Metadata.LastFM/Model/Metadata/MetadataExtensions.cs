using DrasticMedia.Core.Model.Metadata;

namespace DrasticMedia.Metadata
{
    public static class MetadataExtensions
    {
        public static AlbumLastFmMetadata FromAlbum(this Hqub.Lastfm.Entities.Album album, int albumId)
        {
            var lastfmAlbum = new AlbumLastFmMetadata(albumId);
            lastfmAlbum.MBID = album.MBID;
            lastfmAlbum.Name = album.Name;
            lastfmAlbum.Image = album.Images.LastOrDefault()?.Url;
            lastfmAlbum.LastUpdated = DateTime.UtcNow;
            return lastfmAlbum;
        }

        public static ArtistLastFmMetadata FromArtist(this Hqub.Lastfm.Entities.Artist artist, int artistId)
        {
            var lastfmArtist = new ArtistLastFmMetadata(artistId);
            lastfmArtist.MBID = artist.MBID;
            lastfmArtist.Name = artist.Name;
            lastfmArtist.Biography = artist.Biography.Content;
            lastfmArtist.BiographyPublished = artist.Biography.Published;
            return lastfmArtist;
        }
    }
}
