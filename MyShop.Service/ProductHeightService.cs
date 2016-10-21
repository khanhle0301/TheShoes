using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IProductHeightService
    {
        ProductHeight Add(ProductHeight ProductHeight);

        void Update(ProductHeight ProductHeight);

        ProductHeight Delete(int id);

        void DeleteMulti(int id);

        IEnumerable<ProductHeight> GetAll();

        IEnumerable<ProductHeight> GetAll(string keyword);

        ProductHeight GetById(int id);

        void Save();
    }
    public class ProductHeightService : IProductHeightService
    {
        private IProductHeightRepository _productHeightRepository;
        private IUnitOfWork _unitOfWork;

        public ProductHeightService(IProductHeightRepository productHeightRepository, IUnitOfWork unitOfWork)
        {
            this._productHeightRepository = productHeightRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductHeight Add(ProductHeight productHeight)
        {
            return _productHeightRepository.Add(productHeight);
        }

        public ProductHeight Delete(int id)
        {
            return _productHeightRepository.Delete(id);
        }

        public void DeleteMulti(int id)
        {
            _productHeightRepository.DeleteMulti(x => x.ProductId == id);
        }

        public IEnumerable<ProductHeight> GetAll()
        {
            return _productHeightRepository.GetAll();
        }

        public IEnumerable<ProductHeight> GetAll(string keyword)
        {
            return _productHeightRepository.GetAll();
        }

        public ProductHeight GetById(int id)
        {
            return _productHeightRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductHeight productHeight)
        {
            _productHeightRepository.Update(productHeight);
        }
    }
}