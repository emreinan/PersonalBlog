using App.Shared.Dto.Comment;
using App.Shared.Models;
using Ardalis.Result;

namespace App.Shared.Services.Comment;

public interface ICommentService
{
    Task<List<CommentViewModel>> GetCommentsForPost(Guid postId);
    Task CreateComment(CommentDto comment);
    Task DeleteComment(int id);
    Task ApproveComment(int id);
    Task<List<CommentViewModel>> GetComments();
    Task UpdateComment(int id, CommentUpdateDto commentUpdateDto);
    Task<CommentViewModel> GetCommentById(int id);
}

