using MyShop.Common.Exceptions;
using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface ISizeService
    {
        Size Add(Size size);

        void Update(Size Size);

        Size Delete(int id);

        IEnumerable<Size> GetAll();

        IEnumerable<Size> GetAll(string keyword);

        Size GetById(int id);

        IEnumerable<Size> GetListSizeByProductId(int id);

        void Save();
    }

    public class SizeService : ISizeService
    {
        private ISizeRepository _sizeRepository;
        private IUnitOfWork _unitOfWork;

        public SizeService(ISizeRepository sizeRepository, IUnitOfWork unitOfWork)
        {
            this._sizeRepository = sizeRepository;
            this._unitOfWork = unitOfWork;
        }

        public Size Add(Size size)
        {
            if (_sizeRepository.CheckContains(x => x.Name == size.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _sizeRepository.Add(size);
        }

        public Size Delete(int id)
        {
            return _sizeRepository.Delete(id);
        }

        public IEnumerable<Size> GetAll()
        {
            return _sizeRepository.GetAll();
        }

        public IEnumerable<Size> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _sizeRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _sizeRepository.GetAll();
        }

        public Size GetById(int id)
        {
            return _sizeRepository.GetSingleByCondition(x => x.ID == id);
        }

        public IEnumerable<Size> GetListSizeByProductId(int id)
        {
            return _sizeRepository.GetListSizeByProductId(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Size Size)
        {
            if (_sizeRepository.CheckContains(x => x.Name == Size.Name && x.ID != Size.ID))
                throw new NameDuplicatedException("Tên không được trùng");
            _sizeRepository.Update(Size);
        }
    }
}