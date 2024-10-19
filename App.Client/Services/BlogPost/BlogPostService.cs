using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.BlogPost;

public class BlogPostService(IHttpClientFactory httpClientFactory) : IBlogPostService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<List<BlogPostViewModel>> GetBlogPosts()
    {
        var response = await _dataHttpClient.GetAsync("/api/BlogPost/GetBlogPosts");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<BlogPostViewModel>>();
        return result;
    }
}
