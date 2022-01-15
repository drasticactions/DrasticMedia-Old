namespace DrasticMedia.Core.Model.Metadata
{
    public interface IArtistMetadata
    {
        int Id { get; set; }

        int ArtistItemId { get; set; }

        string Type { get; }

        string? Name { get; set; }

        string? Image { get; set; }

        DateTime? LastUpdated { get; set; }
    }
}
