using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IProductColorService
    {
        ProductColor Add(ProductColor ProductColor);

        void Update(ProductColor ProductColor);

        ProductColor Delete(int id);

        void DeleteMulti(int id);

        IEnumerable<ProductColor> GetAll();

        IEnumerable<ProductColor> GetAll(string keyword);

        ProductColor GetById(int id);

        void Save();
    }

    public class ProductColorService : IProductColorService
    {
        private IProductColorRepository _productColorRepository;
        private IUnitOfWork _unitOfWork;

        public ProductColorService(IProductColorRepository productColorRepository, IUnitOfWork unitOfWork)
        {
            this._productColorRepository = productColorRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductColor Add(ProductColor productColor)
        {
            return _productColorRepository.Add(productColor);
        }

        public ProductColor Delete(int id)
        {
            return _productColorRepository.Delete(id);
        }

        public void DeleteMulti(int id)
        {
            _productColorRepository.DeleteMulti(x => x.ProductID == id);
        }

        public IEnumerable<ProductColor> GetAll()
        {
            return _productColorRepository.GetAll();
        }

        public IEnumerable<ProductColor> GetAll(string keyword)
        {
            return _productColorRepository.GetAll();
        }

        public ProductColor GetById(int id)
        {
            return _productColorRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductColor productColor)
        {
            _productColorRepository.Update(productColor);
        }
    }
}