using App.Shared.Dto.BlogPost;
using App.Shared.Dto.File;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace App.Shared.Services.BlogPost;

public class BlogPostService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IBlogPostService
{
    public async Task CreateBlogPostAsync(BlogPostDto blogPostDto)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(blogPostDto.Title), "Title" },
            { new StringContent(blogPostDto.Content), "Content" },
            { new StringContent(blogPostDto.AuthorId.ToString()), "AuthorId" }
        };

        if (blogPostDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await blogPostDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(blogPostDto.Image.ContentType);
            content.Add(imageContent, "Image", blogPostDto.Image.FileName);
        }

        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PostAsync("/api/BlogPost", content);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task DeleteBlogPostAsync(Guid id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.DeleteAsync($"/api/BlogPost/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<BlogPostViewModel> GetBlogPostAsync(Guid postId)
    {
        var response = await _apiHttpClient.GetAsync($"/api/BlogPost/{postId}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<BlogPostViewModel>() ?? 
            throw new DeserializationException("Failed to deserialize BlogPostViewModel from response content.");
        return result;
    }

    public async Task<List<BlogPostViewModel>> GetBlogPostsAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/BlogPost");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<List<BlogPostViewModel>>() ??
            throw new DeserializationException("Failed to deserialize List<BlogPostViewModel> from response content.");
        return result;
    }

    public async Task UpdateBlogPostAsync(Guid id, BlogPostUpdateDto blogPostUpdateDto)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(blogPostUpdateDto.Title), "Title" },
            { new StringContent(blogPostUpdateDto.Content), "Content" },
            { new StringContent(blogPostUpdateDto.AuthorId.ToString()), "AuthorId" },
            { new StringContent(id.ToString()), "Id" }
        };

        if (blogPostUpdateDto.Image != null)
        {
            using var memoryStream = new MemoryStream();
            await blogPostUpdateDto.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var imageContent = new ByteArrayContent(memoryStream.ToArray());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(blogPostUpdateDto.Image.ContentType);
            content.Add(imageContent, "Image", blogPostUpdateDto.Image.FileName);
        }

        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"/api/BlogPost/{id}", content);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }
}
