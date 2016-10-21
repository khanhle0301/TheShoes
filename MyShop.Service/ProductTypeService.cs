using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IProductTypeService
    {
        ProductType Add(ProductType ProductType);

        void Update(ProductType ProductType);

        ProductType Delete(int id);

        void DeleteMulti(int id);

        IEnumerable<ProductType> GetAll();

        IEnumerable<ProductType> GetAll(string keyword);

        ProductType GetById(int id);

        void Save();
    }
    public class ProductTypeService : IProductTypeService
    {
        private IProductTypeRepository _productTypeRepository;
        private IUnitOfWork _unitOfWork;

        public ProductTypeService(IProductTypeRepository productTypeRepository, IUnitOfWork unitOfWork)
        {
            this._productTypeRepository = productTypeRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductType Add(ProductType productType)
        {
            return _productTypeRepository.Add(productType);
        }

        public ProductType Delete(int id)
        {
            return _productTypeRepository.Delete(id);
        }

        public void DeleteMulti(int id)
        {
            _productTypeRepository.DeleteMulti(x => x.ProductId == id);
        }

        public IEnumerable<ProductType> GetAll()
        {
            return _productTypeRepository.GetAll();
        }

        public IEnumerable<ProductType> GetAll(string keyword)
        {
            return _productTypeRepository.GetAll();
        }

        public ProductType GetById(int id)
        {
            return _productTypeRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductType productType)
        {
            _productTypeRepository.Update(productType);
        }
    }
}
