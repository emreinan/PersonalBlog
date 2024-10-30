using App.Shared.Dto.Comment;
using App.Shared.Models;
using Ardalis.Result;

namespace App.Shared.Services.Comment;

public interface ICommentService
{
    Task<List<CommentViewModel>> GetCommentsForPostAsync(Guid postId);
    Task CreateCommentAsync(CommentDto comment);
    Task DeleteCommentAsync(int id);
    Task ApproveCommentAsync(int id);
    Task<List<CommentViewModel>> GetCommentsAsync();
    Task UpdateCommentAsync(int id, CommentUpdateDto commentUpdateDto);
    Task<CommentViewModel> GetCommentByIdAsync(int id);
}

