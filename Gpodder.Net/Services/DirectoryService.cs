using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
#if (NET45)
using System.Web;
#endif
using GpodderLib.Dto;
using GpodderLib.Services.Base;

namespace GpodderLib.Services
{
    public class DirectoryService : ConfigurableRemoteServiceBase
    {
        private const string ApiPodcastToplistUri = "/toplist/{number}.json&scale_logo={size}";
        private const string ApiPodcastSearchUri = "/search.json?q={query}&scale_logo={size}";
        private const string ApiTopTagsUri = "/api/2/tags/{count}.json";
        private const string ApiPodcastsForTagUri = "/api/2/tag/{tag}/{count}.json&scale_logo={size}";
        private const string ApiPodcastDataUri = "/api/2/data/podcast.json?url={url}&scale_logo={size}";
        private const string ApiEpisodeDataUri = "/api/2/data/episode.json?podcast={podcast-url}&url={episode-url}";


        public DirectoryService(Configuration configuration, ConfigurationService configurationService)
            : base(configuration, configurationService)
        {
        }

        public async Task<Episode> QueryEpisodeData(Uri url, Uri podcastUrl)
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiEpisodeDataUri
                .Replace("{episode-url}", url.ToString())
                .Replace("{podcast-url}", podcastUrl.ToString())
                );
            return await Query<Episode>(uri);
        }

        public async Task<Podcast> QueryPodcastData(Uri url, uint scaleLogo = 64)
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiPodcastDataUri
                .Replace("{url}", url.ToString())
                .Replace("{size}", scaleLogo.ToString(CultureInfo.InvariantCulture))
                );
            return await Query<Podcast>(uri);
        }

        public async Task<List<Podcast>> QueryPodcastsForTag(Tag tag, uint count, uint scaleLogo = 64)
        {
            return await QueryPodcastsForTag(tag.Name, count, scaleLogo);
        }

        public async Task<List<Podcast>> QueryPodcastsForTag(string tag, uint count, uint scaleLogo = 64)
        {
            scaleLogo = scaleLogo > 256 ? 256 : scaleLogo;

            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiPodcastsForTagUri
                .Replace("{tag}", Uri.EscapeDataString(tag))
                .Replace("{count}", count.ToString(CultureInfo.InvariantCulture))
                .Replace("{size}", scaleLogo.ToString(CultureInfo.InvariantCulture))
                );
            return await Query<List<Podcast>>(uri);
        }

        public async Task<List<Tag>> QueryTopTags(uint count)
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiTopTagsUri
                .Replace("{count}", count.ToString(CultureInfo.InvariantCulture))
                );
            return await Query<List<Tag>>(uri);
        }

        public async Task<List<Podcast>> QueryTopPodcasts(uint count, uint scaleLogo = 64)
        {
            scaleLogo = scaleLogo > 256 ? 256 : scaleLogo;

            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiPodcastToplistUri
                .Replace("{number}", count.ToString(CultureInfo.InvariantCulture))
                .Replace("{size}", scaleLogo.ToString(CultureInfo.InvariantCulture))
                );
            return await Query<List<Podcast>>(uri);
        }

        public async Task<List<Podcast>> QuerySearchPodcasts(string query, uint scaleLogo = 64)
        {
            scaleLogo = scaleLogo > 256 ? 256 : scaleLogo;

            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiPodcastSearchUri
                .Replace("{query}", Uri.EscapeDataString(query))
                .Replace("{size}", scaleLogo.ToString(CultureInfo.InvariantCulture))
                );
            return await Query<List<Podcast>>(uri);
        }


    }
}
