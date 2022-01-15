namespace DrasticMedia.Core.Model.Metadata
{
    public interface IAlbumMetadata
    {
        int Id { get; set; }

        int AlbumItemId { get; set; }

        string Type { get; }

        string? Name { get; set; }

        string? Image { get; set; }

        DateTime? LastUpdated { get; set; }
    }
}
