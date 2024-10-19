using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.Comment;

public class CommentService(IHttpClientFactory httpClientFactory) : ICommentService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<List<CommentViewModel>> GetCommentsForPost(Guid postId)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Comment/GetCommentsForPost/{postId}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<CommentViewModel>>();
        return result;
    }
}
