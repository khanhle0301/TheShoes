using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;


namespace MyShop.Service
{
    public interface IProductSizeService
    {
        ProductSize Add(ProductSize ProductSize);

        void Update(ProductSize ProductSize);

        ProductSize Delete(int id);

        void DeleteMulti(int id);

        IEnumerable<ProductSize> GetAll();

        IEnumerable<ProductSize> GetAll(string keyword);

        ProductSize GetById(int id);

        void Save();
    }
    public class ProductSizeService : IProductSizeService
    {
        private IProductSizeRepository _productSizeRepository;
        private IUnitOfWork _unitOfWork;

        public ProductSizeService(IProductSizeRepository productSizeRepository, IUnitOfWork unitOfWork)
        {
            this._productSizeRepository = productSizeRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductSize Add(ProductSize productSize)
        {
            return _productSizeRepository.Add(productSize);
        }

        public ProductSize Delete(int id)
        {
            return _productSizeRepository.Delete(id);
        }

        public void DeleteMulti(int id)
        {
            _productSizeRepository.DeleteMulti(x => x.ProductID == id);
        }

        public IEnumerable<ProductSize> GetAll()
        {
            return _productSizeRepository.GetAll();
        }

        public IEnumerable<ProductSize> GetAll(string keyword)
        {
            return _productSizeRepository.GetAll();
        }

        public ProductSize GetById(int id)
        {
            return _productSizeRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductSize productSize)
        {
            _productSizeRepository.Update(productSize);
        }
    }
}