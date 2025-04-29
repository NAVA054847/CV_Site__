using Microsoft.AspNetCore.Mvc;


using Service1;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly GitHubService _gitHubService;

        public GitHubController(GitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }




        [HttpGet("{username}/repositories")]
        public async Task<IActionResult> GetUserRepositories(string username)
        {
            try
            {
                var repositories = await _gitHubService.GetRepositoriesAsync(username);
                if (repositories == null || !repositories.Any())
                {
                    return NotFound($"No repositories found for user '{username}'.");
                }
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                // כאן כדאי לטפל בשגיאות בצורה יותר מפורטת ולרשום ללוגים
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve repositories.");
            }
        }


        [HttpGet("{owner}/{repoName}")]
        public async Task<IActionResult> GetRepositoryDetails(string owner, string repoName)
        {
            try
            {
                var repository = await _gitHubService.GetRepositoryDetailsAsync(owner, repoName);
                if (repository == null)
                {
                    return NotFound($"Repository '{repoName}' not found for user '{owner}'.");
                }
                return Ok(repository);
            }
            catch (Exception ex)
            {
                // שוב, טיפול שגיאות מפורט יותר ורישום לוגים מומלצים
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve repository details.");
            }
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchRepositories([FromQuery] string? name, [FromQuery] string? language, [FromQuery] string? user)
        {
            try
            {
                var searchResult = await _gitHubService.SearchRepositoriesAsync(name, language, user);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                // טיפול מפורט בשגיאות ורישום לוגים מומלצים
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to search repositories.");
            }
        }

    }










}