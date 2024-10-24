using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.BlogPost;

public class BlogPostService(IHttpClientFactory httpClientFactory) : IBlogPostService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<BlogPostViewModel> GetBlogPost(Guid postId)
    {
        var response = await _dataHttpClient.GetAsync($"/api/BlogPost/{postId}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<BlogPostViewModel>();
        return result;
    }

    public async Task<List<BlogPostViewModel>> GetBlogPosts()
    {
        var response = await _dataHttpClient.GetAsync("/api/BlogPost");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<BlogPostViewModel>>();
        return result;
    }


}
