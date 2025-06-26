using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewVote> ReviewVotes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasMany(b => b.Reviews)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasMany(r => r.Votes)
                .WithOne(v => v.Review)
                .HasForeignKey(v => v.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReviewVote>()
            .HasIndex(v => new { v.ReviewId, v.UserId })
            .IsUnique();

            builder.Entity<ReviewVote>()
                .HasOne(v => v.Review)
                .WithMany(r => r.Votes)
                .HasForeignKey(v => v.ReviewId)
                .OnDelete(DeleteBehavior.Cascade); // If review is deleted, votes go too

            builder.Entity<ReviewVote>()
                .HasOne(v => v.User)
                .WithMany(u => u.ReviewVotes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: delete votes when user is deleted

            // Unique vote per review/user
            builder.Entity<ReviewVote>()
                .HasIndex(v => new { v.ReviewId, v.UserId })
                .IsUnique();

            // One review per book per user
            builder.Entity<Review>()
                .HasIndex(r => new { r.BookId, r.UserId })
                .IsUnique();

            builder.Entity<ReviewVote>()
                .HasIndex(v => new { v.ReviewId, v.UserId })
                .IsUnique();

            builder.Entity<Book>().HasData(
            new Book { Id = Guid.Parse("ACA4835F-7B6F-495C-9781-FD383E9B8983"), Title = "1984", Author = "George Orwell", PublishedYear = 1949, Genre = "Dystopian" },
            new Book { Id = Guid.Parse("D5FEC283-A357-4C9A-98E0-E59F54889ECD"), Title = "The Hobbit", Author = "J.R.R. Tolkien", PublishedYear = 1937, Genre = "Fantasy" }
            );
        }
    }
}
