using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;


namespace MyShop.Service
{
    public interface IFooterService
    {
        Footer Add(Footer Footer);

        void Update(Footer Footer);

        Footer Delete(int id);

        IEnumerable<Footer> GetAll();

        IEnumerable<Footer> GetAll(string keyword);

        Footer GetById(int id);

        void Save();
    }

    public class FooterService : IFooterService
    {
        private IFooterRepository _footerRepository;
        private IUnitOfWork _unitOfWork;

        public FooterService(IFooterRepository footerRepository, IUnitOfWork unitOfWork)
        {
            this._footerRepository = footerRepository;
            this._unitOfWork = unitOfWork;
        }

        public Footer Add(Footer Footer)
        {
            return _footerRepository.Add(Footer);
        }

        public Footer Delete(int id)
        {
            return _footerRepository.Delete(id);
        }

        public IEnumerable<Footer> GetAll()
        {
            return _footerRepository.GetAll();
        }

        public IEnumerable<Footer> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _footerRepository.GetMulti(x => x.Content.Contains(keyword));
            else
                return _footerRepository.GetAll();
        }

        public Footer GetById(int id)
        {
            return _footerRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Footer Footer)
        {
            _footerRepository.Update(Footer);
        }
    }
}