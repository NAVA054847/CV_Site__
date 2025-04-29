using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Microsoft.Extensions.Configuration;
using System.Net;



using Microsoft.Extensions.Caching.Memory;
using Octokit;
using System;
using System.Threading.Tasks;



//namespace Service1
//{
//    public class GitHubService
//    {
//        private readonly GitHubClient _gitHubClient;
//        private readonly IConfiguration _configuration;

//        //public GitHubService()
//        //{
//        //    var builder = new ConfigurationBuilder()
//        //        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
//        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//        //    _configuration = builder.Build();

//        //    var token = _configuration["GitHubToken"];
//        //    _gitHubClient = new GitHubClient(new ProductHeaderValue("YourAppName")); // החלף את YourAppName
//        //    if (!string.IsNullOrEmpty(token))
//        //    {
//        //        _gitHubClient.Credentials = new Credentials(token);
//        //    }
//        //}


//        public GitHubService(IConfiguration configuration)
//        {
//            _configuration = configuration;

//            var githubToken = _configuration["GitHubToken"]; // קריאת הטוקן מההגדרות

//            var productInformation = new ProductHeaderValue("GitHubPortfolio");
//            var credentials = new Credentials(githubToken);
//            _gitHubClient = new GitHubClient(productInformation) { Credentials = credentials };
//        }





namespace Service1
{
    public class GitHubService
    {
        private readonly GitHubClient _gitHubClient;
        private readonly IConfiguration _configuration;

        private readonly IMemoryCache _memoryCache;
        private const string RepositoriesCacheKey = "GitHubRepositories_"; // מפתח Cache עם פרפיקס

        public GitHubService(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;

            var githubToken = _configuration["GitHubToken"];
            var productInformation = new ProductHeaderValue("GitHubPortfolio");
            var credentials = new Credentials(githubToken);
            _gitHubClient = new GitHubClient(productInformation) { Credentials = credentials };
        }





        //public async Task<IReadOnlyList<Repository>> GetRepositoriesAsync(string username)
        //        {
        //            try
        //            {
        //                return await _gitHubClient.Repository.GetAllForUser(username);
        //            }
        //            catch (ApiException ex)
        //            {
        //                // כאן תוכל לטפל בשגיאות API של GitHub (למשל, משתמש לא קיים)
        //                Console.WriteLine($"GitHub API Error: {ex.StatusCode} - {ex.Message}");
        //                return new List<Repository>(); // החזרת רשימה ריקה במקרה של שגיאה
        //            }
        //        }




        public async Task<IReadOnlyList<Repository>> GetRepositoriesAsync(string username)
        {
            string cacheKey = RepositoriesCacheKey + username.ToLowerInvariant(); // מפתח Cache ייחודי לכל משתמש

            if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyList<Repository> repositories))
            {
                // נתונים נמצאו ב-Cache, החזר אותם
                return repositories;
            }

            // נתונים לא נמצאו ב-Cache, קרא מ-GitHub API
            try
            {
                repositories = await _gitHubClient.Repository.GetAllForUser(username);

                if (repositories != null)
                {
                    // הגדר אפשרויות Cache
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)); // שמור ב-Cache למשך 5 דקות

                    // שמור את התוצאה ב-Cache
                    _memoryCache.Set(cacheKey, repositories, cacheEntryOptions);
                }

                return repositories;
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"GitHub API Error (GetRepositoriesAsync for {username}): {ex}");
                return null; // או טיפול שגיאה אחר
            }
        }


        public async Task<Repository> GetRepositoryDetailsAsync(string owner, string name)
        {
            try
            {
                return await _gitHubClient.Repository.Get(owner, name);
                
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"GitHub API Error (Get Repository Details): {ex.StatusCode} - {ex.Message}");

                // טיפול בשגיאות ספציפיות (דוגמאות):
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Repository לא נמצא
                    Console.WriteLine($"Repository '{name}' not found for user '{owner}'.");
                    return null; // או לזרוק חריגה ספציפית אם אתה רוצה שה-API יחזיר 404
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized access to GitHub API. Check your token permissions.");
                    // בעיות הרשאה - ייתכן שהטוקן לא מאפשר גישה למידע הזה
                    // כדאי לשקול לוגיקה לטיפול בטוקן לא תקין
                    return null; // או לזרוק חריגה
                }
            
                else
                {
                    // שגיאה כללית אחרת מה-API
                    Console.WriteLine($"An unexpected error occurred while fetching repository details: {ex.StatusCode} - {ex.Message}");
                    return null; // החזרת null במקרה של שגיאה לא צפויה
                }
              
            }
        }

        public async Task<SearchRepositoryResult> SearchRepositoriesAsync(string name, string language, string user)
        {
                       
            string  query = "";
          
            if (!string.IsNullOrEmpty(name))
            {
                query += name + " ";
            }
            if (!string.IsNullOrEmpty(language))
            {
                query += "language:" + language + " ";
            }
            if (!string.IsNullOrEmpty(user))
            {
                query += "user:" + user + " ";
            }

            // הסר רווחים מיותרים בסוף המחרוזת Q
            query = query.Trim();
            var searchRequest = new SearchRepositoriesRequest(query);

            // אם אין קריטריוני חיפוש, נחזיר תוצאה ריקה כדי למנוע שגיאה מה-API
            if (string.IsNullOrEmpty(query))
            {
                return new SearchRepositoryResult(0, false, new List<Repository>());
            }

            try
            {
                return await _gitHubClient.Search.SearchRepo(searchRequest);
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"GitHub API Search Error: {ex.StatusCode} - {ex.Message}");

                // טיפול בשגיאות ספציפיות (דוגמאות):
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // בקשה לא תקינה - ייתכן שהשאילתה לא חוקית
                    // במקרה הזה, כדאי לשקול רישום מפורט או החזרה של הודעה ידידותית יותר
                    Console.WriteLine("Search query was invalid.");
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized access to GitHub API. Check your token.");
                    // במקרה של שגיאת הרשאה, ייתכן שיש בעיה בטוקן
                    // כדאי לשקול לוגיקה לטיפול בטוקן לא תקין
                }
            
                // שגיאות אחרות מה-API
                

                return new SearchRepositoryResult(0, false, new List<Repository>()); // החזרת תוצאה ריקה במקרה של שגיאה
            }
        }







    }








}