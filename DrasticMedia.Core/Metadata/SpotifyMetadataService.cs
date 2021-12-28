using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using SpotifyAPI.Web;

namespace DrasticMedia.Core.Metadata
{
    public class SpotifyMetadataService : IMetadataService
    {
        private SpotifyClient client;
        private HttpClient httpClient;

        public SpotifyMetadataService(string baseLocation, string apiKey = "", string apiSecret = "")
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                return;
            }

            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest(apiKey, apiSecret);
            var response = new OAuthClient(config).RequestToken(request).Result;

            this.client = new SpotifyClient(config.WithToken(response.AccessToken));

            if (string.IsNullOrEmpty(baseLocation))
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            var directory = Directory.CreateDirectory(baseLocation);
            if (!directory.Exists)
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            this.httpClient = new HttpClient();
            this.BaseMetadataLocation = baseLocation;
            this.IsEnabled = true;
        }

        public string BaseMetadataLocation { get; }

        public bool IsEnabled { get; }

        public async Task UpdatetArtistItemInfo(ArtistItem artist)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            if (this.httpClient is null)
            {
                return;
            }

            if (artist.Name is null)
            {
                return;
            }

            var result = await this.client.Search.Item(new SearchRequest(SearchRequest.Types.Artist, artist.Name));
            if (result is not null && result.Artists.Items.Any())
            {
                var artistInfo = result.Artists.Items.First();
                artist.Name = artistInfo.Name;
                artist.SpotifyId = artistInfo.Id;
                if (artistInfo.Images.Any())
                {
                    await artist.SaveArtistImage(this.BaseMetadataLocation, new Uri(artistInfo.Images[0].Url), this.httpClient);
                }
            }
        }
    }
}
