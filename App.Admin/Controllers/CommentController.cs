﻿using App.Shared.Dto.Comment;
using App.Shared.Models;
using App.Shared.Services.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace App.Admin.Controllers;

[Route("Comment")]
public class CommentController(ICommentService commentService) : Controller
{
    [HttpGet("Comments")]
    public async Task<IActionResult> Comments()
    {
        var comments = await commentService.GetComments();
        if (comments is null)
        {
            ViewBag.Error = "No comments found.";
            return View();
        }

        return View(comments);
    }

    [HttpGet("PostComment/{postId}")]
    public async Task<IActionResult> GetCommentsForPost(Guid postId)
    {
        var comments = await commentService.GetCommentsForPost(postId);
        if (comments is null)
        {
            ViewBag.Error = "No comments found for this post.";
            return View();
        }

        return Ok(comments);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await commentService.GetCommentById(id);
        if (comment is null)
        {
            ViewBag.Error = "Comment not found.";
            return View();
        }
        var commentUpdateViewModel = new CommentUpdateViewModel { Id = comment.Id, Content = comment.Content };

        return View(commentUpdateViewModel);
    }

    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] CommentUpdateViewModel comment)
    {
        if (!ModelState.IsValid)
            return View(comment);

        var commentUpdateDto = new CommentUpdateDto { Content = comment.Content };
        await commentService.UpdateComment(id, commentUpdateDto);

        return RedirectToAction(nameof(Comments));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await commentService.DeleteComment(id);

        return RedirectToAction(nameof(Comments));
    }

    [HttpGet("Approve/{id}")]
    public async Task<IActionResult> ApproveComment(int id)
    {
        await commentService.ApproveComment(id);

        return RedirectToAction(nameof(Comments));
    }
}



