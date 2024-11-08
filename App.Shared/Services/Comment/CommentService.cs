﻿using App.Shared.Dto.Comment;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using Ardalis.Result;
using System.Net.Http.Json;

namespace App.Shared.Services.Comment;

public class CommentService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),ICommentService
{
    public async Task ApproveCommentAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsync($"/api/Comment/Approve/{id}", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task CreateCommentAsync(CommentDto comment)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PostAsJsonAsync("/api/Comment", comment);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteCommentAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.DeleteAsync($"/api/Comment/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<CommentViewModel> GetCommentByIdAsync(int id)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Comment/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<CommentViewModel>();
        return result;
    }

    public async Task<List<CommentViewModel>> GetCommentsAsync()
    {
        var response = await _dataHttpClient.GetAsync("/api/Comment");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<CommentViewModel>>();
        return result;
    }

    public async Task<List<CommentViewModel>> GetCommentsForPostAsync(Guid postId)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Comment/PostComment/{postId}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<CommentViewModel>>();
        return result;
    }

    public async Task UpdateCommentAsync(int id,CommentUpdateDto commentUpdateDto)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsJsonAsync($"/api/Comment/{id}", commentUpdateDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }
}
