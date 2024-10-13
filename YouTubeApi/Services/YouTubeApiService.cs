using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YouTubeApi.Models;

namespace YouTubeApi.Services
{
    public class YouTubeApiService
    {
        private readonly YouTubeService _youtubeService;

        public YouTubeApiService(string apiKey)
        {
            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = GetType().ToString()
            });
        }

        public async Task<List<YouTubeVideoModel>> GetTrendingVideosAsync()
        {
            var videoRequest = _youtubeService.Videos.List("snippet");
            videoRequest.Chart = VideosResource.ListRequest.ChartEnum.MostPopular;
            videoRequest.RegionCode = "MY"; 
            videoRequest.MaxResults = 30; 
            var videoResponse = await videoRequest.ExecuteAsync();
            var videos = new List<YouTubeVideoModel>();

            foreach (var item in videoResponse.Items)
            {
                var video = new YouTubeVideoModel
                {
                    Id = item.Id,
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description, 
                    ChannelId = item.Snippet.ChannelId,
                    ChannelTitle = item.Snippet.ChannelTitle,
                    Duration = "Unknown", 
                    ChannelThumbnailUrl = await GetChannelThumbnailUrl(item.Snippet.ChannelId) 
                };

                videos.Add(video);
            }

            return videos;
        }


        public async Task<(List<YouTubeVideoModel>, string)> SearchVideosAsync(string query, string sortBy, string pageToken = null)
        {
            var searchRequest = _youtubeService.Search.List("snippet");
            searchRequest.Q = query;
            searchRequest.Type = "video";
            searchRequest.MaxResults = 10; 
            searchRequest.PageToken = pageToken;

            switch (sortBy)
            {
                case "viewCount":
                    searchRequest.Order = SearchResource.ListRequest.OrderEnum.ViewCount;
                    break;
                case "date":
                    searchRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                    break;
                case "rating":
                    searchRequest.Order = SearchResource.ListRequest.OrderEnum.Rating;
                    break;
                default:
                    searchRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                    break;
            }

            var searchResponse = await searchRequest.ExecuteAsync();
            var videos = new List<YouTubeVideoModel>();
            var videoIds = searchResponse.Items.Select(item => item.Id.VideoId).ToList();
            var videoRequest = _youtubeService.Videos.List("contentDetails");
            videoRequest.Id = string.Join(",", videoIds);
            var videoResponse = await videoRequest.ExecuteAsync();

            foreach (var item in searchResponse.Items)
            {
                var duration = videoResponse.Items.FirstOrDefault(v => v.Id == item.Id.VideoId)?.ContentDetails?.Duration;
                videos.Add(new YouTubeVideoModel
                {
                    Id = item.Id.VideoId,
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ChannelId = item.Snippet.ChannelId,
                    ChannelTitle = item.Snippet.ChannelTitle,
                    Duration = duration ?? "Unknown",
                    ChannelThumbnailUrl = await GetChannelThumbnailUrl(item.Snippet.ChannelId),
                    
                });
            }

            return (videos, searchResponse.NextPageToken);
        }

        private async Task<string> GetChannelThumbnailUrl(string channelId)
        {
            var channelRequest = _youtubeService.Channels.List("snippet");
            channelRequest.Id = channelId;
            var channelResponse = await channelRequest.ExecuteAsync();

            if (channelResponse.Items.Count > 0)
            {
                var channel = channelResponse.Items[0];
                return channel.Snippet.Thumbnails.High.Url; 
            }

            return string.Empty; 
        }


    }
}