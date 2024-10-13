using Microsoft.AspNetCore.Mvc;
using YouTubeApi.Services;
using YouTubeApi.Models;

namespace YouTubeApi.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;

        public YouTubeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        // Index
        public async Task<IActionResult> Index()
        {
            var trendingVideos = await _youtubeService.GetTrendingVideosAsync();
            return View(trendingVideos);
        }

        // Search
        public IActionResult Search()
        {
            return View();
        }

        // SearchResults
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> SearchResults(string query, string sortBy, string pageToken = null, int currentPage = 1, bool isNext = false)
        {
            var (videos, nextPageToken) = await _youtubeService.SearchVideosAsync(query, sortBy, pageToken);
            ViewData["CurrentQuery"] = query;
            ViewData["SortBy"] = sortBy;
            ViewData["NextPageToken"] = nextPageToken;
            ViewData["PrevPageToken"] = pageToken; 

            if (isNext)
            {
                ViewData["CurrentPage"] = currentPage + 1; 
            }
            else if (!string.IsNullOrEmpty(pageToken))
            {
                ViewData["CurrentPage"] = currentPage - 1; 
            }
            else
            {
                ViewData["CurrentPage"] = currentPage; 
            }

            return View("Results", videos);
        }
    }
}
