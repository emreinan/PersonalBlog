using App.Shared.Models;

namespace App.Shared.Services.Comment;

public interface ICommentService
{
    Task<List<CommentViewModel>> GetCommentsForPost(Guid postId);
    Task<CommentViewModel> CreateComment(CommentViewModel comment);
}

