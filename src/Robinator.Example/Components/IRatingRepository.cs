using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Example.Components
{
    public interface IRatingRepository
    {
        RatingViewModel GetRating(IRateable rateable);
        Task<RatingViewModel> GetRatingAsync(IRateable rateable, CancellationToken token = default);
        void AddRating(IRateable rateable, int stars, ClaimsPrincipal user);
        Task AddRatingAsync(IRateable rateable, int stars, ClaimsPrincipal user, CancellationToken token = default);
    }
}
