namespace YouTubeApi.Models
{
    public class YouTubeVideoModel
    {
        public string Id { get; set; } // Video ID
        public string Title { get; set; } //Titile for Video
        public string Description { get; set; } // Description for Video
        public string ChannelId { get; set; } // Channel ID
        public string ChannelTitle { get; set; } // Title for Channel
        public string Duration { get; set; } // Video Duration
        public string ChannelThumbnailUrl { get; set; } // Channel Thumbnail URL
        public DateTime UploadDate { get; set; } // Upload date (Sorting)
        public int ViewCount { get; set; } // View count (Sorting) 
        public float Rating { get; set; } // Rating (Sorting)
        public string NextPageToken { get; set; } // Token for next page (for pagination)
        public string PreviousPageToken { get; set; } // Token for previous page (for pagination)
        public float RelevanceScore { get; set; }

        // Thumbnail URL properties
        public string ThumbnailUrl => $"https://img.youtube.com/vi/{Id}/hqdefault.jpg"; 
        public string HighQualityThumbnailUrl => $"https://img.youtube.com/vi/{Id}/maxresdefault.jpg"; 

        // Generate the Channel URL
        public string ChannelUrl => $"https://www.youtube.com/channel/{ChannelId}";

        // Method to format the ISO 8601 duration
        public string GetFormattedDuration()
        {
            if (string.IsNullOrEmpty(Duration))
                return "Unknown";

            Duration = Duration.Replace("PT", "");
            int hours = 0, minutes = 0, seconds = 0;

            if (Duration.Contains("H"))
            {
                var splitHours = Duration.Split("H");
                hours = int.Parse(splitHours[0]);
                Duration = splitHours.Length > 1 ? splitHours[1] : "0";
            }

            if (Duration.Contains("M"))
            {
                var splitMinutes = Duration.Split("M");
                minutes = int.Parse(splitMinutes[0]);
                Duration = splitMinutes.Length > 1 ? splitMinutes[1] : "0";
            }

            if (Duration.Contains("S"))
            {
                var splitSeconds = Duration.Split("S");
                seconds = int.Parse(splitSeconds[0]);
            }

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}
