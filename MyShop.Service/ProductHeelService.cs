using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IProductHeelService
    {
        ProductHeel Add(ProductHeel ProductHeel);

        void Update(ProductHeel ProductHeel);

        ProductHeel Delete(int id);

        void DeleteMulti(int id);

        IEnumerable<ProductHeel> GetAll();

        IEnumerable<ProductHeel> GetAll(string keyword);

        ProductHeel GetById(int id);

        void Save();
    }
    class ProductHeelService : IProductHeelService
    {
        private IProductHeelRepository _productHeelRepository;
        private IUnitOfWork _unitOfWork;

        public ProductHeelService(IProductHeelRepository productHeelRepository, IUnitOfWork unitOfWork)
        {
            this._productHeelRepository = productHeelRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductHeel Add(ProductHeel productHeel)
        {
            return _productHeelRepository.Add(productHeel);
        }

        public ProductHeel Delete(int id)
        {
            return _productHeelRepository.Delete(id);
        }

        public void DeleteMulti(int id)
        {
            _productHeelRepository.DeleteMulti(x => x.ProductId == id);
        }

        public IEnumerable<ProductHeel> GetAll()
        {
            return _productHeelRepository.GetAll();
        }

        public IEnumerable<ProductHeel> GetAll(string keyword)
        {
            return _productHeelRepository.GetAll();
        }

        public ProductHeel GetById(int id)
        {
            return _productHeelRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductHeel productHeel)
        {
            _productHeelRepository.Update(productHeel);
        }
    }
}
