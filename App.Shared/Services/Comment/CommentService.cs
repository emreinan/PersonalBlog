using App.Shared.Dto.Comment;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Json;

namespace App.Shared.Services.Comment;

public class CommentService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),ICommentService
{
    public async Task ApproveCommentAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"/api/Comment/Approve/{id}", null);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task CreateCommentAsync(CommentDto comment)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Comment", comment);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task DeleteCommentAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.DeleteAsync($"/api/Comment/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<CommentViewModel> GetCommentByIdAsync(int id)
    {
        var response = await _apiHttpClient.GetAsync($"/api/Comment/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<CommentViewModel>() ?? 
            throw new DeserializationException("CommentViewModel", response.Content.ReadAsStringAsync().Result);
        return result;
    }

    public async Task<List<CommentViewModel>> GetCommentsAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/Comment");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<List<CommentViewModel>>() ??
            throw new DeserializationException("List<CommentViewModel>", response.Content.ReadAsStringAsync().Result);
        return result;
    }

    public async Task<List<CommentViewModel>> GetCommentsForPostAsync(Guid postId)
    {
        var response = await _apiHttpClient.GetAsync($"/api/Comment/PostComment/{postId}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<List<CommentViewModel>>() ??
            throw new DeserializationException("List<CommentViewModel>", response.Content.ReadAsStringAsync().Result);
        return result;
    }

    public async Task UpdateCommentAsync(int id,CommentUpdateDto commentUpdateDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsJsonAsync($"/api/Comment/{id}", commentUpdateDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }
}
