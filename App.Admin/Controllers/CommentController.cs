using App.Shared.Dto.Comment;
using App.Shared.Models;
using App.Shared.Services.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("Comment")]
public class CommentController(ICommentService commentService) : Controller
{
    [HttpGet("Comments")]
    public async Task<IActionResult> Comments()
    {
        var comments = await commentService.GetCommentsAsync();
        return View(comments);
    }

    [Authorize]
    [HttpGet("PostComment/{postId}")]
    public async Task<IActionResult> PostComment(Guid postId)
    {
        var comments = await commentService.GetCommentsForPostAsync(postId);
        return View(comments);
    }

    [Authorize]
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await commentService.GetCommentByIdAsync(id);
        var commentUpdateViewModel = new CommentUpdateViewModel { Id = comment.Id, Content = comment.Content };

        return View(commentUpdateViewModel);
    }
    [Authorize]
    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] CommentUpdateViewModel comment)
    {
        if (!ModelState.IsValid)
            return View(comment);

        var commentUpdateDto = new CommentUpdateDto { Content = comment.Content };
        await commentService.UpdateCommentAsync(id, commentUpdateDto);

        TempData["SuccessMessage"] = "Comment updated successfully.";
        return RedirectToAction(nameof(Comments));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await commentService.DeleteCommentAsync(id);

        TempData["SuccessMessage"] = "Comment deleted successfully.";
        return RedirectToAction(nameof(Comments));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Approve/{id}")]
    public async Task<IActionResult> ApproveComment(int id)
    {
        await commentService.ApproveCommentAsync(id);

        TempData["SuccessMessage"] = "Comment approved successfully.";
        return RedirectToAction(nameof(Comments));
    }
}



