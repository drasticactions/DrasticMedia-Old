using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Model.Metadata
{
    public interface IAlbumMetadata
    {
        int Id { get; set; }

        string? Name { get; set; }

        string? Image { get; set; }

        DateTime? LastUpdated { get; set; }
    }
}
