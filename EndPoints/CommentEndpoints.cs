﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using WebAssignment3.Data;
using WebAssignment3.Models;

namespace WebAssignment3.EndPoints
{
    public static class CommentEndpoints
    {
        public static void MapCommentEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Comment").WithTags(nameof(Comment));

            group.MapGet("/", async (MyDatabaseContext db) =>
            {
                return await db.Comments.ToListAsync();
            })
            .WithName("GetAllComments")
            .WithOpenApi();

            group.MapGet("/{id}", async Task<Results<Ok<Comment>, NotFound>> (int id, MyDatabaseContext db) =>
            {
                return await db.Comments.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id)
                    is Comment model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            })
            .WithName("GetCommentById")
            .WithOpenApi();

            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Comment comment, MyDatabaseContext db) =>
            {
                // Find the existing comment by its ID
                var existingComment = await db.Comments.FindAsync(id);
                if (existingComment == null)
                {
                    return TypedResults.NotFound();
                }

                // Update the properties of the existing comment
                existingComment.ProductId = comment.ProductId;
                existingComment.UserId = comment.UserId;
                existingComment.Rating = comment.Rating;
                existingComment.Image = comment.Image;
                existingComment.Text = comment.Text;

                // Save the changes to the database
                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithName("UpdateComment")
            .WithOpenApi();

            group.MapPost("/", async (Comment comment, MyDatabaseContext db) =>
            {
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Comment/{comment.Id}", comment);
            })
            .WithName("CreateComment")
            .WithOpenApi();

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MyDatabaseContext db) =>
            {
                var affected = await db.Comments
                    .Where(model => model.Id == id)
                    .ExecuteDeleteAsync();
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("DeleteComment")
            .WithOpenApi();
        }
    }
}
