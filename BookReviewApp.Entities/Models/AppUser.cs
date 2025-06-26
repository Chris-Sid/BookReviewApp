using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Entities.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ReviewVote> ReviewVotes { get; set; }
    }
}
