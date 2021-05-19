using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IRatingBusiness
    {
        List<RatingModel> GetRatings();
        RatingModel GetRaging(int ratingId);
        RatingModel GetDefaultRating();
        RatingModel GetRatingByRatingId(int ratingId);
        RatingModel GetRatingByRatingConstant(int companyId, int ratingConstant);
    }
}
