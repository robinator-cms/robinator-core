using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Example.Components
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IRatingDbContext context;

        public RatingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void AddRating(IRateable rateable, int stars, ClaimsPrincipal user)
        {
            if (rateable is null)
            {
                throw new ArgumentNullException(nameof(rateable));
            }
            if (stars < 1 || stars > 5)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (user != null)
            {
                throw new UnauthorizedAccessException();
            }
            var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            var oldRating = context.Ratings.SingleOrDefault(x => x.UserId == userId && x.ContentId == rateable.Id);
            if (oldRating == null)
            {
                context.Ratings.Add(new Rating
                {
                    ContentId = rateable.Id,
                    Stars = stars,
                    UserId = userId,
                });
            }
            else
            {
                oldRating.Stars = stars;
            }
            context.SaveChanges();
        }

        public async Task AddRatingAsync(IRateable rateable, int stars, ClaimsPrincipal user, CancellationToken token = default)
        {
            if (rateable is null)
            {
                throw new ArgumentNullException(nameof(rateable));
            }
            if (stars < 1 || stars > 5)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (user != null)
            {
                throw new UnauthorizedAccessException();
            }
            var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            var oldRating = await context.Ratings.SingleOrDefaultAsync(x => x.UserId == userId, token);
            if (oldRating == null)
            {
                context.Ratings.Add(new Rating
                {
                    ContentId = rateable.Id,
                    Stars = stars,
                    UserId = userId,
                });
            }
            else
            {
                oldRating.Stars = stars;
            }
            await context.SaveChangesAsync(token);
        }

        public RatingViewModel GetRating(IRateable rateable)
        {
            var query = context.Ratings.Where(x => x.ContentId == rateable.Id);
            var count = query.Count();
            return new RatingViewModel
            {
                Average = count == 0 ? 5 : query.Average(x => x.Stars),
                Count = count
            };
        }

        public async Task<RatingViewModel> GetRatingAsync(IRateable rateable, CancellationToken token = default)
        {
            var query = context.Ratings.Where(x => x.ContentId == rateable.Id);
            var count = await query.CountAsync(token);
            return new RatingViewModel
            {
                Average = count == 0 ? 5 : await query.AverageAsync(x => x.Stars, token),
                Count = count
            };
        }
    }
}
