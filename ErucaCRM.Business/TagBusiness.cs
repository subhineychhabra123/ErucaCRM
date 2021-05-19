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
    public class TagBusiness : ITagBusiness
    {

        private readonly TagRepository tagRepository;
        private readonly AccountTagRepository accountTagRepository;
        private readonly ContactTagRepository contactTagRepository;
        private readonly LeadTagRepository leadTagRepository;
        public TagBusiness(IUnitOfWork _unitOfWork)
        {
            tagRepository = new TagRepository(_unitOfWork);
            accountTagRepository = new AccountTagRepository(_unitOfWork);
            contactTagRepository = new ContactTagRepository(_unitOfWork);
            leadTagRepository= new LeadTagRepository(_unitOfWork);
        }

        public bool AddTag(TagModel tagModel)
        {
            Tag tag = new Tag();

            bool isExists = tagRepository.Exists(t => t.TagName.ToLower() == tagModel.TagName.ToLower() && (t.CompanyId == tagModel.CompanyId) && (t.RecordDeleted == false));
            if (!isExists)
            {
                tag = new Tag();

                AutoMapper.Mapper.Map(tagModel, tag);
                tagRepository.Insert(tag);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateTag(TagModel tagModel)
        {
            Tag tag = new Tag();

            bool isExists = tagRepository.Exists(t => t.TagName.ToLower() == tagModel.TagName.ToLower() && t.RecordDeleted == false && t.TagId != tagModel.TagId && (t.CompanyId == tagModel.CompanyId));
            if (!isExists)
            {
                tag = tagRepository.SingleOrDefault(x => x.TagId == tagModel.TagId);
                if (tag != null)
                {

                    AutoMapper.Mapper.Map(tagModel, tag);

                    tagRepository.Update(tag);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public TagModel GetTagDetails(int tagId)
        {
            TagModel tagModel = tagRepository.GetAll(x => x.TagId == tagId).Select(x => new TagModel() { TagId = x.TagId, TagName = x.TagName, Description = x.Description }).FirstOrDefault();

            return tagModel;

        }

        /// <summary>
        /// This function will update the tag as deleted in DB and wll de-associate modules 
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteTag(int tagId, int userId)
        {

            Tag tag = new Tag();


            tag = tagRepository.SingleOrDefault(x => x.TagId == tagId);
            if (tag != null)
            {

                tag.RecordDeleted = true;
                tag.ModifiedBy = userId;
                tag.ModifiedDate = DateTime.Now;

                //Delete tag associated Accounts
                accountTagRepository.Delete(x => x.TagId == tag.TagId);
                //Delete tag associated Contacts
                contactTagRepository.Delete(x => x.TagId == tag.TagId);

                //Delete tag associatd with lead
                leadTagRepository.Delete(x => x.TagId == tag.TagId);

                //update tag in db as deleted
                tagRepository.Update(tag);

                return true;
            }
            else
            {
                return false;
            }

        }

        public List<AutoCompleteModel> GetSearchTags(int companyId, string tagSearchText)
        {
            List<AutoCompleteModel> listAutoCompleteTagsModel = new List<AutoCompleteModel>();


            listAutoCompleteTagsModel = tagRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.TagName.Contains(tagSearchText)).Select(y => new AutoCompleteModel() { label = y.TagName, value = y.TagId.Encrypt() }).ToList();
            // AutoMapper.Mapper.Map(listTags, listTagsModel);
            return listAutoCompleteTagsModel;
        }

        public List<TagModel> GetAllTags(int companyId, int currentPage, Int16 pageSize, ref int totalRecords)
        {
            List<TagModel> listTagsModel = new List<TagModel>();

            totalRecords = tagRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
            List<Tag> listTags = tagRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.TagName, currentPage, 100).ToList();
            AutoMapper.Mapper.Map(listTags, listTagsModel);
            return listTagsModel;
        }
    }
}
