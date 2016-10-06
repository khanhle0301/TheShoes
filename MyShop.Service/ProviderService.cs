using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IProviderService
    {
        Provider Add(Provider Provider);

        void Update(Provider Provider);

        Provider Delete(int id);

        IEnumerable<Provider> GetAll();

        IEnumerable<Provider> GetAll(string keyword);

        Provider GetById(int id);

        void Save();
    }
   public class ProviderService : IProviderService
    {
        private IProviderRepository _providerRepository;
        private IUnitOfWork _unitOfWork;

        public ProviderService(IProviderRepository providerRepository, IUnitOfWork unitOfWork)
        {
            this._providerRepository = providerRepository;
            this._unitOfWork = unitOfWork;
        }

        public Provider Add(Provider Provider)
        {
            return _providerRepository.Add(Provider);
        }

        public Provider Delete(int id)
        {
            return _providerRepository.Delete(id);
        }

        public IEnumerable<Provider> GetAll()
        {
            return _providerRepository.GetAll();
        }

        public IEnumerable<Provider> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _providerRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _providerRepository.GetAll();
        }


        public Provider GetById(int id)
        {
            return _providerRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Provider Provider)
        {
            _providerRepository.Update(Provider);
        }
    }
}
