using DrasticMedia.Core.Model.Metadata;
using E.Deezer.Api;

namespace DrasticMedia.Core.Metadata
{
    public static class MetadataExtensions
    {
        public static AlbumDeezerMetadata FromAlbum(this IAlbum album, int albumId)
        {
            var deezerAlbum = new AlbumDeezerMetadata(albumId);

            return deezerAlbum;
        }

        public static ArtistDeezerMetadata FromArtist(this IArtist artist, int artistId)
        {
            var deezerArtist = new ArtistDeezerMetadata(artistId);

            return deezerArtist;
        }
    }
}
