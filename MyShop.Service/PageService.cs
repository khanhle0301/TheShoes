using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyShop.Service
{
    public interface IPageService
    {
        Page Add(Page Page);

        void Update(Page Page);

        Page Delete(int id);

        IEnumerable<Page> GetAll();

        IEnumerable<Page> GetAllHome();

        IEnumerable<Page> GetAll(string keyword);

        Page GetById(int id);

        Page GetByAlias(string alias);

        void Save();
    }

    public class PageService : IPageService
    {
        private IPageRepository _pageRepository;
        private IUnitOfWork _unitOfWork;

        public PageService(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            this._pageRepository = pageRepository;
            this._unitOfWork = unitOfWork;
        }

        public Page Add(Page Page)
        {
            return _pageRepository.Add(Page);
        }

        public Page Delete(int id)
        {
            return _pageRepository.Delete(id);
        }

        public IEnumerable<Page> GetAll()
        {
            return _pageRepository.GetAll();
        }

        public IEnumerable<Page> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _pageRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _pageRepository.GetAll();
        }

        public Page GetById(int id)
        {
            return _pageRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Page Page)
        {
            _pageRepository.Update(Page);
        }

        public Page GetByAlias(string alias)
        {
            return _pageRepository.GetSingleByCondition(x => x.Alias == alias);
        }

        public IEnumerable<Page> GetAllHome()
        {
            return _pageRepository.GetMulti(x => x.Status).OrderBy(x => x.DisplayOrder);
        }
    }
}