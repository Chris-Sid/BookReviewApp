using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Contracts.DTOs
{
    public record ReviewDTO1(Guid Id, string Content, int Rating, DateTime DateCreated, string? UserName);
}
