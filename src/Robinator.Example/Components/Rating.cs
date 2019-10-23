using Microsoft.AspNetCore.Identity;
using System;

namespace Robinator.Example.Components
{
    public class Rating
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int Stars { get; set; }
        public Guid ContentId { get; set; }
    }
}
