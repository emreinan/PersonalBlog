using App.Client.Models;
using App.Client.Util.ExceptionHandling;
using App.Shared.Dto.Comment;

namespace App.Client.Services.Comment;

public class CommentService(IHttpClientFactory httpClientFactory) : ICommentService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<CommentViewModel> CreateComment(CommentViewModel comment)
    {
        var commentDto = new CommentDto {Content = comment.Content, PostId = comment.PostId, UserId = comment.UserId};
        var response = await _dataHttpClient.PostAsJsonAsync("/api/Comment", commentDto);
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<CommentViewModel>();
        return result;
    }

    public async Task<List<CommentViewModel>> GetCommentsForPost(Guid postId)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Comment/PostComment/{postId}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<CommentViewModel>>();
        return result;
    }
}
