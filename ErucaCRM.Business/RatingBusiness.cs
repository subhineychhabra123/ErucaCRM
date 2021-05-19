using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;
namespace ErucaCRM.Business
{
    public class RatingBusiness : IRatingBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RatingRepository ratingRepository;
        public RatingBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ratingRepository = new RatingRepository(unitOfWork);
        }

        List<RatingModel> IRatingBusiness.GetRatings()
        {
            List<RatingModel> ratingModelList = new List<RatingModel>();
            List<Rating> ratingList = ratingRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(ratingList, ratingModelList);
            return ratingModelList;
        }

        RatingModel IRatingBusiness.GetRaging(int ratingId)
        {
            RatingModel ratingModel = new RatingModel();
            Rating rating = ratingRepository.SingleOrDefault(x => x.RatingId == ratingId);
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        public RatingModel GetRatingByRatingId(int ratingId)
        {
            RatingModel ratingModel = new RatingModel();
            Rating rating = ratingRepository.SingleOrDefault(x => x.RatingId == ratingId);
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        public RatingModel GetDefaultRating()
        {
            RatingModel ratingModel = new RatingModel();
            Rating rating = ratingRepository.GetAll().FirstOrDefault();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        public RatingModel GetRatingByRatingConstant(int companyId, int ratingConstant)
        {
            RatingModel ratingModel = new RatingModel();
            Rating rating = ratingRepository.SingleOrDefault(x => x.RatingConstant == ratingConstant && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;

        }
    }
}
