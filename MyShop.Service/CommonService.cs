using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Common;
using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;

namespace MyShop.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
        ContactDetail GetContactDetail();
        IEnumerable<Slide> GetSlides();
        IEnumerable<Banner> GetBanners();
        IEnumerable<Tag> GetPostTags();
        IEnumerable<Material> GetMaterial();
        Tag GetById(string id);
    }
    public class CommonService : ICommonService
    {
        IFooterRepository _footerRepository;       
        IUnitOfWork _unitOfWork;
        ISlideRepository _slideRepository;
        IBannerRepository _bannerRepository;
        ITagRepository _tagRepository;
        IMaterialRepository _materialRepository;
        IContactDetailRepository _contactDetailRepository;
        public CommonService(IFooterRepository footerRepository,IUnitOfWork unitOfWork,
            ISlideRepository slideRepository, 
            IBannerRepository bannerRepository, ITagRepository tagRepository, IMaterialRepository materialRepository,
        IContactDetailRepository contactDetailRepository)
        {
            _materialRepository = materialRepository;
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;          
            _slideRepository = slideRepository;
            _bannerRepository = bannerRepository;
            _tagRepository = tagRepository;
            _contactDetailRepository = contactDetailRepository;
        }

        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x=>x.Status);
        }

        public IEnumerable<Banner> GetBanners()
        {
            return _bannerRepository.GetMulti(x => x.Status);
        }

        public IEnumerable<Tag> GetPostTags()
        {
            return _tagRepository.GetMulti(x => x.Type== "post");
        }

        public Tag GetById(string id)
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == id);
        }

        public ContactDetail GetContactDetail()
        {
            return _contactDetailRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultContactDetailId);
        }

        public IEnumerable<Material> GetMaterial()
        {
            return _materialRepository.GetAll();
        }
    }
}
