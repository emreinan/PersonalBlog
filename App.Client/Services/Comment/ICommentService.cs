using App.Client.Models;

namespace App.Client.Services.Comment;

public interface ICommentService
{
    Task<List<CommentViewModel>> GetCommentsForPost(Guid postId);
    Task<CommentViewModel> CreateComment(CommentViewModel comment);
}

