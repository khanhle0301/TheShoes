using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IProductMaterialService
    {
        ProductMaterial Add(ProductMaterial ProductMaterial);

        void Update(ProductMaterial ProductMaterial);

        ProductMaterial Delete(int id);

        void DeleteMulti(int id);

        IEnumerable<ProductMaterial> GetAll();

        IEnumerable<ProductMaterial> GetAll(string keyword);

        ProductMaterial GetById(int id);

        void Save();
    }

    public class ProductMaterialService : IProductMaterialService
    {
        private IProductMaterialRepository _productMaterialRepository;
        private IUnitOfWork _unitOfWork;

        public ProductMaterialService(IProductMaterialRepository productMaterialRepository, IUnitOfWork unitOfWork)
        {
            this._productMaterialRepository = productMaterialRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductMaterial Add(ProductMaterial productMaterial)
        {
            return _productMaterialRepository.Add(productMaterial);
        }

        public ProductMaterial Delete(int id)
        {
            return _productMaterialRepository.Delete(id);
        }

        public void DeleteMulti(int id)
        {
            _productMaterialRepository.DeleteMulti(x => x.ProductID == id);
        }

        public IEnumerable<ProductMaterial> GetAll()
        {
            return _productMaterialRepository.GetAll();
        }

        public IEnumerable<ProductMaterial> GetAll(string keyword)
        {
            return _productMaterialRepository.GetAll();
        }

        public ProductMaterial GetById(int id)
        {
            return _productMaterialRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductMaterial productMaterial)
        {
            _productMaterialRepository.Update(productMaterial);
        }
    }
}