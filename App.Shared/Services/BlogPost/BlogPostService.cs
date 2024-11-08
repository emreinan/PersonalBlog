﻿using App.Shared.Dto.BlogPost;
using App.Shared.Dto.File;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace App.Shared.Services.BlogPost;

public class BlogPostService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IBlogPostService
{
    public async Task CreateBlogPostAsync(BlogPostDto blogPostDto)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(blogPostDto.Title), "Title");
        content.Add(new StringContent(blogPostDto.Content), "Content");
        content.Add(new StringContent(blogPostDto.AuthorId.ToString()), "AuthorId");

        if (blogPostDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await blogPostDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(blogPostDto.Image.ContentType);
            content.Add(imageContent, "Image", blogPostDto.Image.FileName);
        }

        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PostAsync("/api/BlogPost", content);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteBlogPostAsync(Guid id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.DeleteAsync($"/api/BlogPost/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<BlogPostViewModel> GetBlogPostAsync(Guid postId)
    {
        var response = await _dataHttpClient.GetAsync($"/api/BlogPost/{postId}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<BlogPostViewModel>();
        return result;
    }

    public async Task<List<BlogPostViewModel>> GetBlogPostsAsync()
    {
        var response = await _dataHttpClient.GetAsync("/api/BlogPost");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<BlogPostViewModel>>();
        return result;
    }

    public async Task UpdateBlogPostAsync(Guid id, BlogPostUpdateDto blogPostUpdateDto)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(blogPostUpdateDto.Title), "Title");
        content.Add(new StringContent(blogPostUpdateDto.Content), "Content");
        content.Add(new StringContent(blogPostUpdateDto.AuthorId.ToString()), "AuthorId");
        content.Add(new StringContent(id.ToString()), "Id");

        if (blogPostUpdateDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await blogPostUpdateDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(blogPostUpdateDto.Image.ContentType);
            content.Add(imageContent, "Image", blogPostUpdateDto.Image.FileName);
        }

        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsync($"/api/BlogPost/{id}", content);
        await response.EnsureSuccessStatusCodeWithApiError();
    }
}
